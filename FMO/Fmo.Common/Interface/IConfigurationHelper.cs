﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Common.Interface
{
    public interface IConfigurationHelper
    {
        string ReadAppSettingsConfigurationValues(string key);
    }
}