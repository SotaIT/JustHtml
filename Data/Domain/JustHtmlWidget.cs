using Nop.Core;

namespace Nop.Plugin.Widgets.JustHtml.Data.Domain
{
    public class JustHtmlWidget: BaseEntity
    {
        public int StoreId { get; set; }

        public bool IsActive { get; set; }

        public string Content { get; set; }

        public string WidgetZone { get; set; }

        public int Order { get; set; }
    }
}
