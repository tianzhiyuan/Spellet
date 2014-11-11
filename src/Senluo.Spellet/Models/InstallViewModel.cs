using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Senluo.Spellet.Models
{
    public class InstallViewModel
    {
        public string DataBaseName { get; set; }
        public string MySqlUserName { get; set; }
        public string MySqlPassword { get; set; }
        public string Host { get; set; }
        public string AdminAccount { get; set; }
        public string AdminPassword { get; set; }
        public bool InstallSampleData { get; set; }
    }
}