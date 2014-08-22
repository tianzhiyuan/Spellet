using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ors.Core.Serialization
{
    public class Json : IJsonSerializer
    {
        private readonly JsonSerializer _serializer;
        public Json()
        {
            _serializer = new JsonSerializer()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                };
        }
        public string Serialize(object obj)
        {
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return JsonConvert.SerializeObject(obj, settings);
        }

        public TData Deserialize<TData>(string data)
        {
            using (var reader = new JsonTextReader(new StringReader(data)))
            {
                return this._serializer.Deserialize<TData>(reader);
            }
        }
    }
}
