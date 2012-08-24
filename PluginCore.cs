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
                    String.Format("{0:#,0}", item.Time.Subtract(context.Timestamp).TotalMilliseconds),
                    item.Duration.HasValue ? String.Format("{0:#,0}", item.Duration.Value.TotalMilliseconds) : null,
                    item.Command,
                    item.Params.Count > 0
                        ? new[]{new object[]{"Name","Type","Value"}}.Concat(item.Params.Select(p => new object[]
                            {
                                p.Name,
                                p.Type,
                                p.Value,
                            }))
                        : null
                }));

            return data;
        }

        private static readonly GlimpseStructuredLayout _structuredLayout = new GlimpseStructuredLayout
        {
            new GlimpseStructuredLayoutSection
            {
                new GlimpseStructuredLayoutCell
                {
                    Data = 0,
                    Width = "30px",
                },
                new GlimpseStructuredLayoutCell
                {
                    Data = 1,
                    Width = "75px",
                    Align = "right",
                    Prefix = "T+ ",
                    Postfix = " ms",
                },
                new GlimpseStructuredLayoutCell
                {
                    Data = 2,
                    Width = "65px",
                    Align = "right",
                    Postfix = " ms",
                },
                new GlimpseStructuredLayoutCell
                {
                    Data = 3,
                    Width = "65%",
                    IsCode = true,
                    CodeType = "sql",
                    SuppressAutoPreview = false,
                },
                new GlimpseStructuredLayoutCell
                {
                    Data = 4,
                    Width = "270px",
                    Limit = 0,
                    Structure = new GlimpseStructuredLayout
                    {
                        new GlimpseStructuredLayoutSection
                        {
                            new GlimpseStructuredLayoutCell
                            {
                                Data = 0,
                                Width = "50px",
                            },
                            new GlimpseStructuredLayoutCell
                            {
                                Data = 1,
                                Width = "60px",
                            },
                            new GlimpseStructuredLayoutCell
                            {
                                Data = 2,
                                Width = "120px",
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
