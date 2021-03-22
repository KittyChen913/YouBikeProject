using System.Collections.Generic;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YouBikeProject.Services;

namespace YouBikeProject
{
    public class Startup
    {
        private readonly string hangfirePostgreDBConnection;
        private readonly List<string> corsAllowHosts;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            hangfirePostgreDBConnection = Configuration.GetConnectionString("HangfirePostgreDBString");
            corsAllowHosts = Configuration.GetSection("Cors:Hosts").Get<List<string>>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddHangfire(configuration => configuration
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(hangfirePostgreDBConnection)
            );

            services.AddHangfireServer(options => options.WorkerCount = 1);
            services.AddHttpClient();
            services.AddSingleton<IYoubike, Youbike>();
            services.AddSingleton<IHttpClientHelpers, HttpClientHelpers>();
            services.AddSingleton<IDBHelpers, DBHelpers>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                builder =>
                {
                    builder.WithOrigins(corsAllowHosts.ToArray())
                      .AllowAnyHeader()
                      .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IYoubike youbike, IBackgroundJobClient backgroundJob, IRecurringJobManager recurringJob, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseHangfireDashboard();
            recurringJob.AddOrUpdate("Get Youbike 1hr Log.", () => youbike.GetYoubikeAPI(), Cron.Hourly);

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
