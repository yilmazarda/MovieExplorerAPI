using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;

namespace movie_explorer.Services
{
    public interface IMovieService
    {
        Task<Movie> AddMovieAsync(Movie quiz);
        Task DeleteMovieAsync(Movie movie);
        Task<IQueryable<Movie>> GetAllMoviesAsync();
        Task<MovieDto> GetMovieByIdAsync(int id);
        Task<List<MovieDto>> GetPopularMoviesAsync(int page = 1);
        Task<List<MovieDto>> GetTrendingMoviesAsync();
        Task<Movie> UpdateMovieAsync(Movie quiz);
    }
}