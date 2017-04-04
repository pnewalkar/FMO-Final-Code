using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Interfaces
{
    public interface IConfigurationHelper
    {
        string ReadAppSettingsConfigurationValues(string key);
    }
}
