using System;
using Fmo.Common.Enums;

namespace Fmo.Common.Interface
{
    public interface IExceptionHelper
    {
        bool HandleException(Exception exception, ExceptionHandlingPolicy policy, out Exception execptionToThrow);

        bool HandleException(Exception exception, ExceptionHandlingPolicy policy);
    }
}
