using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Ors.Framework.Data;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Student.Controllers
{
    public class CourseController : StudentController<Course, CourseQuery>
    {
        //
        // GET: /Student/Course/

        public ActionResult Index()
        {
            var courses = Service.Select(new CourseQuery() { });
            var contents =
                Service.Select(new CourseContentQuery() { CourseIDList = courses.Select(o => o.ID).OfType<int>().ToArray() });
            foreach (var c in courses)
            {
                c.Contents = contents.Where(o => o.CourseID == c.ID).ToArray();
            }
            var entry =
                Service.Select(new EntryQuery() { IDList = contents.Select(o => o.ContentID).OfType<int>().ToArray() });
            foreach (var c in contents)
            {
                c.Entry = entry.FirstOrDefault(o => o.ID == c.ContentID);
            }
            return View(courses);
        }
        public override ActionResult List(CourseQuery query)
        {
            var courses = Service.Select(query);
            var contents =
                Service.Select(new CourseContentQuery() { CourseIDList = courses.Select(o => o.ID).OfType<int>().ToArray() });
            foreach (var c in courses)
            {
                c.Contents = contents.Where(o => o.CourseID == c.ID).ToArray();
            }
            var entry =
                Service.Select(new EntryQuery() { IDList = contents.Select(o => o.ContentID).OfType<int>().ToArray() });
            foreach (var c in contents)
            {
                c.Entry = entry.FirstOrDefault(o => o.ID == c.ContentID);
            }
            return Serialize(new { success = true, items = courses, count = query.Count });
        }
        
    }
}
