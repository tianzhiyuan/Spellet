using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Senluo.Spellet.EF
{
    public class CreateTableIfNotExists<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        private string _database;
        public CreateTableIfNotExists(string dataBaseName)
        {
            _database = dataBaseName;
        }
        public void InitializeDatabase(TContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                bool createTables = false;
                //check whether tables are already created
                int numberOfTables = 0;
                foreach (var t1 in context.Database.SqlQuery<int>("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE' and TABLE_SCHEMA='"+_database + "' "))
                    numberOfTables = t1;

                createTables = numberOfTables == 0;
                if (createTables)
                {
                    //create all tables
                    var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                    context.Database.ExecuteSqlCommand(dbCreationScript);

                    //Seed(context);
                    context.SaveChanges();

                }
            }
        }

    }
}