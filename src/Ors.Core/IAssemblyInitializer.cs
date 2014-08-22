using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ors.Core
{
    public interface IAssemblyInitializer
    {
        void Initialize(Assembly[] assemblies);
    }
}
