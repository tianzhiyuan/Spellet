using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using MySql.Data.Entity;
using MySql.Data.MySqlClient;
using Ors.Core.Components;
using Ors.Framework.Data;
using Ors.Framework.Installation;
using Senluo.Spellet.EF;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Controllers
{
    public class InstallController : Controller
    {
        //
        // GET: /Install/

        public ActionResult Index()
        {
            var installModel = new InstallViewModel();
            installModel.DataBaseName = "Voca";
            installModel.Host = "localhost";
            installModel.MySqlUserName = "root";
            installModel.MySqlPassword = "sa";
            installModel.AdminAccount = "admin";
            installModel.AdminPassword = "123456";
            return View(installModel);
        }
        [HttpPost]
        public ActionResult Index(InstallViewModel model)
        {
            if (DataSettingsHelper.DatabaseIsInstalled())
            {
                return Json(new {success = false, msg = "已经执行过安装，无需重复执行"}, JsonRequestBehavior.AllowGet);
            }
            var setting = ConfigurationManager.ConnectionStrings["Voca"];
            var connectionString = setting.ConnectionString;
            if (!MySqlExists(connectionString))
            {
                CreateDatabase(connectionString);
            }
            
            var initializer = new CreateTableIfNotExists<EFDbContext>("Voca");
            #pragma warning disable 0618
            Database.DefaultConnectionFactory = new MySqlConnectionFactory();
            Database.SetInitializer(initializer);
            var installService = ObjectContainer.Resolve<IInstallationService>();
            installService.InstallData(model.AdminAccount, model.AdminPassword, model.InstallSampleData);
            
            DataSettingsHelper.SetInstalled();
            return Json(new { success = true, connectionString }, JsonRequestBehavior.AllowGet);
        }
        [NonAction]
        private bool MySqlExists(string connectionString)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        protected void CreateDatabase(string connectionString)
        {

            var builder = new MySqlConnectionStringBuilder(connectionString);
            var dataBaseName = builder.Database;
            var builder2 = new MySqlConnectionStringBuilder();
            builder2.UserID = builder.UserID;
            builder2.Password = builder.Password;
            builder2.Server = builder.Server;
            string sql = string.Format("CREATE DATABASE `{0}` default charset utf8 COLLATE utf8_general_ci;",
                                       dataBaseName);
            using (var conn = new MySqlConnection(builder2.GetConnectionString(true)))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
