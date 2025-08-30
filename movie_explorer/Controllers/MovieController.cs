using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using movie_explorer.Services;
using movie_explorer.Models;

namespace movie_explorer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        public readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var movies = await _movieService.GetAllMoviesAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null) return NotFound();
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie) {
            await _movieService.AddMovieAsync(movie);
            return CreatedAtAction(nameof(Get), new { TmdbId = movie.TmdbId }, movie);
        }

        [HttpPost("popular/fetch")]
        public async Task<IActionResult> Fetch() {
            List<Movie> movies = await _movieService.FetchPopularMoviesAsync();
            return Ok(movies);
        }

        


    }
}