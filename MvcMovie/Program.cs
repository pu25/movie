using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using MvcMovie.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Thêm các dịch vụ vào container
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext")));

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Thêm middleware để kiểm tra routing
app.Use(async (context, next) =>
{
    Console.WriteLine($"Routing to: {context.Request.Path}");
    await next();
});

app.MapDefaultControllerRoute();

// Kiểm tra kết nối cơ sở dữ liệu và seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MvcMovieContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

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

        SeedData.Initialize(scope.ServiceProvider, context);
    }
    catch (Exception ex)
    {
        logger.LogError($"Kết nối cơ sở dữ liệu thất bại: {ex.Message}");
    }
}

app.Run();
