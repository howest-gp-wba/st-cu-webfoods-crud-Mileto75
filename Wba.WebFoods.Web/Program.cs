using Microsoft.EntityFrameworkCore;
using Wba.WebFoods.Web.Data;
using Wba.WebFoods.Web.Services;
using Wba.WebFoods.Web.Services.Interfaces;

namespace Wba.WebFoods.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Add/register database service
            builder.Services.AddDbContext<WebFoodsDbContext>(
                options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("WebFoodsDb"))
                );
            //add own services
            builder.Services.AddTransient<IFormBuilderService,FormBuilderService>();
            builder.Services.AddTransient<IFileService,FileService>();
            builder.Services.AddControllersWithViews();

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

            app.UseAuthorization();

            //route mapping for area
            app.MapControllerRoute(
                name: "areaDefault",
                pattern: "{area:exists}/{controller=Products}/{action=Index}/{id?}");
            //default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}