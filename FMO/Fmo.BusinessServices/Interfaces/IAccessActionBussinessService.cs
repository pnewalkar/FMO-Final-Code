﻿using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.BusinessServices.Interfaces
{
   public interface IAccessActionBussinessService
    {
        List<AccessActionDTO> FetchAccessActions();
    }
}