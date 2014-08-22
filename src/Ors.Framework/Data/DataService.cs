using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Components;
using Ors.Core.Utilities;
using Ors.Framework.Complilation;
using Ors.Core.Data;

namespace Ors.Framework.Data
{
    [Component(LifeStyle.Singleton)]
    public class DataService : IModelService
    {

        public DataService()
        {
            Handlers = new Dictionary<Type, Delegate>();
        }

        public Dictionary<Type, Delegate> Handlers { get; set; }
        private readonly object _contextCreatorLocker = new object();
        private Func<string, DbContext> _createContext;
        public void Update<TModel>(params TModel[] models) where TModel : class, IModel, new()
        {
            if (BeforeUpdate != null)
            {
                BeforeUpdate.Invoke(this, new DataServiceEventArgs(){Items = models});
            }
            using (var db = GetContext())
            {
                foreach (var model in models)
                {
                    var clone = ObjectUtils.Clone(model);
                    var entry = db.Entry<TModel>(clone);
                    entry.State = EntityState.Modified;
                }
                int rowsAffected = db.SaveChanges();
                Debug.Assert(rowsAffected > 0);
            }
            if (AfterUpdate != null)
            {
                AfterUpdate.Invoke(this, new DataServiceEventArgs() {Items = models});
            }
        }

        public void Create<TModel>(params TModel[] models) where TModel : class, IModel, new()
        {
            if (BeforeCreate != null)
            {
                BeforeCreate.Invoke(this, new DataServiceEventArgs() { Items = models });
            }
            using (var db = GetContext())
            {
                var dbSet = db.Set<TModel>();
                var clones = new List<TModel>();
                foreach (var model in models)
                {
                    var clone = ObjectUtils.Clone(model);
                    clones.Add(clone);
                    dbSet.Add(clone);
                }
                var rowsAffected = db.SaveChanges();
                for (var index = 0; index < models.Count(); index++)
                {
                    models[index].ID = clones[index].ID;
                }
                Debug.Assert(rowsAffected > 0);
            }
            if (AfterCreate != null)
            {
                AfterCreate.Invoke(this, new DataServiceEventArgs() { Items = models });
            }
        }

        public event EventHandler<DataServiceEventArgs> BeforeUpdate;
        public event EventHandler<DataServiceEventArgs> AfterUpdate;
        public event EventHandler<DataServiceEventArgs> BeforeDelete;
        public event EventHandler<DataServiceEventArgs> AfterDelete;
        public event EventHandler<DataServiceEventArgs> BeforeCreate;
        public event EventHandler<DataServiceEventArgs> AfterCreate;

        public void Delete<TModel>(params TModel[] models) where TModel : class, IModel, new()
        {
            if (BeforeDelete != null)
            {
                BeforeDelete.Invoke(this, new DataServiceEventArgs() { Items = models });
            }
            using (var db = GetContext())
            {
                foreach (var model in models)
                {
                    var clone = ObjectUtils.Clone<TModel>(model);
                    var entry = db.Entry(clone);
                    entry.State = EntityState.Deleted;
                }
                var rowsAffected = db.SaveChanges();
                Debug.Assert(rowsAffected > 0);
            }
            if (AfterDelete != null)
            {
                AfterDelete.Invoke(this, new DataServiceEventArgs() { Items = models });
            }
        }


        public IEnumerable<TModel> Select<TModel>(IQuery<TModel> query) where TModel : class, IModel
        {
            using (var db = GetContext())
            {
                var source = db.Set<TModel>() as IQueryable<TModel>;
                var handlers = Handlers;
                Delegate handler;
                if (!handlers.TryGetValue(typeof(TModel), out handler))
                {
                    lock (_compileLock)
                    {
                        if (!handlers.TryGetValue(typeof(TModel), out handler))
                        {
                            handler = Compiler.Compile<TModel>(query);
                            handlers.Add(typeof(TModel), handler);
                        }
                    }
                }
                IEnumerable<TModel> result;
                
                if (query.Includes != null && query.Includes.Any())
                {
                    source = query.Includes.Aggregate(source, (current, include) => current.Include(include));
                }
                
                result = ((Func<IQueryable<TModel>, IQuery<TModel>, IQueryable<TModel>>)handler).Invoke(source,
                                                                                                         query)
                                                                                                 .AsNoTracking();
                if (!(query.Take <= 0))
                {
                    result = result.ToList();
                }
                return result;
            }
        }



        private readonly object _compileLock = new object();
        public string NameOrConnectionString { get; set; }
        public string ContextTypeString { get; set; }

        protected DbContext GetContext()
        {
            Func<string, DbContext> handler = _createContext;
            if (handler == null)
            {
                lock (_contextCreatorLocker)
                {
                    if (_createContext == null)
                    {
                        var type = Type.GetType(ContextTypeString);
                        if (type == null) throw new ArgumentNullException("ContextTypeString");

                        var constructors = type.GetConstructors();
                        var arg = Expression.Parameter(typeof(string));
                        var predicateMethod = typeof(string).GetMethod("IsNullOrWhiteSpace");
                        handler = _createContext = Expression.Lambda<Func<string, DbContext>>(
                            Expression.Condition(
                                Expression.Call(predicateMethod, arg),
                                Expression.New(constructors.First(o => o.GetParameters().Length == 0)),
                                Expression.New(constructors.First(o => o.GetParameters().Length == 1), arg)
                            ),
                            arg
                        ).Compile();
                    }
                    else
                    {
                        handler = _createContext;
                    }
                }
            }

            var db = handler(NameOrConnectionString);
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            return db;
        }


        #region IModelService Members

        IEnumerable<TModel> IModelService.Select<TModel>(IQuery<TModel> query)
        {
            return this.Select<TModel>(query);
        }

        void IModelService.Update<TModel>(params TModel[] models)
        {
            this.Update(models);
        }

        void IModelService.Delete<TModle>(params TModle[] models)
        {
            this.Delete(models);
        }

        void IModelService.Create<TModel>(params TModel[] models)
        {
            this.Create(models);
        }

        #endregion
    }
}
