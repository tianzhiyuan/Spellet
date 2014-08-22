using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Framework.File
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FileTypeAcceptExtensionAttribute : Attribute
    {
        public FileTypeAcceptExtensionAttribute(string extensions)
        {
            if (string.IsNullOrWhiteSpace(extensions))
            {
                AcceptExtensions = new string[0];
            }
            else
            {
                AcceptExtensions =
                    extensions.Split(',')
                              .Where(o => !string.IsNullOrWhiteSpace(o))
                              .Select(o => o.Trim().ToLower())
                              .ToArray();
            }
        }

        public string[] AcceptExtensions { get; set; }
    }
    public enum FileType
    {
        General = 1,
        [Description("")]
        [FileTypeAcceptExtension("jpg,bmp,jpeg,png")]
        Picture = 2,
        [FileTypeAcceptExtension("doc,docx,xls,xlsx,pdf,txt,rtf")]
        Document = 3,
        [FileTypeAcceptExtension("mpg,mp4")]
        Video = 4,
        [FileTypeAcceptExtension("mp3,wav,rm")]
        Audio = 5,
    }
}
