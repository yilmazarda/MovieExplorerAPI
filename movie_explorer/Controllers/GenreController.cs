using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using movie_explorer.Services;

namespace movie_explorer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        public readonly IGenreService _genreService;
        public readonly ILogger<GenreController> _logger;

        public GenreController(IGenreService genreService, ILogger<GenreController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenresAsync()
        {
            _logger.LogInformation("Getting all genres");

            try
            {
                var genres = await _genreService.GetAllGenresAsync();
                if (genres == null)
                {
                    _logger.LogWarning("Genres list is empty");
                    return NotFound();
                }
                _logger.LogInformation("Genres list retrieved successfully");
                return Ok(genres);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occured during getting all genres");
                return StatusCode(500, "External server error");
            }

        }
    }
}