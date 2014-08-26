using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Components;
using Ors.Core.Data;

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

        public static bool IsComponent(Type type)
        {
            return type != null && type.IsClass && type.GetCustomAttributes(typeof(ComponentAttribute), false).Any();
        }

        public static bool IsAssemblyInitializer(Type type)
        {
            return type != null && type.IsClass && !type.IsAbstract &&
                   typeof (IAssemblyInitializer).IsAssignableFrom(type);
        }

        public static bool IsModel(Type type)
        {
            return type != null && type.IsClass && !type.IsAbstract && typeof (IModel).IsAssignableFrom(type);
        }
    }
}
