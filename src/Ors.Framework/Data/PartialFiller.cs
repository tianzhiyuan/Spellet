using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using Ors.Core;
using Ors.Core.Components;
using Ors.Core.Data;

namespace Ors.Framework.Data
{
    [Component(LifeStyle.Singleton)]
    public class PartialFiller : IAssemblyInitializer
    {
        
        public static void Fill<TModel, TQuery>(TModel model)
            where TModel:class, IModel
            where TQuery:IQuery<TModel>,new()
        {
            if (model == null || model.ID == null) return;
            var query = new TQuery() {ID = model.ID};
            var svc = ObjectContainer.Resolve<IModelService>();
            var current = svc.FirstOrDefault(query);
            AutoMapper.Mapper.Map(current, model);
        }
        public static void Fill<TModel, TQuery>(TModel[] models)
            where TModel : class, IModel
            where TQuery : IQuery<TModel>, new()
        {
            if (models == null || !models.Any()) return;
            var query = new TQuery() {IDList = models.Select(o => o.ID).OfType<int>().ToArray()};
            var svc = ObjectContainer.Resolve<IModelService>();
            var origins = svc.Select(query);
            foreach (var model in models)
            {
                var origin = origins.FirstOrDefault(o => o.ID == model.ID);
                AutoMapper.Mapper.Map(origin, model);
            }
        }

        void CreateMap<TModel>()
        {
            Mapper.Configuration.AllowNullCollections = true;
            Mapper.CreateMap<TModel, TModel>().ForAllMembers(config =>
                config.Condition(context =>
                    context.DestinationValue == null
                )
            );
        }
        public void Initialize(Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes().Where(typeof (IModel).IsAssignableFrom))
                    {
                        var method =
                            new Action(this.CreateMap<AbstractModel>).Method.GetGenericMethodDefinition()
                                                                     .MakeGenericMethod(type);
                        method.Invoke(this, null);

                    }
                }
                catch
                {
                }
            }
        }
    }
}
