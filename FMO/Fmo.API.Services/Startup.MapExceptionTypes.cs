using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Fmo.API.Services.Authentication;
using System.Collections.Generic;
using Fmo.API.Services.ExceptionHandling.ResponseExceptionHandler;
using Fmo.Common.ExceptionManagement;

namespace Fmo.API.Services
{
    public partial class Startup
    {
        private void MapExceptionTypes(IApplicationBuilder app)
        {
            app.UseResponseExceptionHandler(options =>
            {
                options.Map<ArgumentNullException>(HttpStatusCode.BadRequest);
                options.Map<NotSupportedException>(HttpStatusCode.InternalServerError);
                options.Map<SystemException>(HttpStatusCode.InternalServerError);
                options.Map<ApplicationException>(HttpStatusCode.InternalServerError);
                options.Map<ObjectDisposedException>(HttpStatusCode.RequestedRangeNotSatisfiable);
                options.Map<InvalidOperationException>(HttpStatusCode.BadRequest);
                options.Map<UnauthorizedAccessException>(HttpStatusCode.Unauthorized);

                options.Map<ServiceException>(HttpStatusCode.BadRequest);
                options.Map<DataAccessException>(HttpStatusCode.NotAcceptable);
                options.Map<DbConcurrencyException>(HttpStatusCode.Conflict);
                options.Map<InfrastructureException>(HttpStatusCode.InternalServerError);
                options.Map<BusinessLogicException>(HttpStatusCode.ExpectationFailed);
            });
        }
    }
}
