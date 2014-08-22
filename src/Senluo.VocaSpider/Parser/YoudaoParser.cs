using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Senluo.VocaSpider.Model;

namespace Senluo.VocaSpider.Parser
{
    public class YoudaoParser : IParser
    {


        public Entry Search(string word)
        {
            Entry entry = new Entry();
            var translation = new List<Translation>();
            var examples = new List<Example>();
            entry.Word = word;
            var url = string.Format("http://dict.youdao.com/search?q={0}", word);
            var client = new WebClient();
            client.Headers.Add("user-agent",
                               "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36");

            var bytes = client.DownloadData(url);
            var str = Encoding.UTF8.GetString(bytes);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(str);
            var pronounce = doc.DocumentNode.SelectNodes(@"//div[@class='baav']/span[@class='pronounce']");
            foreach (var p in pronounce)
            {
                if (p.InnerText.Contains("英"))
                {
                    var phonectic = p.SelectSingleNode("//span[@class='phonetic']");
                    if (phonectic != null)
                    {
                        entry.Phonetic_UK = TrimSpacing(phonectic.InnerText);

                        var audioUrl = string.Format("http://dict.youdao.com/dictvoice?audio={0}&type={1}", word, 1);
                        var path = string.Format(@"audio\{0}_{1}.mp3", word, 1);
                        downloadAndSaveMp3(path, audioUrl);
                        entry.Phonetic_UK_Audio = path;
                        //下载音频
                    }
                }
                else if (p.InnerText.Contains("美"))
                {
                    var phonectic = p.SelectSingleNode("//span[@class='phonetic']");
                    if (phonectic != null)
                    {
                        entry.Phonetic_US = TrimSpacing(phonectic.InnerText);
                        var audioUrl = string.Format("http://dict.youdao.com/dictvoice?audio={0}&type={1}", word, 2);
                        var path = string.Format(@"audio\{0}_{1}.mp3", word, 2);
                        downloadAndSaveMp3(path, audioUrl);
                        entry.Phonetic_US_Audio = path;
                    }
                }
            }
            var trans = doc.DocumentNode.SelectSingleNode(@"//div[@class='trans-container']").SelectNodes("ul/li");
            translation.AddRange(trans.Select(tran => new Translation() { Description = tran.InnerText }));

            var bilingualExamples = doc.DocumentNode.SelectNodes(@"//div[@id='bilingual']/ul/li");
            foreach (var example in bilingualExamples)
            {
                var p = example.SelectNodes("p");
                var ex = new Example();
                ex.Origin = TrimSpacing(string.Join("", p[0].ChildNodes.Select(o => o.InnerText)));
                ex.Trans = TrimSpacing(string.Join("", p[1].ChildNodes.Select(o => o.InnerText)));
                examples.Add(ex);
            }
            //    bilingualExamples.Each((example) =>
            //        {
            //            var ex = new Example();
            //            ex.Origin = string.Join("",
            //                                    example.ChildElements.ElementAt(0).ChildElements.Select(o => o.InnerText));
            //            ex.Trans = string.Join("", example.ChildElements.ElementAt(1).ChildElements.Select(o => o.InnerText));
            //            examples.Add(ex);
            //        });
            entry.Translations = translation;
            entry.Examples = examples;
            return entry;
        }
        string TrimSpacing(string origin)
        {
            var result = origin.Trim().Replace(@"\t", "").Replace(@"\n", "");
            return result;
        }
        void downloadAndSaveMp3(string path, string url)
        {
            try
            {
                var client = new WebClient();
                client.Headers.Add("user-agent",
                               "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/30.0.1599.101 Safari/537.36");
                var bytes = client.DownloadData(url);
                File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path), bytes);
            }
            catch { }
        }

        Entry IParser.Search(string word)
        {
            try
            {
                return this.Search(word);
            }
            catch
            {
            }
            return null;
        }

    }
}
