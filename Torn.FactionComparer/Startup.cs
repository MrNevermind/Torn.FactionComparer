using Microsoft.EntityFrameworkCore;
using Torn.FactionComparer.Infrastructure;
using Torn.FactionComparer.Services;

namespace Torn.FactionComparer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddTransient<ICompareDataRetriever, CompareDataRetriever>();
            services.AddTransient<IViewRenderer, ViewRenderer>();
            services.AddTransient<IImageGenerator, ImageGenerator>();

            services.AddDbContext<TornContext>(options => options.UseSqlite(Configuration.GetConnectionString("CacheConnString")));
            services.AddTransient<ITornContext, TornContext>();

            services.AddTransient<IDbService, DbService>();
            services.AddTransient<IStatsCalculator, StatsCalculator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure the HTTP request pipeline.
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}
