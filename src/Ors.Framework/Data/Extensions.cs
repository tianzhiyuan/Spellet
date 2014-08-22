using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Data;
using Ors.Core.Exceptions;

namespace Ors.Framework.Data
{
    public static class Extensions
    {
        public static TModel Single<TModel>(this IModelService svc, IQuery<TModel> query)
            where TModel : class, IModel
        {
            var models = svc.Select(query);
            if (models == null || !models.Any()) return null;
            if (models.Count() == 1) return models.FirstOrDefault();
            throw new RuleViolatedException("Sequence contains more than one element.");
        }

        public static TModel FirstOrDefault<TModel>(this IModelService svc, IQuery<TModel> query)
            where TModel : class, IModel
        {
            query.Take = 1;
            var models = svc.Select(query);
            return models.FirstOrDefault();
        }

        public static TModel FindByID<TModel, TQuery>(this IModelService svc, int ID)
            where TModel : class, IModel
            where TQuery : class, IQuery<TModel>, new()
        {
            var query = new TQuery() {ID = ID};
            var models = svc.Select(query);
            return models.FirstOrDefault();
        }

        public static int GetCount<TModel>(this IModelService svc, IQuery<TModel> query) where TModel : class, IModel
        {
            query.Take = 0;
            query.Skip = 0;
            svc.Select(query);
            return query.Count ?? 0;
        }

        public static bool Any<TModel>(this IModelService svc, IQuery<TModel> query) where TModel : class, IModel
        {
            return svc.GetCount(query) > 0;
        }
    }
}
