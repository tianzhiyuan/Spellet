using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Exceptions;

namespace Senluo.UI.Mvc
{
    public class Pagination
    {
        public Pagination(int take, int skip ,int count)
        {
            if (skip <= 0) skip = 0;
            if (count <= 0) count = 0;
            if (take <= 0) throw new RuleViolatedException("take less than zero");
            this.take = take;
            this.skip = skip;
            this.count = count;
        }

        private readonly int take;
        private readonly int skip;
        private readonly int count;
        public int Take { get { return take; } }
        public int Skip { get { return skip; } }
        public int Count { get { return count; } }
        public int CurrentPage { get { return  skip/take + 1; } }
        public int TotalPage { get { return count/take + 1; } }

        public int NextPage
        {
            get
            {
                if (CurrentPage == TotalPage) return TotalPage;
                return CurrentPage + 1;
            }
        }

        public int PrevPage
        {
            get
            {
                if (CurrentPage == 1) return 1;
                return CurrentPage - 1;
            }
        }

        public int NextSkip
        {
            get { return (NextPage - 1)*take; }
        }
        public int PrevSkip
        {
            get { return (PrevPage - 1)*take; }
        }
        public int FirstSkip
        {
            get { return 0; }
        }
        public int LastSkip
        {
            get { return (TotalPage - 1)*take; }
        }
    }
}
