using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ors.Core.Data;
using Senluo.Spellet.Models;

namespace Senluo.GenMySqlScript
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = new Answer();
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sql");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes().Where(o => typeof (IModel).IsAssignableFrom(o)))
                    {
                        var sql = new MySqlScriptGenerator().Go(type);
                        var path = Path.Combine(basePath, type.Name + ".sql");
                        File.WriteAllText(path, sql);
                    }
                }catch{}
            }
        }
    }
}
