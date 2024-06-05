using BLLayer.Interface;
using BLLayer.Repository;
using DALayer.Context;
using DALayer.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PLayer.Mapper;

namespace PLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add Services
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<MenuDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });



            builder.Services.AddScoped(typeof(IGenericRepository<MenuItem>), typeof(GenericRepository<MenuItem>));
            builder.Services.AddScoped(typeof(IMenuRepository), typeof(MenuRepository));
            builder.Services.AddScoped(typeof(IGenericRepository<Category>), typeof(GenericRepository<Category>));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(M => M.AddProfile(new MapperProfile()));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                             .AddCookie(option =>
                             {
                                 option.LoginPath = "Account/Login";
                                 option.AccessDeniedPath = "Home/Error";

                             });


            builder.Services.AddIdentity<AppUser, IdentityRole>().
                            AddEntityFrameworkStores<MenuDbContext>().
                            AddDefaultTokenProviders();

            #endregion

            var app = builder.Build();
            #region MiddelWare

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
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run(); 
            #endregion
        }
    }
}
