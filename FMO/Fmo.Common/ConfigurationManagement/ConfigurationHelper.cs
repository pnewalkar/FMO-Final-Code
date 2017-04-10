using Fmo.Common.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Common
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public string ReadAppSettingsConfigurationValues(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
