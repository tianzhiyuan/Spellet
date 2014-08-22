using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ors.Core.Data;

namespace Ors.Framework.Data
{

    public class DataServiceEventArgs : EventArgs
    {
        public IModel[] Items { get; internal set; }
    }
}
