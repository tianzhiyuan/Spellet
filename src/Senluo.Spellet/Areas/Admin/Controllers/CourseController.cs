using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Ors.Framework.Data;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.Areas.Admin.Controllers
{
    public class CourseController : AdminController<Course, CourseQuery>
    {
        //
        // GET: /Admin/Course/

        public ActionResult Index()
        {
            var courses = Service.Select(new CourseQuery() {});
            var contents =
                Service.Select(new CourseContentQuery() {CourseIDList = courses.Select(o => o.ID).OfType<int>().ToArray()});
            foreach (var c in courses)
            {
                c.Contents = contents.Where(o => o.CourseID == c.ID).ToArray();
            }
            var entry =
                Service.Select(new EntryQuery() {IDList = contents.Select(o => o.ContentID).OfType<int>().ToArray()});
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
            return Serialize(new {success = true, items = courses, count = query.Count});
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Modify(int id)
        {
            var course = Service.FindByID<Course, CourseQuery>(id);
            var contents = Service.Select(new CourseContentQuery() {CourseIDList = new[] {id}});
            var entry =
               Service.Select(new EntryQuery() { IDList = contents.Select(o => o.ContentID).OfType<int>().ToArray() });
            foreach (var c in contents)
            {
                c.Entry = entry.FirstOrDefault(o => o.ID == c.ContentID);
            }
            course.Contents = contents.ToArray();
            return View(course);
        }
        [HttpPost]
        public ActionResult Create(Course course)
        {
            using (var ts = new TransactionScope())
            {
                Service.Create(course);
                if (course.Contents != null && course.Contents.Any())
                {
                    foreach (var c in course.Contents)
                    {
                        c.CourseID = course.ID;
                    }
                    Service.Create(course.Contents);
                }
                ts.Complete();
            }
            return Serialize(new {success = true, item = course});
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put | HttpVerbs.Delete)]
        public virtual ActionResult Content(CourseContent item)
        {
            var svc = Service;
            if (item == null) return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);

            var verb = Request.HttpMethod;
            switch (verb)
            {
                case "POST":
                    svc.Create(item);
                    return Serialize(new { success = true, item = item });
                case "PUT":
                    svc.Patch<CourseContent, CourseContentQuery>(item);
                    break;
                case "DELETE":
                    svc.Delete(item);
                    break;
                default:
                    return new HttpStatusCodeResult((int)HttpStatusCode.MethodNotAllowed);
            }

            return Serialize(new { success = true });
        }
        
    }
}
