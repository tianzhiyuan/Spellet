using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ors.Core.Exceptions;
using Ors.Core.Security;
using Ors.Framework.Data;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class LoginController : AdminController
    {
        public LoginController()
        {
            this.DoAuth = false;
            
        }
        //
        // GET: /Admin/Login/

        public ActionResult Index(string url)
        {
            ViewBag.Url = HttpUtility.UrlDecode(url);
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            var username = form["username"];
            var password = form["password"];
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new RuleViolatedException("用户名和密码不能为空");
            }
            var teacher = Service.FirstOrDefault(new TeacherQuery() {Account = username});
            if (teacher == null)
            {
                throw new RuleViolatedException("用户不存在");
            }
            if (!Encryption.IsMatch(teacher.Password, password))
            {
                throw new RuleViolatedException("密码错误");
            }
            this.LogIn(teacher.ID.Value);
            return Serialize(new {success = true});
        }
    }
}
