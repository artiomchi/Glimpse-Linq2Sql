using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Glimpse.Core.Extensibility;

namespace FlexLabs.Glimpse.Linq2Sql
{
    [GlimpsePlugin]
    internal class PluginCore : IGlimpsePlugin, IProvideGlimpseStructuredLayout
    {
        public string Name
        {
            get { return "Linq2SQL"; }
        }

        public void SetupInit()
        {
        }

        public object GetData(HttpContextBase context)
        {
            List<LogItem> items = LogItemHandler.GetLogList(context);
            if (items == null)
                return null;

            var data = new List<object[]> { new[] { "No", "Started", "Duration", "Command", "Parameters" } };
            data.AddRange(items.Select((item, i) =>
                new object[] 
                {
                    i,
                    TimeSpanString(item.Time.Subtract(context.Timestamp)),
                    item.Duration.HasValue ? TimeSpanString(item.Duration.Value) : null,
                    item.Command,
                    item.Params.Count > 0
                        ? new[]{new object[]{"Name","Type","Value"}}.Concat(item.Params.Select(p => new object[]
                            {
                                p.Name,
                                p.Type,
                                p.Value,
                            }))
                        : "--" as object
                }));

            return data;
        }

        private static String TimeSpanString(TimeSpan period)
        {
            var hours = period.Days * 24 + period.Hours;
            return 
                (hours > 0 ? period.Hours + " hrs, " : null) +
                (hours > 0 || period.Minutes > 0 ? period.Minutes + " min, " : null) +
                (hours > 0 || period.Minutes > 0 || period.Seconds > 0 ? period.Seconds + " sec, " : null) +
                period.Milliseconds + " ms";
        }

        private static readonly GlimpseStructuredLayout _structuredLayout = new GlimpseStructuredLayout
        {
            new GlimpseStructuredLayoutSection
            {
                new GlimpseStructuredLayoutCell
                {
                    Data = 0,
                    Width = "25px",
                },
                new GlimpseStructuredLayoutCell
                {
                    Data = 1,
                    Width = "60px",
                },
                new GlimpseStructuredLayoutCell
                {
                    Data = 2,
                    Width = "60px",
                },
                new GlimpseStructuredLayoutCell
                {
                    Data = 3,
                    Width = "65%",
                    IsCode = true,
                    CodeType = "sql",
                },
                new GlimpseStructuredLayoutCell
                {
                    Data = 4,
                    Width = "100px",
                    Structure = new GlimpseStructuredLayout
                    {
                        new GlimpseStructuredLayoutSection
                        {
                            new GlimpseStructuredLayoutCell
                            {
                                Data = 0,
                                Width = "25px",
                            },
                            new GlimpseStructuredLayoutCell
                            {
                                Data = 1,
                                Width = "35px",
                            },
                            new GlimpseStructuredLayoutCell
                            {
                                Data = 2,
                                Width = "35px",
                            },
                        },
                    }
                },
            }
        };

        public GlimpseStructuredLayout StructuredLayout
        {
            get { return _structuredLayout; }
        }
    }
}
