using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Models;
using System;
using System.Linq;

namespace MvcMovie.Data
{
    public class SeedData
    {
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
                        ReleaseDate = DateTime.Parse("1989-2-12"),
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
                    },
                     new Movie
                     {
                         Title = "A Bravo",
                         ReleaseDate = DateTime.Parse("1959-4-16"),
                         Genre = "Western",
                         Rating = "R",
                         Price = 4M
                     },
                    new Movie
                    {
                        Title = "B Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-17"),
                        Genre = "Western",
                        Rating = "R",
                        Price = 5M
                    },
                    new Movie
                    {
                        Title = "C Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-18"),
                        Genre = "Western",
                        Rating = "R",
                        Price = 6M
                    }, new Movie
                    {
                        Title = "D Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-19"),
                        Genre = "Western",
                        Rating = "R",
                        Price = 7M
                    },
                    new Movie
                    {
                        Title = "E Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-19"),
                        Genre = "Western",
                        Rating = "R",
                        Price = 8M
                    },
                    new Movie
                    {
                        Title = "F Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-20"),
                        Genre = "Western",
                        Rating = "R",
                        Price = 9M
                    }
                );

                var movielist = Enumerable.Range(1, 20).Select(x => new Movie
                {
                    Title = "Rio " + x.ToString(),
                    ReleaseDate = DateTime.Parse("1959-4-" + x),
                    Genre = "Comedy",
                    Rating = "R",
                    Price = 5M
                });

                context.Movie.AddRange(movielist);
                context.SaveChanges();
            }
        }
    }
}
