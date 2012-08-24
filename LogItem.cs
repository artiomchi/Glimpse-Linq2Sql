using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexLabs.Glimpse.Linq2Sql
{
    internal class LogItem
    {
        public DateTime Time = DateTime.Now;
        public TimeSpan? Duration;
        public String Command;
        public IList<Parameter> Params = new List<Parameter>();

        public class Parameter
        {
            public String Name { get; set; }
            public String Value { get; set; }
            public String Type { get; set; }

        }
    }
}
