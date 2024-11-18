using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using MvcMovie.Models;  // Đảm bảo chỉ dùng đúng namespace chứa lớp Movie

namespace MvcMovie.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, MvcMovieContext context)
        {
            // Kiểm tra xem bảng Movie đã có dữ liệu chưa
            if (context.Movie.Any()) // Sử dụng đúng DbSet<Movie> từ MvcMovieContext
            {
                return; // Dữ liệu đã có sẵn, không cần thêm
            }

            // Thêm dữ liệu mẫu vào bảng Movie
            context.Movie.AddRange(
                new Movie
                {
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-1-11"),
                    Genre = "Romantic Comedy",
                    Price = 7.99M
                },
                new Movie
                {
                    Title = "Ghostbusters",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Price = 8.99M
                },
                new Movie
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Price = 9.99M
                },
                new Movie
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Price = 3.99M
                }
            );

            context.SaveChanges(); // Lưu các thay đổi vào cơ sở dữ liệu
        }
    }
}
