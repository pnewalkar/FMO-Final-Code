﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;

namespace RM.CommonLibrary.EntityFramework.DTO
{
  public class AccessActionDTO
    {

        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayText { get; set; }

    }
}
