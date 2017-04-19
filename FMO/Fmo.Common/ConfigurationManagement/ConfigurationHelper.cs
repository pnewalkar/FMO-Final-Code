using System;
using System.Configuration;
using Fmo.Common.Interface;

namespace Fmo.Common.ConfigurationManagement
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public string ReadAppSettingsConfigurationValues(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
}
