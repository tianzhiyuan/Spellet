using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Senluo.Spellet.EF
{
    public class DataSettingsHelper
    {
        public static bool DatabaseIsInstalled()
        {
            var dataFile = HostingEnvironment.MapPath("~/App_Data/") + "data.dat";
            if (File.Exists(dataFile))
            {
                return true;
            }
            return false;
        }
        public static void SetInstalled()
        {
            var dataFile = HostingEnvironment.MapPath("~/App_Data/") + "data.dat";
            if (!File.Exists(dataFile))
            {
                File.Create(dataFile);
            }
        }
    }
}