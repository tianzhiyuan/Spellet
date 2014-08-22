using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Framework.Plugins
{
    public abstract class AbstractPlugin:IPlugin
    {
        public abstract void Install();
        public abstract void Uninstall();
        public string Name { get; set; }
    }
}
