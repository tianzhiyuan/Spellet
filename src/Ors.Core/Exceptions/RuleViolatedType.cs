using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Core.Exceptions
{
    /// <summary>
    /// 错误码
    /// </summary>
    public enum RuleViolatedType
    {
        UnKnown = 1,
        ArgumentError = 2,
        NotAuthenticated = 10,
        NotAuthorizaed = 20,
        NotSupported = 30,
        ArgumentNull = 40,
        Duplicated = 50,
        OutOfRange = 60,
        ObjectNotFound = 70,
    }
}
