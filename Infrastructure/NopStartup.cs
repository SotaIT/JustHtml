using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Plugin.Widgets.JustHtml.Data;
using Nop.Web.Framework.Infrastructure.Extensions;

namespace Nop.Plugin.Widgets.JustHtml.Infrastructure
{
    public class NopStartup: INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add object context
            services.AddDbContext<WidgetObjectContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServerWithLazyLoading(services);
            });

        }

        public void Configure(IApplicationBuilder application)
        {
            
        }

        public int Order => 100;
    }
}