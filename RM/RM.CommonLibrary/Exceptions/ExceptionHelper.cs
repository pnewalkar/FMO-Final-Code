﻿using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.ExceptionMiddleware
{
    public class ExceptionHelper : IExceptionHelper
    {
        #region Member Variables

        private static bool isInitialized = false;

        #endregion Member Variables

        #region Public Methods

        public ExceptionHelper()
        {
            if (!isInitialized)
            {
                IConfigurationSource source = ConfigurationSourceFactory.Create();
                LogWriterFactory log = new LogWriterFactory(source);
                LogWriter logWriter = log.Create();
                Logger.SetLogWriter(logWriter, false);
                ExceptionManager exceptionManager = ExceptionHandlingConfiguration.BuildExceptionHandlingConfiguration(logWriter);
                ExceptionPolicy.SetExceptionManager(exceptionManager);
                isInitialized = true;
            }
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="execptionToThrow">The execption to throw.</param>
        /// <returns>bool</returns>
        public bool HandleException(Exception exception, ExceptionHandlingPolicy policy, out Exception execptionToThrow)
        {
            return ExceptionPolicy.HandleException(exception, policy.GetDescription(), out execptionToThrow);
        }

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="policy">The policy.</param>
        /// <returns>bool</returns>
        public bool HandleException(Exception exception, ExceptionHandlingPolicy policy)
        {
            return ExceptionPolicy.HandleException(exception, policy.GetDescription());
        }

        #endregion Public Methods
    }
}