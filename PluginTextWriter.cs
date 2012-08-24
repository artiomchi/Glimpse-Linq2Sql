using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FlexLabs.Glimpse.Linq2Sql
{
    public class PluginTextWriter : TextWriter
    {
        private static PluginTextWriter _instance;
        private static readonly object _instanceLock = new object();
        public static PluginTextWriter Instance
        {
            get
            {
                if (_instance == null)
                    lock (_instanceLock)
                        if (_instance == null)
                            _instance = new PluginTextWriter();
                return _instance;
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(char value)
        {
            LogItemHandler.WriteLine(value.ToString());
        }

        public override void Write(string value)
        {
            if (value != null)
            {
                LogItemHandler.WriteLine(value);
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            if (buffer == null || index < 0 || count < 0 || buffer.Length - index < count)
            {
                base.Write(buffer, index, count); // delegate throw exception to base class
            }

            var value = new string(buffer, index, count);
            LogItemHandler.WriteLine(value);
        }
    }
}
