using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using movie_explorer.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Newtonsoft.Json;


namespace movie_explorer.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly IExternalMovieService _externalMovieService;
        private readonly IDistributedCache _cache;

        public MovieService(IMovieRepository repository, IExternalMovieService externalMovieService, IDistributedCache cache) {
            _repository = repository;
            _externalMovieService = externalMovieService;
            _cache = cache;
        }

        public async Task<IQueryable<Movie>> GetAllMoviesAsync() => await _repository.GetAllAsync();


        public async Task<Movie> AddMovieAsync(Movie movie) => await _repository.AddAsync(movie);

        public async Task<Movie> UpdateMovieAsync(Movie movie) => await _repository.UpdateAsync(movie);

        public async Task DeleteMovieAsync(Movie movie) => await _repository.DeleteAsync(movie);

        public async Task<List<Movie>> FetchPopularMoviesAsync()
        {
            var movies = await _externalMovieService.FetchPopularMoviesAsync();

            foreach (var movie in movies)
            {
                var movie_data = await _repository.GetByTmdbIdAsync(movie.TmdbId);

                if (movie_data != null)
                {
                    movie_data.Name = movie.Name;
                    movie_data.Description = movie.Description;
                    movie_data.ImageUrl = movie.ImageUrl;
                    movie_data.Language = movie.Language;
                    movie_data.VoteCount = movie.VoteCount;
                    movie_data.VoteAverage = movie.VoteAverage;
                    movie_data.Popularity = movie.Popularity;
                    movie_data.ReleaseDate = movie.ReleaseDate;
                    movie_data.Genres = movie.Genres;
                    await _repository.UpdateAsync(movie_data);
                    continue;
                }
                
                await _repository.AddAsync(movie);
            }

            return movies;
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movie_data = await _cache.GetStringAsync($"Movie Id: {id.ToString()}"); //first check cache
            Movie movie = new Movie();
            if (movie_data != null)
            {
                movie = JsonConvert.DeserializeObject<Movie>(movie_data);
                Console.WriteLine("From cache");
                return movie;
            }

            movie = await _externalMovieService.FetchMovieByIdAsync(id); //if not exist in cache, fetch
            var jsonData = System.Text.Json.JsonSerializer.Serialize(movie);
            Console.WriteLine("From API");

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // 30 min lifetime
            };


            
            await _cache.SetStringAsync($"Movie Id: {id.ToString()}", jsonData, cacheOptions); //save to cache
            return movie;
        }
    }
}