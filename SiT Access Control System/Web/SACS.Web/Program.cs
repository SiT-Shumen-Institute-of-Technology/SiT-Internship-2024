using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SACS.Data;
using SACS.Data.Common;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Data.Repositories;
using SACS.Data.Seeding;
using SACS.Data.Seeding.SACS.Data.Seeding;
using SACS.Services.Data;
using SACS.Services.Data.Interfaces;
using SACS.Services.Mapping;
using SACS.Services.Messaging;
using SACS.Web.Profiles;
using SACS.Web.ViewModels;

namespace SACS.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        ConfigureServices(builder.Services, builder.Configuration);

        var app = builder.Build();
        ConfigureApplication(app).GetAwaiter().GetResult(); // Await Configure method
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // Connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException(
                                   "Connection string 'DefaultConnection' not found.");

        // Database context
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Identity configuration
        services.AddIdentity<ApplicationUser, ApplicationRole>(IdentityOptionsProvider.GetIdentityOptions)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

        // Cookie policy
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        // MVC and Razor configuration
        services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        }).AddRazorRuntimeCompilation();

        services.AddRazorPages();
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton(configuration);

        // AutoMapper configuration
        var currentAssembly = Assembly.GetExecutingAssembly();
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
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IDashboardService, DashboardService>();

        services.AddSingleton<IDateTimeProviderService, DateTimeProviderService>();
    }

    private static async Task ConfigureApplication(WebApplication app)
    {
        // Database migration and seeding
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();

            await new ApplicationDbContextSeeder().SeedAsync(dbContext, scope.ServiceProvider);
            await AdminSeeder.SeedAdminAsync(scope.ServiceProvider);
        }

        // Register AutoMapper mappings
        AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

        // Environment configuration
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

        // Middleware configuration
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCookiePolicy();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // Request buffering middleware
        app.Use(async (context, next) =>
        {
            context.Request.EnableBuffering();
            await next();
        });

        // Routes configuration
        app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();
    }
}