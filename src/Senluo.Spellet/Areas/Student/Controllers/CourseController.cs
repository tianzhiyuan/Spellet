using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using Ors.Core.Data;
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

        public ActionResult Today()
        {
            var now = DateTime.Now;
            var course =
                Service.FirstOrDefault(new CourseQuery()
                    {
                        StartTimeRange = new Range<DateTime>() { Left = new DateTime(now.Year, now.Month, now.Day) }
                    });
            if (course != null)
            {
                fillCourse(course);
                ViewBag.Title = "今日课程";
                return View("Detail", course);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult History()
        {
            return View();
        }

        public ActionResult Detail(int courseId)
        {
            var course =
                Service.FirstOrDefault(new CourseQuery()
                {
                    ID = courseId
                });

            if (course != null)
            {
                fillCourse(course);
                ViewBag.Title = "查看课程";
                return View("Detail", course);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Words(int id = 7)
        {
            IEnumerable<CourseContent> Contents =
                    Service.Select(new CourseContentQuery() { CourseIDList = new int[] { id } }).ToArray();

            if (Contents == null || !Contents.Any())
                return View();

            IEnumerable<Entry> entries =
                Service.Select(new EntryQuery()
                {
                    IDList = Contents.Select(o => o.ContentID).OfType<int>().ToArray()
                });
            foreach (var content in Contents)
            {
                content.Entry = entries.FirstOrDefault(o => o.ID == content.ContentID);
            }
            var trans =
                Service.Select(new TranslationQuery() { EntryIDList = entries.Select(o => o.ID).OfType<int>().ToArray() });
            var examples =
                Service.Select(new ExampleQuery() { EntryIDList = entries.Select(o => o.ID).OfType<int>().ToArray() });
            foreach (var entry in entries)
            {
                entry.Translations = trans.Where(o => o.EntryID == entry.ID).ToArray();
                entry.Examples = examples.Where(o => o.EntryID == entry.ID).ToArray();
            }

            return View(entries);
        }

        private Course fillCourse(Course course)
        {
            course.Contents =
                    Service.Select(new CourseContentQuery() { CourseIDList = new int[] { course.ID.Value } }).ToArray();
            var entries =
                Service.Select(new EntryQuery()
                {
                    IDList = course.Contents.Select(o => o.ContentID).OfType<int>().ToArray()
                });
            foreach (var content in course.Contents)
            {
                content.Entry = entries.FirstOrDefault(o => o.ID == content.ContentID);
            }
            var trans =
                Service.Select(new TranslationQuery() { EntryIDList = entries.Select(o => o.ID).OfType<int>().ToArray() });
            var examples =
                Service.Select(new ExampleQuery() { EntryIDList = entries.Select(o => o.ID).OfType<int>().ToArray() });
            foreach (var entry in entries)
            {
                entry.Translations = trans.Where(o => o.EntryID == entry.ID).ToArray();
                entry.Examples = examples.Where(o => o.EntryID == entry.ID).ToArray();
            }
            return course;
        }
    }
}
