using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ors.Core.Exceptions;
using Ors.Core.Security;
using Ors.Framework.Data;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Student.Controllers
{
    public class LoginController : StudentController
    {
        public LoginController()
        {
            this.DoAuth = false;

        }
        //
        // GET: /Student/Login/

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
            try
            {
                var student = Service.FirstOrDefault(new StudentQuery() { StudentID = username });

                if (student == null)
                {
                    throw new RuleViolatedException("用户不存在");
                }
                if (!Encryption.IsMatch(student.Password, password))
                {
                    throw new RuleViolatedException("密码错误");
                }
                if (student.Enabled != true)
                {
                    throw new RuleViolatedException("未启用");
                }
                this.LogIn(student.ID.Value);
                return Serialize(new { success = true });
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
                return null;
            }
        }
    }
}
