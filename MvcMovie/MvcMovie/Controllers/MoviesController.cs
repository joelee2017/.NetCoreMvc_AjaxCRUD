﻿using BackendServices.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Helper;
using MvcMovie.Models;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string movieGenre, string searchString)
        {


            // Use LINQ to get list of genres.
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
            };

            return View(movieGenreVM);
        }

        public async Task<IActionResult> Page(MovieQuery movieQuery)
        {
            PagedListModel<MovieViewModel> pagedListModel = new PagedListModel<MovieViewModel>();

            var movies = from m in _context.Movie
                         select m;


            if (!string.IsNullOrEmpty(movieQuery.searchString))
            {
                movies = movies.Where(s => s.Title.Contains(movieQuery.searchString));
            }

            if (!string.IsNullOrEmpty(movieQuery.movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieQuery.movieGenre);
            }

            // 排序
            if (!string.IsNullOrWhiteSpace(movieQuery.OrderType) && !string.IsNullOrWhiteSpace(movieQuery.OrderName))
            {
                IQueryable<Movie> orderByResult =
                    movieQuery.OrderType.ToLower() == "desc" ?
                    movies.ColumnOrderByDescending(movieQuery.OrderName) : movies.ColumnOrdersBy(movieQuery.OrderName);

                pagedListModel.Total = orderByResult.Count(); // 查詢資料總數
                pagedListModel.Items = orderByResult.ModelListConvert<MovieViewModel>()
                .ToPagedList(movieQuery.Page < 1 ? 1 : movieQuery.Page, movieQuery.PageSize);
            }
            else
            {
                pagedListModel.Total = movies.Count(); // 查詢資料總數
                pagedListModel.Items = movies.ModelListConvert<MovieViewModel>()
                    .ToPagedList(movieQuery.Page < 1 ? 1 : movieQuery.Page, movieQuery.PageSize);
            }
            

            // 重新指定排序順序
            pagedListModel.OrderType = movieQuery.OrderType == "Asc" ? "Asc" : "Desc";

            return View("~/Views/Movies/_PagePartialView.cshtml", pagedListModel);
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateModel]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(movie);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(movie);

            _context.Add(movie);
            await _context.SaveChangesAsync();

            Result result = new Result() { IsSuccess = true, Message = "成功" };

            return this.Json(new { result.IsSuccess, result.Message });
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}
