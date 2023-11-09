using Microsoft.EntityFrameworkCore;
using Wba.WebFoods.Web.Data;

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
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            builder
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}