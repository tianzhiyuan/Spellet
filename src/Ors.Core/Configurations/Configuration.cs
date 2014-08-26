using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Components;
using Ors.Core.Serialization;
using Ors.Core.Utilities;

namespace Ors.Core.Configurations
{
    public class Configuration
    {
        private static Configuration _instance = new Configuration();

        private Configuration()
        {
        }

        public static Configuration Instance { get { return _instance; } }

        public Configuration SetDefault<TService, TImplementer>(LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService
        {
            ObjectContainer.Register<TService, TImplementer>(life);
            return this;
        }
        public Configuration SetDefault<TService, TImplementer>(TImplementer instance)
            where TService : class
            where TImplementer : class, TService
        {
            ObjectContainer.RegisterInstance<TService, TImplementer>(instance);
            return this;
        }

        public Configuration RegisterCommon()
        {
            this.SetDefault<IJsonSerializer, Json>();
            return this;
        }

        private readonly IList<Type> _assemblyInitializers = new List<Type>(); 
        
        public Configuration InitializeAssemblies(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes().Where(TypeUtils.IsAssemblyInitializer))
                    {
                        ObjectContainer.RegisterType(type, LifeStyle.Singleton);
                        var initializer = ObjectContainer.Resolve(type) as IAssemblyInitializer;
                        if (initializer != null)
                        {
                            initializer.Initialize(assemblies);
                        }
                    }
                }
                catch { }
            }
            
            return this;
        }
    }
}
