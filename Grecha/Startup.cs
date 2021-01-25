using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DAL.DbContext;
using DAL.Helpers;
using DAL.Parsing.ParserContext;
using Grecha.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Grecha
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
 
        private IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Develop");
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddControllersWithViews();
            services.AddTransient<IOptionsBuilderService<AppDbContext>, OptionsBuilderService<AppDbContext>>();
            services.AddHostedService<ParsingService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //Parser.Initialize(app.ApplicationServices); Add Parsing data, if last parse more than 7 days ago or Database Empty 
        }
    }
}
