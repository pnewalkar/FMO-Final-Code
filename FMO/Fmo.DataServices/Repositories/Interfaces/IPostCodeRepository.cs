﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;

namespace Fmo.DataServices.Repositories.Interfaces
{
  public interface IPostCodeRepository
    {
        Task<List<PostCodeDTO>> FetchPostCodeUnit(string searchText);
    }
}