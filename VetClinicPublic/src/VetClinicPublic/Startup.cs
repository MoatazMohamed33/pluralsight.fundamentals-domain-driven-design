using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VetClinicPublic.Configuration;
using VetClinicPublic.Interfaces;
using VetClinicPublic.Services;

namespace VetClinicPublic
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureFrom(_configuration);

            services.AddSingleton<ISendEmail, SmtpEmailSender>();
            services.AddSingleton<ISendConfirmationEmail, ConfirmationEmailSender>();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }

    internal static class ConfigurationExtensions
    {
        internal static void ConfigureFrom(this IServiceCollection services, IConfiguration configuration)
        {
            var siteConfiguration = configuration.GetSection("Site");
            services.Configure<SiteConfiguration>(siteConfiguration);

            var mailConfiguration = configuration.GetSection("Mail");
            services.Configure<MailConfiguration>(mailConfiguration);
        }
    }
}
