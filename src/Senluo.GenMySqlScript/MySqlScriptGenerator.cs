using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Senluo.GenMySqlScript
{
    public class MySqlScriptGenerator
    {
        public string Go<TModel>()
        {
            var modelType = typeof (TModel);
            return Go(modelType);
        }
        public string Go(Type modelType)
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Format("CREATE TABLE IF NOT EXISTS {0}", modelType.Name));
            builder.AppendLine("(");
            foreach (var pi in modelType.GetProperties().Where(o => CanParse(o.PropertyType)))
            {
                var t = pi.PropertyType;
                if (t.IsGenericType)
                {
                    t = t.GetGenericArguments().FirstOrDefault();
                }
                var name = pi.Name;
                string sqlType = "";
                if (name == "ID")
                {
                    if (t == typeof(long))
                    {
                        sqlType = "BIGINT NOT NULL AUTO_INCREMENT";
                    }
                    if (t == typeof(int))
                    {
                        sqlType = "INT NOT NULL AUTO_INCREMENT";
                    }
                    
                }
                else if (t == typeof (string))
                {
                    sqlType = "NVARCHAR(100) NULL";
                }
                else if (t == typeof (bool))
                {
                    sqlType = "BIT(1) NULL";
                }
                else if (t == typeof (DateTime))
                {
                    sqlType = "DATETIME NULL";
                }
                else if (t == typeof (long))
                {
                    sqlType = "BIGINT NULL";
                }
                else if (t == typeof (int))
                {
                    sqlType = "INT NULL";
                }
                else if (t == typeof (decimal))
                {
                    sqlType = "DECIMAL(10,6) NULL";
                }
                builder.AppendLine(string.Format("    {0} {1},", name, sqlType));
            }
            builder.AppendLine("    PRIMARY KEY(ID)");
            builder.AppendLine(")ENGINE=InnoDB DEFAULT CHARACTER SET=utf8 COLLATE=utf8_general_ci;");
            return builder.ToString();
        }
        static readonly IList<Type> _types = new List<Type>()
            {
                typeof(bool),
                typeof(int),
                typeof(long),
                typeof(DateTime),
                typeof(decimal),
            };  
        bool CanParse(Type type)
        {
            if (type == typeof (string)) return true;
            if (_types.Contains(type)) return true;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>) &&
                _types.Contains(type.GetGenericArguments().FirstOrDefault()))
            {
                return true;
            }
            return false;
        }
        
    }
}
