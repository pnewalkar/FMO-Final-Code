using System.Configuration;

namespace RM.CommonLibrary.ConfigurationMiddleware
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public string ReadAppSettingsConfigurationValues(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
}