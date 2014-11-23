using Ors.Core.Data;
using Senluo.Spellet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Senluo.Spellet.Areas.Student.Controllers
{
    public class TestController : StudentController
    {
        //
        // GET: /Student/Test/

        public ActionResult Empty()
        {
            return View();
        }

        public ActionResult Joined()
        {
            return View();
        }
        public ActionResult Self()
        {
            return View();
        }

        public ActionResult Index()
        {
            var exam = Service.Select(new ExamQuery()
            {
                Enabled = true,
                OrderField = "ID",
                OrderDirection = OrderDirection.DESC
            }).FirstOrDefault();
            if (exam == null)
            {
                return RedirectToAction("empty");
            }
            var asheet = Service.Select(new AnswerSheetQuery()
            {
                ExamID = exam.ID,
                StudentID = UserID
            }).FirstOrDefault();
            if (asheet != null)
            {
                return RedirectToAction("joined");
            }

            return View(exam);
        }

        public ActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public JsonResult answer(String data)
        {
            var response = new JsonResult()
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
            try
            {
                String[] items = data.Split(';');
                int examId = Convert.ToInt32(items[items.Length - 1].ToString());
                var aSheets = Service.Select(new AnswerSheetQuery()
                {
                    ExamID = examId,
                    StudentID = UserID
                }).FirstOrDefault();
                if (aSheets != null)
                {
                    response.Data = new
                    {
                        success = false,
                        msg = "Can't join the same exam for two times"
                    };
                    return response;
                }
                var questions = Service.Select(new QuestionQuery()
                {
                    ExamIDList = new[] { examId }
                });
                if (questions == null || !questions.Any())
                {
                    response.Data = new
                    {
                        success = false,
                        msg = "System Data Error"
                    };
                    return response;
                }
                AnswerSheet aSheet = new AnswerSheet();
                aSheet.CreatedAt = DateTime.Now;
                aSheet.CreatorID = UserID;
                aSheet.ExamID = examId;
                aSheet.StudentID = UserID;
                aSheet.LastModifiedAt = DateTime.Now;
                aSheet.LastModifierID = UserID;
                Service.Create(aSheet);

                foreach (String item in items)
                {
                    Answer model = new Answer();
                    model.CreatedAt = DateTime.Now;
                    model.CreatorID = UserID;
                    int eid = Convert.ToInt32(item.Split('_')[0].ToString());
                    Question queston = questions.Where(c => c.ContentID == eid).FirstOrDefault();
                    model.Fill = item.Split('_')[1].ToString();
                    model.LastModifiedAt = DateTime.Now;
                    model.LastModifierID = UserID;
                    model.SheetID = aSheet.ID;
                    model.QuestionID = queston == null ? -1 : queston.ID;
                    Service.Create(model);
                }

                response.Data = new
                {
                    success = true,
                    msg = "OK"
                };
                return response;
            }
            catch (Exception ex)
            {
                response.Data = new
                {
                    success = false,
                    msg = ex.ToString()
                };
                return response;
            }
        }

        /// <summary>
        /// 获取试题
        /// </summary>
        /// <param name="type">试题类型 1.历史自测  2.课程自测  3.试题测试</param>
        /// <param name="tid">试题编号 type为1时，试题数量 type为2时，课程编号  type为3时</param>
        /// <returns></returns>
        public ActionResult Exam(int type, int tid)
        {
            ViewBag.type = type;
            ViewBag.qid = tid;
            //历史自测
            if (type == 1)
            {
                var now = DateTime.Now;
                var courses =
                    Service.Select(new CourseQuery()
                    {
                        StartTimeRange = new Range<DateTime>() { Right = new DateTime(now.Year, now.Month, now.Day) }
                    });
                if (courses != null && courses.Any())
                {
                    int[] courseIds = courses.Select(c => c.ID).OfType<int>().ToArray();
                    var courseContents = Service.Select(new CourseContentQuery()
                    {
                        CourseIDList = courseIds
                    });
                    if (courseContents != null && courseContents.Any())
                    {
                        //历史课程中不重复单词列表
                        List<int> contentIds = courseContents.Select(o => o.ContentID).OfType<int>().Distinct().ToList();
                        int wordCount = contentIds.Count;
                        int total = Math.Min(tid, wordCount);
                        List<int> oids = new List<int>();//随机出的单词列表
                        int seed = wordCount;
                        int selectId = -1;
                        for (int i = 0; i < total; i++)
                        {
                            selectId = contentIds[new Random().Next(seed)];
                            oids.Add(selectId);
                            seed--;
                            contentIds.Remove(selectId);
                        }
                        List<ExamContent> models = buildModel(oids.ToArray());
                        return View(models);
                    }
                }

                return View();
            }
            else if (type == 2)
            {
                var course =
                    Service.Select(new CourseQuery()
                    {
                        ID = tid
                    }).FirstOrDefault();
                if (course == null)
                    return View();

                var courseContents = Service.Select(new CourseContentQuery()
                {
                    CourseIDList = new[] { course.ID.Value }
                });
                if (courseContents == null || !courseContents.Any())
                    return View();
                int[] entryIds = courseContents.Select(c => c.ContentID).OfType<int>().ToArray();
                List<ExamContent> models = buildModel(entryIds);
                return View(models);
            }
            else
            {
                var questions = Service.Select(new QuestionQuery()
                {
                    ExamIDList = new[] { tid }
                });
                if (questions == null || !questions.Any())
                    return View();

                int[] entryIds = questions.Select(c => c.ContentID.Value).OfType<int>().ToArray();
                List<ExamContent> models = buildModel(entryIds);
                return View(models);
            }
        }

        private List<ExamContent> buildModel(int[] entryIds)
        {
            var entries = Service.Select(new EntryQuery()
            {
                IDList = entryIds
            });
            var examples = Service.Select(new ExampleQuery() { EntryIDList = entryIds });
            List<ExamContent> models = new List<ExamContent>();
            foreach (var entry in entries)
            {
                ExamContent model = new ExamContent();
                model.id = entry.ID.Value;
                model.word = entry.Word;
                Example example = examples.Where(o => o.EntryID == entry.ID).FirstOrDefault();
                if (example == null)
                {
                    break;
                }
                String sentence = example.Origin.ToLower().Replace(entry.Word.ToLower(), "#");
                if (sentence.IndexOf("#") >= 0)
                {
                    String[] oList = sentence.Split('#');
                    model.first = oList[0];
                    model.second = oList[1];
                }
                models.Add(model);
            }

            return models;
        }
    }
}
