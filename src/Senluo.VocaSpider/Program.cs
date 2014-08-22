using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ors.Core.Autofac;
using Ors.Core.Components;
using Ors.Core.Configurations;
using Ors.Framework.Data;
using Senluo.VocaSpider.Model;
using Senluo.VocaSpider.Parser;
using Ors.Core.Extensions;

namespace Senluo.VocaSpider
{
    class Program
    {
        static void Main(string[] args)
        {
            //Configuration.Instance
            //             .UseAutofac()
            //             .UseDataService("Senluo.VocaSpider.EFDbContext, Senluo.VocaSpider", "Voca")
            //             .UseYoudaoParser();

            //var svc = ObjectContainer.Resolve<IModelService>();
            //var parser = ObjectContainer.Resolve<IParser>();
            //var rnd = new Random();
            //var a = svc.Select(new EntryQuery() {Word = "abandon",Take = 1}).FirstOrDefault();
            //var flag = svc.Any(new EntryQuery() { Word = "abandon" });
            //TestEveryExampleContainsExactWord();
            //Console.ReadKey();
            //return;
            //foreach (var word in ReadWordFromFile(@"E:\Projects\orz\src\Senluo.VocaSpider\词典.txt"))
            //{
            //    try
            //    {
            //        var entry = parser.Search(word);
            //        WriteToDb(entry);
            //    }
            //    catch{}
            //    var sleep = rnd.Next(10);
            //    if (sleep < 7)
            //    {
            //        sleep = rnd.Next(3*1000);
            //    }
            //    else
            //    {
            //        sleep = rnd.Next(10*1000);
            //    }
            //    System.Threading.Thread.Sleep(sleep);
            //}
            
            //var entry = parser.Search("abandon");
        }

        //static IEnumerable<string> ReadWordFromFile(string path)
        //{
        //    var file = File.ReadLines(path);
        //    foreach (var line in file)
        //    {
        //        if (line.Split(' ').Any())
        //        {
        //            yield return line.Split(' ')[0];
        //        }
        //    }
            
        //}

        //static void WriteToDb(Entry entry)
        //{
        //    try
        //    {
        //        var svc = ObjectContainer.Resolve<IModelService>();
        //        if (svc.Any(new EntryQuery() { Word = entry.Word }))
        //        {
        //            //update
        //        }
        //        else
        //        {
        //            //insert
        //            svc.Create(entry);
        //            if (!entry.Examples.IsNullOrEmpty())
        //            {
        //                entry.Translations.ForEach((ele) =>
        //                    {
        //                        ele.EntryID = entry.ID;
        //                    });
        //                svc.Create(entry.Translations.ToArray());
        //            }
        //            if (!entry.Examples.IsNullOrEmpty())
        //            {
        //                entry.Examples.ForEach((ele) =>
        //                    {
        //                        ele.EntryID = entry.ID;
        //                    });
        //                svc.Create(entry.Examples.ToArray());
        //            }
        //        }
        //    }
        //    catch{}
        //}

        //static void TestEveryExampleContainsExactWord()
        //{
        //    var query = new EntryQuery() {Take = 100, Skip = 0};
        //    int takeCount = 100;
        //    int times = 0;
        //    var svc = ObjectContainer.Resolve<IModelService>();
        //    while (true)
        //    {
        //        query.Skip = times*takeCount;
        //        Console.WriteLine(@"Skip:" + query.Skip);
        //        var items = svc.Select(query);
        //        if (!items.Any())
        //        {
        //            break;
        //        }
        //        foreach (var item in items)
        //        {
        //            var examples = svc.Select(new ExampleQuery() {EntryID = item.ID});
        //            foreach (var example in examples)
        //            {
        //                if (!example.Origin.ToLower().Contains(item.Word.ToLower()))
        //                {
        //                    Console.WriteLine(string.Format("Word:{0},EntryID:{1},ExampleID:{2}", item.Word, item.ID,
        //                                                    example.ID));
        //                }
        //            }
        //        }
        //        times++;
        //    }
        //}
    }
}
