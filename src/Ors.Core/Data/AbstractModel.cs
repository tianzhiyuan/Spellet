using System;

namespace Ors.Core.Data
{
    [Serializable]
    public class AbstractModel : IModel
    {
        public int? ID { get; set; }
        public int? CreatorID { get; set; }
        public int? LastModifierID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModifiedAt { get; set; }
    }
    [Serializable]
    public abstract class AbstractQuery : IQuery
    {
        public int? ID { get; set; }
        public int[] IDList { get; set; }

        public int[] CreatorIDList { get; set; }

        public int[] LastModifierIDList { get; set; }

        public Range<DateTime> CreatedAtRange { get; set; }
        public Range<DateTime> LastModifiedAtRange { get; set; }

        public string[] Includes { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }

        public int? Count { get; set; }
        public string OrderField { get; set; }
        public OrderDirection? OrderDirection { get; set; }
        public abstract Type ModelType { get; }
    }
    [Serializable]
    public class AbstractQuery<TModel> : AbstractQuery, IQuery<TModel> where TModel : IModel
    {
        public override Type ModelType
        {
            get { return typeof (TModel); }
        }
    }
}
