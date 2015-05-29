using Glimpse.AspNet.Extensibility;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlexLabs.Glimpse.Linq2Sql
{
    public class PluginCore : AspNetTab, ITabLayout
    {
        public override string Name
        {
            get { return "Linq2SQL"; }
        }

        public override object GetData(ITabContext context)
        {
            var plugin = Plugin.Create("No", "Started", "Duration", "Command", "Parameters");

            var requestContext = context.GetRequestContext<HttpContextBase>();
            List<LogItem> items = LogItemHandler.GetLogList(requestContext);

            if (items == null)
                return plugin;

            var count = 0;
            foreach (var item in items)
            {
                plugin.AddRow()
                    .Column(count++)
                    .Column(String.Format("{0:#,0}", item.Time.Subtract(requestContext.Timestamp).TotalMilliseconds))
                    .Column(item.Duration.HasValue ? String.Format("{0:#,0}", item.Duration.Value.TotalMilliseconds) : null)
                    .Column(item.Command)
                    .Column(item.Params.Count > 0
                        ? new[] { new object[] { "Name", "Type", "Value" } }.Concat(item.Params.Select(p => new object[]
                            {
                                p.Name,
                                p.Type,
                                p.Value,
                            }))
                        : null);
            }

            return plugin;
        }

        public object GetLayout()
        {
            return Layout;
        }

        private static readonly object Layout = TabLayout.Create()
            .Row(r =>
            {
                r.Cell(0).WidthInPixels(30).AsKey();
                r.Cell(1).WidthInPixels(75).AlignRight().Prefix("T+ ").Suffix(" ms");
                r.Cell(2).WidthInPixels(65).AlignRight().Suffix(" ms");
                r.Cell(3).WidthInPercent(65).AsCode(CodeType.Sql).DisablePreview();
                r.Cell(4).WidthInPixels(270).LimitTo(1).SetLayout(TabLayout.Create()
                    .Row(x =>
                    {
                        x.Cell(0).WidthInPixels(50);
                        x.Cell(1).WidthInPixels(60);
                        x.Cell(2).WidthInPixels(120);
                    }));
            }).Build();
    }
}
