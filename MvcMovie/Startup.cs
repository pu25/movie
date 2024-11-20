namespace MvcMovie
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MvcMovie.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Cấu hình DbContext với chuỗi kết nối từ appsettings.json
            services.AddDbContext<MvcMovieContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MvcMovieContext")));

            // Thêm dịch vụ MVC
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Kiểm tra kết nối cơ sở dữ liệu
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<MvcMovieContext>();
                try
                {
                    if (context.Database.CanConnect())
                    {
                        logger.LogInformation("Kết nối cơ sở dữ liệu thành công!");
                    }
                    else
                    {
                        logger.LogWarning("Không thể kết nối cơ sở dữ liệu.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Kết nối cơ sở dữ liệu thất bại: {ex.Message}");
                }
            }

            // Kiểm tra và thêm dữ liệu mẫu nếu cần thiết
            var scopeData = app.ApplicationServices.CreateScope();
            var contextData = scopeData.ServiceProvider.GetRequiredService<MvcMovieContext>();
            SeedData.Initialize(scopeData.ServiceProvider, contextData);
        }
    }
}
