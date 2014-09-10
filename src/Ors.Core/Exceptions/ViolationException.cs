using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Properties;

namespace Ors.Core.Exceptions
{
    [Serializable]
    public class RuleViolatedException : ApplicationException
    {
        public int Code { get; private set; }

        public RuleViolatedException()
            : base()
        {

        }

        public RuleViolatedException(string msg)
            : base(msg)
        {

        }

        public RuleViolatedException(string msg, Exception innerEx)
            : base(msg, innerEx)
        {

        }

        public RuleViolatedException(int errCode)
        {
            Code = errCode;
        }
        
        public RuleViolatedException(string message, int errCode)
            : base(message)
        {
            Code = errCode;
        }

        public RuleViolatedException(int errCode,string msg, Exception inner) : base(msg, inner)
        {
            Code = errCode;
        }
        public RuleViolatedException(string message, RuleViolatedType code):base(message)
        {
            Code = (int) code;
        }

        public static RuleViolatedException NotSupported(string name = null)
        {
            var msg = string.Format(Resources.RuleViolated_NotSupported, name ?? "");
            var ex = new RuleViolatedException(msg, (int) RuleViolatedType.NotSupported);

            return ex;
        }

        public static RuleViolatedException NotAuthorized(string userName = null)
        {
            var msg = string.Format(Resources.RuleViolated_NotAuthorized, userName ?? "");
            var ex = new RuleViolatedException(msg, (int) RuleViolatedType.NotAuthorizaed);
            return ex;
        }

        public static RuleViolatedException NotAuthenticated()
        {
            var ex = new RuleViolatedException(Resources.RuleViolated_NotAuthenticated,
                                               (int) RuleViolatedType.NotAuthenticated);
            return ex;
        }

        public static RuleViolatedException ArgumentNull(string name = null)
        {
            var msg = string.Format(Resources.RuleViolated_ArgumentNull, name ?? "");
            var ex = new RuleViolatedException(msg, (int) RuleViolatedType.ArgumentNull);
            return ex;
        }

        public static RuleViolatedException Duplicated(string name = null)
        {
            var msg = string.Format(Resources.RuleViolated_Duplicated, name ?? "");
            var ex = new RuleViolatedException(msg, (int) RuleViolatedType.Duplicated);
            return ex;
        }

        public static RuleViolatedException ObjectNotFound(string name = null)
        {
            var msg = string.Format(Resources.RuleViolated_ObjectNotFound, name ?? "");
            var ex = new RuleViolatedException(msg, (int) RuleViolatedType.ObjectNotFound);
            return ex;
        }
    }
}
