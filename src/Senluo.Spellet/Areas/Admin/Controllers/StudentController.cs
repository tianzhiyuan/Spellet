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
    public class StudentController : AdminController<Senluo.Spellet.Models.Student, StudentQuery>
    {
        //
        // GET: /Admin/Student/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Modify(int? id)
        {
            Senluo.Spellet.Models.Student student = id != null
                                         ? Service.FindByID<Senluo.Spellet.Models.Student, StudentQuery>(id.Value)
                                         : new Senluo.Spellet.Models.Student();
            student.Password = "";
            
            return View(student);
        }
        [HttpPost]
        public ActionResult Modify(Senluo.Spellet.Models.Student student)
        {
            var same = Service.FirstOrDefault(new StudentQuery() {StudentID = student.StudentID});
            if (same != null && same.ID != student.ID)
            {
                throw new RuleViolatedException("已经存在相同的学号");
            }
            if (student.ID > 0)
            {
                if (string.IsNullOrWhiteSpace(student.Password))
                {
                    student.Password = null;
                }
                else
                {
                    student.Password = student.Password.Hash();
                }
                Service.Patch<Senluo.Spellet.Models.Student, StudentQuery>(student);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(student.Password))
                {
                    student.Password = "";
                }
                student.Password = student.Password.Hash();
                Service.Create(student);
            }
            return Serialize(new {success = true, item = student});
        }
    }
}
