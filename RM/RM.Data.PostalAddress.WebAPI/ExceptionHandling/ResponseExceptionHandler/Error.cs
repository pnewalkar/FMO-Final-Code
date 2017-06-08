﻿using Newtonsoft.Json;

namespace RM.DataManagement.PostalAddress.WebAPI.ExceptionHandling.ResponseExceptionHandler
{
    public class Error
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ErrorCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }
    }
}