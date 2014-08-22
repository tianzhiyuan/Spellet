using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Components;
using Ors.Core.Serialization;

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
    }
}
