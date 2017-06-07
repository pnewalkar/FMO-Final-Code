﻿namespace RM.DataManagement.DeliveryPoint.WebAPI.ExceptionHandling.ResponseExceptionHandler
{
    public class ExceptionResponse
    {
        public int StatusCode
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public object Response
        {
            get;
            set;
        }
    }
}