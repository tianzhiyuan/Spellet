﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Tasks
{
    interface ISheduler
    {
        ITask[] Tasks { get; set; }
    }
}
