using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Utilities
{
    public static class ObjectUtils
    {
        private static readonly object Locker = new object();
        private static readonly IDictionary<Type, Delegate> Cloners = new Dictionary<Type, Delegate>();

        private static Delegate CreateCloner<TModel>()
        {
            var modelType = typeof(TModel);
            var fromArg = Expression.Parameter(modelType, "from");
            var toArg = Expression.Parameter(modelType, "to");
            var properties =
                modelType.GetProperties()
                         .Where(
                             o =>
                             o.CanRead && o.CanWrite &&
                             (o.PropertyType.IsValueType || TypeUtils.Is(o.PropertyType, typeof (string))))
                         .ToArray();

            var statements = properties.Select(property => Expression.Assign(Expression.Property(toArg, property), Expression.Property(fromArg, property))).Cast<Expression>().ToList();

            Delegate cloner = Expression.Lambda<Action<TModel, TModel>>(Expression.Block(statements), fromArg, toArg).Compile();

            return cloner;
        }
        /// <summary>
        /// Create an object from the source object, assign the properties respectively.
        /// Note: only assign struct or string type properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T Clone<T>(T source) where T : class, new()
        {
            if (source == null) return null;

            var clone = new T();
            var type = typeof(T);
            Delegate cloner;
            if (!Cloners.TryGetValue(type, out cloner))
            {
                lock (Locker)
                {
                    if (!Cloners.TryGetValue(type, out cloner))
                    {
                        cloner = CreateCloner<T>();
                        Cloners.Add(type, cloner);
                    }
                }
            }

            var action = cloner as Action<T, T>;
            if (action != null) action.Invoke(source, clone);

            return clone;
        }
    }
}
