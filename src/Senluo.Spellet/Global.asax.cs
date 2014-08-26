using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Ors.Core.Autofac;
using Ors.Core.Configurations;
using Ors.Core.Log4Net;
using Ors.Framework.Data;
using Senluo.VocaSpider.Parser;

namespace Senluo.Spellet
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Configuration.Instance
                         .UseAutofac()
                         .RegisterCommon()
                         .UseDataService("Senluo.Spellet.EF.EFDbContext, Senluo.Spellet", "Voca")
                         .UseYoudaoParser()
                         .UseLog4Net()
                         .InitializeAssemblies(assemblies);
        }

    }
}