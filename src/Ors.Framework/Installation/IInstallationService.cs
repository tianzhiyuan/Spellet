using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Framework.Installation
{
    public interface IInstallationService
    {
        void InstallData(string defaultUserName, string defaultUserPassword, bool installSample = true);
    }
}
