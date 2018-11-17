using Autofac;
using Autofac.Core;
using Nop.Core.Caching;
using Nop.Core.Configuration;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Widgets.JustHtml.Controllers;
using Nop.Plugin.Widgets.JustHtml.Data;
using Nop.Plugin.Widgets.JustHtml.Data.Domain;
using Nop.Plugin.Widgets.JustHtml.Services;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Plugin.Widgets.JustHtml.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<JustHtmlWidgetService>().As<IJustHtmlWidgetService>().InstancePerLifetimeScope();

            builder.RegisterPluginDataContext<WidgetObjectContext>("nop_object_context_justhtml");

            builder.RegisterType<EfRepository<JustHtmlWidget>>()
                .As<IRepository<JustHtmlWidget>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("nop_object_context_justhtml"))
                .InstancePerLifetimeScope();

            builder.RegisterType<JustHtmlWidgetController>()
                .WithParameter(ResolvedParameter.ForNamed<ICacheManager>("nop_cache_static"));
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 100;
    }
}
