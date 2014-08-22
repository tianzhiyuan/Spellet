using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Framework.File
{
    /// <summary>
    /// 文件
    /// </summary>
    public class File
    {
        public File()
        {
            Type = FileType.General;
        }
        public long FileID { get; set; }
        public int Size { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string RealPath { get; set; }
        public FileType Type { get; set; }
    }
    public class FileQuery
    {
        public long[] FileIDs { get; set; }
    }
}
