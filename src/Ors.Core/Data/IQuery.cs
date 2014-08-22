using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core;

namespace Ors.Core.Data
{
    public interface IQuery
    {
        int? ID { get; set; }
        int[] IDList { get; set; }
        int[] CreatorIDList { get; set; }
        int[] LastModifierIDList { get; set; }
        Range<DateTime> CreatedAtRange { get; set; }
        Range<DateTime> LastModifiedAtRange { get; set; }
        string[] Includes { get; set; }
        int? Skip { get; set; }
        int? Take { get; set; }
        int? Count { get; set; }
        string OrderField { get; set; }
        OrderDirection? OrderDirection { get; set; }
        Type ModelType { get; }
    }
    public interface IQuery<TModel> : IQuery where TModel:IModel
    {
        
    }
}
