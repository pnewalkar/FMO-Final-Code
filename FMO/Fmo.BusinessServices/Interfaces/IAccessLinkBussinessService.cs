﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.BusinessServices.Interfaces
{
  public interface IAccessLinkBussinessService
    {
        List<AccessLinkDTO> SearchAccessLink();
    }
}