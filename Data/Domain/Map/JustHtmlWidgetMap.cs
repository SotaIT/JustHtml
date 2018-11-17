using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;

namespace Nop.Plugin.Widgets.JustHtml.Data.Domain.Map
{
    public class JustHtmlWidgetMap: NopEntityTypeConfiguration<JustHtmlWidget>
    {
        public override void Configure(EntityTypeBuilder<JustHtmlWidget> builder)
        {
            builder.ToTable(nameof(JustHtmlWidget));
            builder.HasKey(x => x.Id);
        }
    }
}