using System.Collections.Generic;
using Nop.Plugin.Widgets.JustHtml.Data.Domain;

namespace Nop.Plugin.Widgets.JustHtml.Services
{
    public interface IJustHtmlWidgetService
    {
        void DeleteWidget(JustHtmlWidget widget);

        IList<JustHtmlWidget> GetAll();

        JustHtmlWidget GetById(int widgetId);

        void InsertWidget(JustHtmlWidget widget);

        void InsertWidgets(IEnumerable<JustHtmlWidget> widgets);

        void UpdateWidget(JustHtmlWidget widget);
    }
}