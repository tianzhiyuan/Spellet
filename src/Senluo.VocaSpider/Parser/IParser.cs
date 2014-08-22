using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Senluo.VocaSpider.Model;

namespace Senluo.VocaSpider.Parser
{
    public interface IParser
    {
        Entry Search(string word);
    }
}
