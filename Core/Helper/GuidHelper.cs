﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public class GuidHelper
    {
        public Guid GenerateNewGuid()
        {
            return Guid.NewGuid();
        }
    }
}
