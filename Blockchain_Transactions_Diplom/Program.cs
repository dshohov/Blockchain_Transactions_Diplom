using Blockchain_Transactions_Diplom.Data;
using Blockchain_Transactions_Diplom.Helpers;
using Blockchain_Transactions_Diplom.IHelpers;
using Blockchain_Transactions_Diplom.Initializer;
using Blockchain_Transactions_Diplom.Interfaces;
using Blockchain_Transactions_Diplom.IRepositories;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
using Blockchain_Transactions_Diplom.Repositories;
using Blockchain_Transactions_Diplom.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blockchain_Transactions_Diplom
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Blockchain_Transactions_Diplom")));
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddSingleton<CoinApp>();
            builder.Services.AddSingleton<ICoinService, CoinService>();
            builder.Services.AddTransient<IPostmarkEmail, PostmarkEmail>();
            builder.Services.AddTransient<ILiqPayInvoice, LiqPayInvoice>();
            builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
            builder.Services.AddScoped<IExerciseService,ExerciseService>();
            builder.Services.AddScoped<ISmartContractRepository, SmartContractRepository>();
            builder.Services.AddScoped<ISmartContractService, SmartContractService>();  
            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("Postmark"));
            builder.Services.Configure<LiqPayOptions>(builder.Configuration.GetSection("LiqPay"));
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                // options.SignIn.RequireConfirmedAccount = true;
            });
            builder.Services.AddTransient<AdminInitializer>();
            builder.Services.AddTransient<RecoveryInitializer>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/LogOff";
                });

            var app = builder.Build();

            // Initialize data
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dataInitializer = serviceProvider.GetRequiredService<AdminInitializer>();
                dataInitializer.Initialize().Wait();
                var dataRecoveryInitializer = serviceProvider.GetRequiredService<RecoveryInitializer>();
                dataRecoveryInitializer.Initialize().Wait();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
