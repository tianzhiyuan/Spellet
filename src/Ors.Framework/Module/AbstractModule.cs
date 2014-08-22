using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Framework.Module
{
    public abstract class AbstractModule:IModule
    {
        public abstract void Initialize();
        public string Name { get; set; }
    }
}
