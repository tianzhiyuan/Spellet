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
                    Password = defaultUserName.Hash(),
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
            }
        }
    }
}