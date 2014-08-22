using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ors.Framework.File
{
    /// <summary>
    /// 文件服务接口
    /// </summary>
    public interface IFileService
    {
        void Create(params File[] items);
        void Delete(params File[] items);
        IEnumerable<File> Get(FileQuery query);
        void Rename(params File[] items);
    }
}
