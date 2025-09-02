using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using movie_explorer.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using AutoMapper;


namespace movie_explorer.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;
        private readonly IExternalMovieService _externalMovieService;
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository repository, IExternalMovieService externalMovieService, IDistributedCache cache, IMapper mapper) {
            _repository = repository;
            _externalMovieService = externalMovieService;
            _cache = cache;
            _mapper = mapper;
        }

        public async Task<IQueryable<Movie>> GetAllMoviesAsync() => await _repository.GetAllAsync();


        public async Task<Movie> AddMovieAsync(Movie movie) => await _repository.AddAsync(movie);

        public async Task<Movie> UpdateMovieAsync(Movie movie) => await _repository.UpdateAsync(movie);

        public async Task DeleteMovieAsync(Movie movie) => await _repository.DeleteAsync(movie);

        public async Task<List<MovieDto>> GetPopularMoviesAsync(int page = 1)
        {
            var popular_movies = await _cache.GetStringAsync("Popular-movies page: " + page.ToString() + ""); //first check cache
            List<Movie> movies = new List<Movie>();
            var movieDtos = new List<MovieDto>();

            if (popular_movies != null)
            {
                movieDtos = JsonConvert.DeserializeObject<List<MovieDto>>(popular_movies);
                Console.WriteLine("From cache");
                return movieDtos;
            }


            movies = await _externalMovieService.FetchPopularMoviesAsync(page);
            movieDtos = _mapper.Map<List<MovieDto>>(movies);

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
                    movie_data.LastUpdated = DateTime.UtcNow;
                    await _repository.UpdateAsync(movie_data);
                    continue;
                }
                
                await _repository.AddAsync(movie);
            }

            var jsonData = JsonConvert.SerializeObject(movieDtos);
            Console.WriteLine("From API");
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // 30 min lifetime
            };

            await _cache.SetStringAsync("Popular-movies page: " + page.ToString() + "", jsonData, cacheOptions);

            return movieDtos;
        }

        public async Task<List<MovieDto>> GetTrendingMoviesAsync()
        {
            var trending_movies = await _cache.GetStringAsync("Trending-movies"); //first check cache
            List<Movie> movies = new List<Movie>();
            var movieDtos = new List<MovieDto>();
            if (trending_movies != null)
            {
                movieDtos = JsonConvert.DeserializeObject<List<MovieDto>>(trending_movies);
                Console.WriteLine("From cache");
                return movieDtos;
            }

            movies = await _externalMovieService.FetchTrendingMoviesAsync();

            movieDtos = _mapper.Map<List<MovieDto>>(movies);


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
                    movie_data.ReleaseDate = movie.ReleaseDate;c
                    movie_data.LastUpdated = DateTime.UtcNow;
                    await _repository.UpdateAsync(movie_data);
                    continue;
                }
                
                await _repository.AddAsync(movie);
            }

            var jsonData = JsonConvert.SerializeObject(movieDtos);
            Console.WriteLine("From API");
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // 30 min lifetime
            };

            await _cache.SetStringAsync("Trending-movies", jsonData, cacheOptions);

            return movieDtos;
        }
        

        public async Task<MovieDto> GetMovieByIdAsync(int id)
        {
            var movie_data = await _cache.GetStringAsync($"Movie Id: {id.ToString()}"); //first check cache
            var movieDto = new MovieDto();
            var movie = new Movie();
            if (movie_data != null)
            {
                movieDto = JsonConvert.DeserializeObject<MovieDto>(movie_data);
                Console.WriteLine("From cache");
                return movieDto;
            }
            else 
            {
                movie = await _repository.GetByTmdbIdAsync(id); //then check db
                if (movie != null)
                {
                    if (movie.LastUpdated < DateTime.Now.AddDays(-1))
                    {
                        
                        var movieNewer = await _externalMovieService.FetchMovieByIdAsync(id);
                        movie.Name = movieNewer.Name;
                        movie.Description = movieNewer.Description;
                        movie.ImageUrl = movieNewer.ImageUrl;
                        movie.Language = movieNewer.Language;
                        movie.VoteCount = movieNewer.VoteCount;
                        movie.VoteAverage = movieNewer.VoteAverage;
                        movie.Popularity = movieNewer.Popularity;
                        movie.ReleaseDate = movieNewer.ReleaseDate;
                        movie.LastUpdated = DateTime.UtcNow;
                        await _repository.UpdateAsync(movie);
                        Console.WriteLine("From DB and API");
                    }
                    else {
                    Console.WriteLine("From DB");
                    }
                    movieDto = _mapper.Map<MovieDto>(movie);
                }
                else 
                {
                    movie = await _externalMovieService.FetchMovieByIdAsync(id); //if not exist in cache&db, fetch
                    
                    Console.WriteLine("From API");

                    Console.WriteLine("movie name: " + movie.Name);
                    await _repository.AddAsync(movie); //save to db
                    movieDto = _mapper.Map<MovieDto>(movie);
                }

                var jsonData = JsonConvert.SerializeObject(movieDto, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // 30 min lifetime
                };

                
                await _cache.SetStringAsync($"Movie Id: {id.ToString()}", jsonData, cacheOptions); //save to cache
            }
            

            return movieDto;
        }
    }
}