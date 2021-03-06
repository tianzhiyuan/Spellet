﻿using System;
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
                        StartTimeRange = new Range<DateTime>()
                        {
                            Right = new DateTime(now.Year, now.Month, now.Day),
                            Left = DateTime.Now.AddMonths(-1)
                        }
                    });

            if (course != null)
            {
                return RedirectToAction("words", "course", new { id = course.ID });
            }

            return RedirectToAction("empty", "course");
        }

        public ActionResult History(int id = 1)
        {
            ViewBag.pre = id - 1;
            ViewBag.next = 0;

            var query = new CourseQuery()
            {
                OrderDirection = OrderDirection.DESC,
                OrderField = "ID",
                Skip = (id - 1) * 8,
                Take = 9
            };
            var courses = Service.Select(query);

            if (courses.Count() == 9)
            {
                ViewBag.next = id + 1;
                return View(courses.Take(8));
            }
            return View(courses);
        }

        public ActionResult HistoryBody(DateTime? start, DateTime? end, int index)
        {
            try
            {
                ViewBag.pre = index - 1;
                ViewBag.next = 0;

                var query = new CourseQuery();
                query.OrderDirection = OrderDirection.DESC;
                query.OrderField = "ID";
                query.Skip = (index - 1) * 8;
                query.Take = 9;
                if (start != null && end != null)
                {
                    if (start <= end)
                    {
                        query.StartTimeRange = new Range<DateTime>();
                        query.StartTimeRange.Left = start;
                        query.StartTimeRange.Right = end;
                    }
                    else
                    {
                        query.StartTimeRange = new Range<DateTime>();
                        query.StartTimeRange.Left = end;
                        query.StartTimeRange.Right = start;
                    }
                }
                else if (start != null)
                {
                    query.StartTimeRange = new Range<DateTime>();
                    query.StartTimeRange.Left = start;
                }
                else if (end != null)
                {
                    query.StartTimeRange = new Range<DateTime>();
                    query.StartTimeRange.Right = end;
                }
                var courses = Service.Select(query);

                if (courses.Count() == 9)
                {
                    ViewBag.next = index + 1;
                    return View(courses.Take(8));
                }
                return View(courses);
            }
            catch (Exception e)
            {
                String msg = e.Message;
                int i = 0;
            }
            return View();
        }

        public ActionResult Empty()
        {
            return View();
        }

        public ActionResult Words(int id = 1)
        {
            IEnumerable<CourseContent> Contents =
                    Service.Select(new CourseContentQuery() { CourseIDList = new int[] { id } }).ToArray();

            if (Contents == null || !Contents.Any())
                return View(new Entry[0]);

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
