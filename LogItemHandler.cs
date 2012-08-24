using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace FlexLabs.Glimpse.Linq2Sql
{
    internal static class LogItemHandler
    {
        internal const string HttpItemID = "Glimpse.Linq2Sql";

        private static readonly object _contextLock = new object();
        private static LogItem _curLog, _lastLog;
        internal static void WriteLine(String value)
        {
            var context = HttpContext.Current;
            if (context == null)
                return;

            if (context.Items[HttpItemID] == null || _curLog == null)
                lock (_contextLock)
                {
                    if (context.Items[HttpItemID] == null)
                        context.Items[HttpItemID] = new List<LogItem>();
                    if (_curLog == null)
                        _curLog = new LogItem();
                }

            if (String.IsNullOrWhiteSpace(value))
            {
                List<LogItem> items = context.Items[HttpItemID] as List<LogItem>;
                items.Add(_lastLog = _curLog);
                _curLog = new LogItem();
                return;
            }

            _curLog.Command += value;

            try
            {
                var match = Regex.Match(value, @"-- @(\w+): \w+ (\w+) (\([\w\s=;-]+\)) \[(.*)\].*");
                if (match.Success)
                {
                    _curLog.Params.Add(new LogItem.Parameter
                    {
                        Name = match.Groups[1].Value,
                        Value = match.Groups[4].Value,
                        Type = match.Groups[2].Value,
                    });
                }
            }
            catch (Exception) { }
        }

        internal static void ConnectionClosed(TimeSpan period)
        {
            if (_lastLog != null)
                _lastLog.Duration = period;
        }

        internal static List<LogItem> GetLogList(HttpContextBase context)
        {
            if (context == null)
                return null;

            var items = context.Items[HttpItemID] as List<LogItem>;
            if (items != null && _curLog != null)
                items.Add(_curLog);

            return items;
        }
    }
}
