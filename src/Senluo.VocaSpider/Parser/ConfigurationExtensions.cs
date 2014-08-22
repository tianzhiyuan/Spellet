using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ors.Core.Configurations;

namespace Senluo.VocaSpider.Parser
{
    public static class ConfigurationExtensions
    {
        public static Configuration UseYoudaoParser(this Configuration configuration)
        {
            configuration.SetDefault<IParser, YoudaoParser>();
            return configuration;
        }
    }
}
