using System;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.ExceptionMiddleware
{
    public interface IExceptionHelper
    {
        bool HandleException(Exception exception, ExceptionHandlingPolicy policy, out Exception execptionToThrow);

        bool HandleException(Exception exception, ExceptionHandlingPolicy policy);
    }
}