using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ors.Core.Components;
using Ors.Core.Security;
using Ors.Framework.Data;
using Ors.Framework.Installation;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.EF
{
    [Component(LifeStyle.Singleton)]
    public class CodeFirstInstallService : IInstallationService
    {
        private readonly IModelService _modelService;
        public CodeFirstInstallService(IModelService modelService)
        {
            _modelService = modelService;
        }
        public void InstallData(string defaultUserName, string defaultUserPassword, bool installSample = true)
        {
            _modelService.Create(new Teacher()
                {
                    Account = defaultUserName,
                    Password = defaultUserPassword.Hash(),
                    Name = "Admin"
                });
            
            if (installSample)
            {
                _modelService.Create(new Student()
                    {
                        StudentID = "1001",
                        Password = "111111".Hash(),
                        Enabled = true,
                        Name = "测试用户",
                        EnglishName = "test user",

                    });

                var entry = new Entry()
                    {
                        Word = "abandon",
                        Phonetic_UK = "[ə'bænd(ə)n]",
                        Phonetic_US = "[ə'bændən]",
                    };
                _modelService.Create(entry);
                _modelService.Create(new Example()
                    {
                        EntryID = entry.ID,
                        Keyword = entry.Word,
                        Origin = "Don't abandon yourself to despair.",
                        Trans = "不要自暴自弃。",
                    });
                _modelService.Create(new Translation()
                    {
                        EntryID = entry.ID,
                        Description = "vt. 遗弃；放弃",
                    });

                var entry1 = new Entry()
                    {
                        Word = "bachelor",
                        Phonetic_UK = "['bætʃələ]",
                        Phonetic_US = "['bætʃəlɚ]",
                    };
                _modelService.Create(entry1);
                _modelService.Create(new Example()
                    {
                        EntryID = entry1.ID,
                        Keyword = entry1.Word,
                        Origin = "Distrusting women, he remained a bachelor all his life.",
                        Trans = "由于不信任女人， 他做了一辈子单身汉。",
                    });
                _modelService.Create(new Translation()
                    {
                        EntryID = entry1.ID,
                        Description = "n. 学士；单身汉；（尚未交配的）小雄兽",
                    });

                var entry2 = new Entry()
                    {
                        Word = "cabbage",
                        Phonetic_UK = "[ˈkæbɪdʒ]",
                        Phonetic_US = "[ˈkæbɪdʒ]",
                    };
                _modelService.Create(entry2);
                _modelService.Create(new Example()
                    {
                        EntryID = entry2.ID,
                        Keyword = entry2.Word,
                        Origin = "When the water boils add the meat and the cabbage.",
                        Trans = "水开时加入肉和洋白菜。",
                    });
                _modelService.Create(new Translation()
                {
                    EntryID = entry2.ID,
                    Description = "n. 卷心菜，甘蓝菜，洋白菜；（俚)脑袋；（非正式、侮辱）植物人（常用于英式英语）；（俚）钱，尤指纸币（常用于美式俚语）",
                });

                var entry3 = new Entry()
                {
                    Word = "dangerous",
                    Phonetic_UK = "['deɪn(d)ʒ(ə)rəs]",
                    Phonetic_US = "['dendʒərəs]",
                };
                _modelService.Create(entry3);
                _modelService.Create(new Example()
                {
                    EntryID = entry3.ID,
                    Keyword = entry3.Word,
                    Origin = "I kept my friend back from the dangerous animal.",
                    Trans = "我不让我的朋友靠近那个危险的动物。",
                });
                _modelService.Create(new Translation()
                {
                    EntryID = entry3.ID,
                    Description = "adj. 危险的",
                });

                var entry4 = new Entry()
                {
                    Word = "eclipse",
                    Phonetic_UK = "[ɪ'klɪps]",
                    Phonetic_US = "[ɪ'klɪps]",
                };
                _modelService.Create(entry4);
                _modelService.Create(new Example()
                {
                    EntryID = entry4.ID,
                    Keyword = entry4.Word,
                    Origin = "An eclipse is an interesting phenomenon.",
                    Trans = "日[月]蚀是一个有趣的现象。",
                });
                _modelService.Create(new Translation()
                {
                    EntryID = entry4.ID,
                    Description = "vt. 使黯然失色；形成蚀",
                });

                var entry5 = new Entry()
                {
                    Word = "facility",
                    Phonetic_UK = "[fə'sɪlɪtɪ]",
                    Phonetic_US = "[fə'sɪləti]",
                };
                _modelService.Create(entry5);
                _modelService.Create(new Example()
                {
                    EntryID = entry5.ID,
                    Keyword = entry5.Word,
                    Origin = "It describes completely the facility and its safety basis.",
                    Trans = "它完整的描述了设备,和它的安全基础。",
                });
                _modelService.Create(new Translation()
                {
                    EntryID = entry5.ID,
                    Description = "n. 设施；设备；容易；灵巧",
                });

                var entry6 = new Entry()
                {
                    Word = "gangster",
                    Phonetic_UK = "['gæŋstə]",
                    Phonetic_US = "['ɡæŋstɚ]",
                };
                _modelService.Create(entry6);
                _modelService.Create(new Example()
                {
                    EntryID = entry6.ID,
                    Keyword = entry6.Word,
                    Origin = "The police crept up from behind and took the gangster by surprise.",
                    Trans = "警察悄悄从背后绕过去，出其不意地将歹徒逮捕。",
                });
                _modelService.Create(new Translation()
                {
                    EntryID = entry6.ID,
                    Description = "n. 歹徒，流氓；恶棍",
                });

                var course = new Course()
                    {
                        Name = "预习",
                        StartTime = DateTime.Now,
                        EndTime = DateTime.Now.AddYears(1),

                    };
                _modelService.Create(course);
                _modelService.Create(new CourseContent[]
                {
                    new CourseContent()
                        {
                            ContentID = entry.ID,
                            CourseID = course.ID
                        }, 
                    new CourseContent()
                        {
                            ContentID = entry1.ID,
                            CourseID = course.ID
                        }, 
                    new CourseContent()
                        {
                            ContentID = entry2.ID,
                            CourseID = course.ID
                        }, 
                    new CourseContent()
                        {
                            ContentID = entry3.ID,
                            CourseID = course.ID
                        }, 
                    new CourseContent()
                        {
                            ContentID = entry4.ID,
                            CourseID = course.ID
                        }, 
                    new CourseContent()
                        {
                            ContentID = entry5.ID,
                            CourseID = course.ID
                        }, 
                    new CourseContent()
                        {
                            ContentID = entry6.ID,
                            CourseID = course.ID
                        }, 
                });

                
            }
        }
    }
}