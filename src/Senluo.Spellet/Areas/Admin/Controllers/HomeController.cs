using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ors.Core.Exceptions;
using Ors.Core.Security;
using Senluo.Spellet.Models;
using Senluo.UI.Mvc;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        //
        // GET: /Admin/Home/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Student");
        }
        [HttpPost]
        public ActionResult ChangePassword()
        {
            var old = Request["OldPwd"];
            var newPwd = Request["NewPwd"];
            if (string.IsNullOrWhiteSpace(old))
            {
                throw new RuleViolatedException("原密码不能为空");
            }
            if (string.IsNullOrWhiteSpace(newPwd))
            {
                throw new RuleViolatedException("密码不能为空");
            }
            var user = this.Teacher;
            if (!Encryption.IsMatch(user.Password, old))
            {
                throw new RuleViolatedException("密码不匹配");
            }
            user.Password = newPwd.Hash();
            Service.Update(user);
            return Serialize(new {success = true});
        }
    }
}
