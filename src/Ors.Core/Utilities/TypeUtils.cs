using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Utilities
{
    public static class TypeUtils
    {
        public static bool Is(Type me, Type baseType)
        {
            return
                (me.IsGenericType && me.GetGenericTypeDefinition() == baseType)
                || (me.GetInterfaces().Any(o => o.IsGenericType && o.GetGenericTypeDefinition() == baseType))
                || baseType.IsAssignableFrom(me);
        }


    }
}
