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
    public class RegisterController : StudentController
    {
        //
        // GET: /Student/Register/
        public RegisterController()
        {
            this.DoAuth = false;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Senluo.Spellet.Models.Student student)
        {
            var same = Service.FirstOrDefault(new StudentQuery() { StudentID = student.StudentID });
            if (same != null && same.ID != student.ID)
            {
                throw new RuleViolatedException("已经存在相同的学号");
            }

            if (string.IsNullOrWhiteSpace(student.Password))
            {
                throw new RuleViolatedException("密码不能为空");
            }
            student.Password = student.Password.Hash();
            Service.Create(student);
            return Serialize(new { success = true, item = student });
        }
    }
}
