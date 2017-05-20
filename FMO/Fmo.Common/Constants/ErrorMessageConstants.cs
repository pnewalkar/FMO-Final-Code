using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Common.Constants
{
    public class ErrorMessageConstants
    {
        public const string SqlAddExceptionMessage = "Error occured on adding new {0}.";
        public const string SqlUpdateExceptionMessage = "Error occured on modifying {0}.";
        public const string SqlDeleteExceptionMessage = "Error occured while deleting {0}.";
        public const string ConcurrencyMessage = "Data you are trying to modify has been already modified by another user.";
        public const string NotSupportedExceptionMessage = "An attempt was made to use unsupported behavior.";
        public const string ObjectDisposedExceptionMessage = "The context or connection have been disposed.";
        public const string EntityNotFoundExceptionMessage = "The entity trying to search cannot be found.";
        public const string ArgumentmentNullExceptionMessage = "Following arguments for the method are null {0}";
        public const string InvalidOperationExceptionMessageForSingleorDefault = "There are more than one element in sequence for SingleOrDefault() extention";
        public const string InvalidOperationExceptionMessageForFirstorDefault = "source does not implement IDbAsyncQueryProvider";
        public const string InvalidOperationExceptionMessageForCountAsync = "The underlying provider does not support this operation."
        public const string OverflowExceptionMessage = "The number of elements in source is larger than MaxValue.";

    }
}
