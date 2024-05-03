using Blockchain_Transactions_Diplom.Data;
using Blockchain_Transactions_Diplom.Helpers;
using Blockchain_Transactions_Diplom.Interfaces;
using Blockchain_Transactions_Diplom.IServices;
using Blockchain_Transactions_Diplom.Models;
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
            builder.Services.AddDbContext<ApplicationDBContext>(e =>
                e.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddTransient<ISendGridEmail, SendGridEmail>();
            builder.Services.AddControllersWithViews();
            builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration.GetSection("SendGrid"));
            builder.Services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 5;
                opt.Password.RequireLowercase = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(10);
                opt.Lockout.MaxFailedAccessAttempts = 5;
                //opt.SignIn.RequireConfirmedAccount = true;
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/LogOff";
                });
            var app = builder.Build();

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