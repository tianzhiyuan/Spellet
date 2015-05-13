using System.Transactions;
using Ors.Core.Data;
using Ors.Core.Exceptions;
using Ors.Framework.Data;
using Senluo.Spellet.Areas.Student.Models;
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
                asheet.Exam = exam;
                return View("Joined", asheet);
            }

            return View(exam);
        }

        public ActionResult Success()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Answer(String data)
        {
            var response = new JsonResult()
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
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
                    ExamIDList = new[] {examId}
                });
            var examples = Service.Select(new ExampleQuery()
                {
                    IDList = questions.Select(o => o.ContentID).OfType<int>().ToArray(),
                    Includes = new string[]{"Entry"}
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
            using (var ts = new TransactionScope())
            {
                AnswerSheet aSheet = new AnswerSheet();
                aSheet.CreatedAt = DateTime.Now;
                aSheet.CreatorID = UserID;
                aSheet.ExamID = examId;
                aSheet.StudentID = UserID;
                aSheet.LastModifiedAt = DateTime.Now;
                aSheet.LastModifierID = UserID;
                Service.Create(aSheet);
                var totalScore = 0;
                foreach (String item in items.Take(items.Count()-1))
                {
                    Answer model = new Answer();
                    model.CreatedAt = DateTime.Now;
                    model.CreatorID = UserID;
                    int eid = Convert.ToInt32(item.Split('_')[0].ToString());
                    Question question = questions.FirstOrDefault(c => c.ContentID == eid);
                    var example = examples.FirstOrDefault(o => o.ID == question.ContentID);
                    model.Fill = item.Split('_')[1].ToString();
                    model.LastModifiedAt = DateTime.Now;
                    model.LastModifierID = UserID;
                    model.SheetID = aSheet.ID;
                    model.QuestionID = question == null ? -1 : question.ID;
                    if (model.Fill.Equals(example.Keyword ?? example.Entry.Word, StringComparison.OrdinalIgnoreCase))
                    {
                        model.Score = 1;
                    }
                    else
                    {
                        model.Score = 0;
                    }
                    totalScore += model.Score.Value;
                    Service.Create(model);
                }
                aSheet.TotalScore = totalScore;
                Service.Update(aSheet);
                ts.Complete();
            }
            

            response.Data = new
                {
                    success = true,
                    msg = "OK"
                };
            return response;
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
                        var random = new Random();
                        for (int i = 0; i < total; i++)
                        {
                            selectId = contentIds[random.Next(seed)];
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
            //修改为按照entryIds排序
            foreach (var entryId in entryIds)
            {
                var entry = entries.FirstOrDefault(o => o.ID == entryId);
                if (entry == null) continue;
                ExamContent model = new ExamContent();
                model.id = entry.ID.Value;
                Example example = examples.FirstOrDefault(o => o.EntryID == entry.ID);
                if (example == null)
                {
                    continue;
                }
                model.word = example.Keyword ?? entry.Word;
                String sentence = example.Origin.Replace(model.word, "__PLACEHOLDER__");
                if (sentence.IndexOf("__PLACEHOLDER__", StringComparison.InvariantCulture) >= 0)
                {
                    String[] oList = sentence.Split(new string[] { "__PLACEHOLDER__" }, StringSplitOptions.None);
                    model.first = oList[0];
                    model.second = oList[1];
                }
                models.Add(model);
            }
            return models;
        }


        public ActionResult Score(int examid)
        {
            return View(BuildAnswerSheetVM(examid, UserID));
        }

        public AnswerSheetVM BuildAnswerSheetVM(int examid, int studentid)
        {
            var exam = Service.FirstOrDefault(new ExamQuery() { ID = examid });
            if (exam == null || !(exam.ID > 0))
            {
                throw new RuleViolatedException("exam为空");
            }
            var questions = Service.Select(new QuestionQuery()
            {
                ExamIDList = new[] { examid }
            });
            var examples =
                Service.Select(new ExampleQuery() { IDList = questions.Select(o => o.ContentID).OfType<int>().ToArray() });
            var answersheet = Service.FirstOrDefault(new AnswerSheetQuery() { ExamID = examid, StudentID = studentid});
            if (answersheet == null || !(answersheet.ID > 0))
            {
                throw new RuleViolatedException("答卷为空");
            }
            var answers = Service.Select(new AnswerQuery() { SheetIDList = new int[] { answersheet.ID.Value } });
            var viewModel = new AnswerSheetVM();
            viewModel.ID = answersheet.ID.Value;
            viewModel.Count = answers.Count();
            viewModel.ExamID = examid;
            viewModel.TotalScore = (answersheet.TotalScore.Value) * 100 / answers.Count();
            var answerVMs = new List<AnswerVM>();
            foreach (var question in questions)
            {
                var vm = new AnswerVM();
                var answer = answers.FirstOrDefault(o => o.QuestionID == question.ID);
                var example = examples.FirstOrDefault(o => o.ID == question.ContentID);
                vm.OriginSentense = example.Origin;
                vm.Translation = example.Trans;
                vm.Expect = example.Keyword;
                if (answer == null)
                {
                    vm.Filling = "";
                    vm.IsRight = false;
                }
                else
                {
                    vm.Filling = answer.Fill;
                    vm.IsRight = answer.Score > 0;
                }
                answerVMs.Add(vm);
            }
            viewModel.Answers = answerVMs;
            return viewModel;
        }
    }
}
