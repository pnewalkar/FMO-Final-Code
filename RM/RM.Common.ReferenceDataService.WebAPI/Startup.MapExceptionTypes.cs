using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using RM.Common.ReferenceData.WebAPI.ExceptionHandling.ResponseExceptionHandler;
using RM.CommonLibrary.ExceptionMiddleware;

namespace RM.Common.ReferenceData.WebAPI
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

                options.Map<ServiceException>(HttpStatusCode.InternalServerError);
                options.Map<DataAccessException>(HttpStatusCode.NotAcceptable);
                options.Map<DbConcurrencyException>(HttpStatusCode.Conflict);
                options.Map<InfrastructureException>(HttpStatusCode.InternalServerError);
                options.Map<BusinessLogicException>(HttpStatusCode.ExpectationFailed);
            });
        }
    }
}