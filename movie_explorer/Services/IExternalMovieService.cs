using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;

namespace movie_explorer.Services
{
    public interface IExternalMovieService
    {
        Task<List<Movie>> FetchPopularMoviesAsync(int page = 1);
        Task<List<Movie>> FetchTrendingMoviesAsync();

        Task<Movie> FetchMovieByIdAsync(int id);
    }
}