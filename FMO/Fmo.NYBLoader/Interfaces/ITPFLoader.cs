﻿using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.NYBLoader.Interfaces
{
    public interface ITpfLoader
    {
        void LoadTPFDetailsFromXml(string strPath);
        List<addressLocation> GetValidRecords(string strPath);
    }
}
