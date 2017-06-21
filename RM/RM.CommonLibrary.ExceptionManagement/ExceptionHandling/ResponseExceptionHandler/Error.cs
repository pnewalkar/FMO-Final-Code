namespace RM.CommonLibrary.ExceptionManagement.ExceptionHandling.ResponseExceptionHandler
{
    using Newtonsoft.Json;

    public class Error
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ErrorCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }
    }
}