﻿using Newtonsoft.Json;

namespace RM.Common.ActionManager.WebAPI.ExceptionHandling.ResponseExceptionHandler
{
    public class Error
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string ErrorCode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Message { get; set; }
    }
}