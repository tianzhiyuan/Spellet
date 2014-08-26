using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Data;

namespace Ors.Framework.Data
{
    public interface IModelService
    {
        IEnumerable<TModel> Select<TModel>(IQuery<TModel> query)
            where TModel : class, IModel;

        void Update<TModel>(params TModel[] models) where TModel : class, IModel, new();
        void Delete<TModle>(params TModle[] models) where TModle : class, IModel, new();
        void Create<TModel>(params TModel[] models) where TModel : class, IModel, new();

        

        event EventHandler<DataServiceEventArgs> BeforeUpdate;
        event EventHandler<DataServiceEventArgs> AfterUpdate;
        event EventHandler<DataServiceEventArgs> BeforeDelete;
        event EventHandler<DataServiceEventArgs> AfterDelete;
        event EventHandler<DataServiceEventArgs> BeforeCreate;
        event EventHandler<DataServiceEventArgs> AfterCreate;
    }
}
