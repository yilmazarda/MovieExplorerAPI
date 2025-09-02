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
        public readonly ILogger<MovieController> _logger;

        public MovieController(IMovieService movieService, ILogger<MovieController> logger)
        {
            _movieService = movieService;
            _logger = logger;
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
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            await _movieService.AddMovieAsync(movie);
            return CreatedAtAction(nameof(Get), new { TmdbId = movie.TmdbId }, movie);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular(int page = 1) {
            _logger.LogInformation("Getting popular movies");

            try
            {
                List<MovieDto> movies = await _movieService.GetPopularMoviesAsync(page);
                if (movies == null)
                {
                    _logger.LogWarning("Popular movies list is empty");
                    return NotFound();
                }
                _logger.LogInformation("Popular movies retrieved succesfully");
                return Ok(movies);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured during getting popular movies list");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("trending")]
        public async Task<IActionResult> GetTrending() {
            _logger.LogInformation("Getting trending movies");

            try
            {
                List<MovieDto> movies = await _movieService.GetTrendingMoviesAsync();
                if (movies == null)
                {
                    _logger.LogWarning("Trending movies list is empty");
                    return NotFound();
                }
                _logger.LogInformation("Trending movies retrieved successfully");
                return Ok(movies);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured during getting trending movies list");
                return StatusCode(500, "Internal server error");
            }
        }
        
    }
}