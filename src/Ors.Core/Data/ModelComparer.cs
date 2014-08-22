using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Data
{
    public class ModelComparer<TModel> : IEqualityComparer<TModel>
        where TModel : class, IModel
    {
        public bool Equals(TModel x, TModel y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            if (ReferenceEquals(x, y)) return true;
            return x.ID != null && x.ID == y.ID;
        }


        public int GetHashCode(TModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
