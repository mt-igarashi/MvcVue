using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace VueMvc.Models
{
    /// <summary>
    /// DB初期化クラス(未使用)
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// 初期化を実行します。
        /// </summary>
        /// <param name="serviceProvider">サービスプロバイダ</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>()))
            {
                // Look for any movies.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                     new Movie
                     {
                         Title = "When Harry Met Sally",
                         ReleaseDate = DateTime.Parse("1989-1-11"),
                         Genre = "Romantic Comedy",
                         Rating = "R",
                         Price = 7.99M
                     },

                     new Movie
                     {
                         Title = "Ghostbusters ",
                         ReleaseDate = DateTime.Parse("1984-3-13"),
                         Genre = "Comedy",
                         Rating = "R",
                         Price = 8.99M
                     },

                     new Movie
                     {
                         Title = "Ghostbusters 2",
                         ReleaseDate = DateTime.Parse("1986-2-23"),
                         Genre = "Comedy",
                         Rating = "R",
                         Price = 9.99M
                     },

                     new Movie
                     {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Rating = "R",
                        Price = 3.99M
                     }
                );
                context.SaveChanges();

                context.Author.AddRange(
                     new Author
                     {
                         MovieID = 1,
                         Name = "Jhon Smith",
                         Sex = "1",
                         Age = 40,
                         UpdateDate = DateTime.Now
                     },

                     new Author
                     {
                         MovieID = 2,
                         Name = "Emmy Godon",
                         Sex = "2",
                         Age = 30,
                         UpdateDate = DateTime.Now
                     },

                     new Author
                     {
                         MovieID = 3,
                         Name = "Willam Goldman",
                         Sex = "1",
                         Age = 35,
                         UpdateDate = DateTime.Now
                     },

                     new Author
                     {
                         MovieID = 4,
                         Name = "Jim Kelly",
                         Sex = "1",
                         Age = 35,
                         UpdateDate = DateTime.Now
                     }
                );
                context.SaveChanges();
            }
        }
    }
}