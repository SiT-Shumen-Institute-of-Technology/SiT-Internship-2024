using SACS.Data;
using SACS.Data.Common;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Data.Repositories;
using SACS.Data.Seeding;
using SACS.Services.Data;
using SACS.Services.Mapping;
using SACS.Services.Messaging;
using SACS.Web.Profiles;
using SACS.Web.ViewModels;

namespace SACS.Web
{
    using System.Reflection;
    using global::SACS.Services.Data.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    namespace SACS.Web
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddControllers()
                    .AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; });

                ConfigureServices(builder.Services, builder.Configuration);
                var app = builder.Build();
                Configure(app);
                app.UseHttpsRedirection();
                app.MapControllers();
                app.UseRouting();
                app.UseAuthorization();
                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

                app.Run();
            }

            private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
            {
                services.AddDbContext<ApplicationDbContext>(
                    options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

                services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                    .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

                services.Configure<CookiePolicyOptions>(
                    options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

                services.AddControllersWithViews(
                        options => { options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()); })
                    .AddRazorRuntimeCompilation();
                services.AddRazorPages();
                services.AddDatabaseDeveloperPageExceptionFilter();

                services.AddSingleton(configuration);

                Assembly currentAssembly = Assembly.GetExecutingAssembly();
                services.AddAutoMapper(currentAssembly);
                services.AddAutoMapper(typeof(EmployeeScheduleProfile));
                services.AddAutoMapper(typeof(EmployeeProfile));
                services.AddAutoMapper(typeof(SummaryProfile));

                // Data repositories
                services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
                services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
                services.AddScoped<IDbQueryRunner, DbQueryRunner>();

                // Application services
                services.AddTransient<IEmailSender, NullMessageSender>();
                services.AddTransient<IEmployeeService, EmployeeService>();
                services.AddTransient<IDepartmentService, DepartmentService>();
                services.AddTransient<IDayService, DayService>();
                services.AddTransient<ISummaryService, SummaryService>();
                services.AddTransient<IRFIDCardService, RFIDCardService>();
                services.AddTransient<IEmployeeRFIDCardService, EmployeeRFIDCardService>();
                services.AddTransient<IScheduleService, ScheduleService>();
                services.AddTransient<IDateTimeProviderService, DateTimeProviderService>();
            }

            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();

                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.Use(async (context, next) =>
                {
                    context.Request.EnableBuffering();
                    await next();
                });

                app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            }

            private static void Configure(WebApplication app)
            {
                // Seed data on application startup
                using (var serviceScope = app.Services.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
                    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter()
                        .GetResult();
                }

                AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseCookiePolicy();

                app.UseDeveloperExceptionPage();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                app.MapRazorPages();
            }
        }
    }
}
