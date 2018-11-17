using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Plugin.Widgets.JustHtml.Data.Domain;

namespace Nop.Plugin.Widgets.JustHtml.Services
{
    public class JustHtmlWidgetService : IJustHtmlWidgetService
    {
        private readonly IRepository<JustHtmlWidget> _repository;

        public JustHtmlWidgetService(IRepository<JustHtmlWidget> repository)
        {
            _repository = repository;
        }

        public void DeleteWidget(JustHtmlWidget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget));

            _repository.Delete(widget);
        }

        public IList<JustHtmlWidget> GetAll()
        {
            var query = from gp in _repository.Table
                        orderby gp.Id
                        select gp;
            var records = query.ToList();
            return records;
        }

        public JustHtmlWidget GetById(int widgetId)
        {
            return _repository.GetById(widgetId);
        }


        public void InsertWidget(JustHtmlWidget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget));

            _repository.Insert(widget);
        }

        public void UpdateWidget(JustHtmlWidget widget)
        {
            if (widget == null)
                throw new ArgumentNullException(nameof(widget));

            _repository.Update(widget);
        }

        public void InsertWidgets(IEnumerable<JustHtmlWidget> widgets)
        {
            if (widgets == null)
                throw new ArgumentNullException(nameof(widgets));

            foreach (var widget in widgets)
            {
                InsertWidget(widget);
            }
        }
    }
}