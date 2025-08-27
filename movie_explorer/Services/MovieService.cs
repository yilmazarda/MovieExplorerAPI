using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using movie_explorer.Repositories;

namespace movie_explorer.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;

        public MovieService(IMovieRepository repository) {
            _repository = repository;
        }

        public async Task<IQueryable<Movie>> GetAllMoviesAsync() => await _repository.GetAllAsync();

        public async Task<Movie> GetMovieByIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<Movie> AddMovieAsync(Movie movie) => await _repository.AddAsync(movie);

        public async Task<Movie> UpdateMovieAsync(Movie movie) => await _repository.UpdateAsync(movie);

        public async Task DeleteMovieAsync(Movie movie) => await _repository.DeleteAsync(movie);
    }
}