// Copyright (c) Nate Barbettini. All rights reserved. Licensed under the Apache License, Version
// 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RM.Common.ActionManager.WebAPI.DataDTO;
using RM.Common.ActionManager.WebAPI.DTO;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.ActionManager.WebAPI.Authentication
{
    /// <summary>
    /// Token generator middleware component which is added to an HTTP pipeline. This class is not
    /// created by application code directly, instead it is added by calling the <see
    /// cref="TokenProviderAppBuilderExtensions.UseSimpleTokenProvider(Microsoft.AspNetCore.Builder.IApplicationBuilder,
    /// TokenProviderOptions)"/> extension method.
    /// </summary>
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly TokenProviderOptions options;
        private readonly JsonSerializerSettings serializerSettings;
        private ILoggingHelper loggingHelper;
        private IActionManagerDataService actionManagerService = default(IActionManagerDataService);

        public TokenProviderMiddleware(
           RequestDelegate next,
           IOptions<TokenProviderOptions> options,
           ILoggingHelper loggingHelper,
           IActionManagerDataService actionManagerService)
        {
            this.next = next;
            this.loggingHelper = loggingHelper;

            this.options = options.Value;
            ThrowIfInvalidOptions(this.options);

            this.serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };

            this.actionManagerService = actionManagerService;
        }

        /// <summary>
        /// Get this datetime as a Unix epoch timestamp (seconds since Jan 1, 1970, midnight UTC).
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>Seconds since Unix epoch.</returns>
        internal static long ToUnixEpochDate(DateTime date)
        {
            var timeSpan = new DateTimeOffset(date).ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)timeSpan.TotalSeconds;
        }

        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="context">http context</param>
        /// <returns>Generated token</returns>
        internal Task Invoke(HttpContext context)
        {
            // If the request path doesn't match, skip
            if (!context.Request.Path.Equals(options.Path, StringComparison.Ordinal))
            {
                return next(context);
            }

            // Request must be POST with Content-Type: application/x-www-form-urlencoded
            if (!context.Request.Method.Equals("POST")
               || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }

            return GenerateToken(context);
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));
            }

            if (options.Expiration == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(TokenProviderOptions.Expiration));
            }

            if (options.IdentityResolver == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.IdentityResolver));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
            }

            if (options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(TokenProviderOptions.NonceGenerator));
            }
        }

        /// <summary>
        /// This method generates token depending on the user and selected unit
        /// </summary>
        /// <param name="context">http context</param>
        /// <returns>Token</returns>
        private async Task GenerateToken(HttpContext context)
        {
            try
            {
                var username = context.Request.Form["username"];
                Guid unitGuid;
                string unitType = string.Empty;
                string unitName = string.Empty;
                bool isGuid = Guid.TryParse(context.Request.Form["UnitGuid"], out unitGuid);

                var identity = await options.IdentityResolver(username, unitGuid != null ? unitGuid.ToString() : string.Empty);
                if (identity == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Invalid username or password.");
                    return;
                }

                //Get the Unit dtails for current user. Details would be empty for the user who has access to units above mail center
                UserUnitInfoDataDTO userUnitDetails = await actionManagerService.GetUserUnitInfo(username, unitGuid);

                if (userUnitDetails == null)
                {
                    //Get the Unit details from reference data if current user has access to the units above mail center
                    userUnitDetails = await actionManagerService.GetUserUnitInfoFromReferenceData(username, unitGuid);
                }

                //unitGuid would be empty while loading the application for first time for the current session for the current user
                if (unitGuid == Guid.Empty)
                {
                    unitGuid = userUnitDetails.LocationId;
                }

                UserUnitInfoDataDTO userUnitInfoDto = new UserUnitInfoDataDTO
                {
                    UserName = username,
                    LocationId = unitGuid,
                    UnitType = userUnitDetails.UnitType,
                    UnitName = userUnitDetails.UnitName
                };

                var roleAccessDataDto = await actionManagerService.GetRoleBasedAccessFunctions(userUnitInfoDto);

                var now = DateTime.UtcNow;

                // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user)
                // claims. You can add other claims here, if you want:
                var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, await options.NonceGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.UserData, roleAccessDataDto.FirstOrDefault().Unit_GUID.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.PrimarySid, roleAccessDataDto.FirstOrDefault().UserId.ToString()),
                new Claim(ClaimTypes.Upn, roleAccessDataDto.FirstOrDefault().UnitType.ToString())
            };

                roleAccessDataDto.ForEach(x => claims.Add(new Claim(ClaimTypes.Role, x.FunctionName)));

                // Create the JWT and write it to a string
                var jwt = new JwtSecurityToken(
                    issuer: options.Issuer,
                    audience: options.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(options.Expiration),
                    signingCredentials: options.SigningCredentials);
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                ResponseDTO responseDTO = new ResponseDTO
                {
                    AccessToken = encodedJwt,
                    ExpiresIn = (int)options.Expiration.TotalSeconds,
                    RoleActions = roleAccessDataDto,
                    UserName = username
                };

                // Serialize and return the response
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(responseDTO, serializerSettings));
            }
            catch (Exception ex)
            {
                loggingHelper.Log(ErrorConstants.Err_Token, TraceEventType.Error, ex);
                var result = JsonConvert.SerializeObject(new { error = ErrorConstants.Err_Default });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(result);
            }
        }
    }
}