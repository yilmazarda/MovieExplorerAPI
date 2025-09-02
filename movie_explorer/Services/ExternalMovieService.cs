using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using movie_explorer.Models;
using movie_explorer.Repositories;
using Newtonsoft.Json;

namespace movie_explorer.Services
{
    public class ExternalMovieService : IExternalMovieService
    {
        public readonly HttpClient _httpClient;
        public readonly string _apiKey;
        public readonly string _baseUrl;
        public readonly IGenreRepository _genreRepository;

        public ExternalMovieService(HttpClient httpClient, IConfiguration configuration, IGenreRepository genreRepository)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TMDB:ApiKey"] ?? string.Empty;
            _baseUrl = configuration["MovieApi:BaseUrl"]; 
            _genreRepository = genreRepository;
        }

        public async Task<List<Movie>> FetchPopularMoviesAsync(int page = 1)
        {
            var response = await _httpClient.GetFromJsonAsync<ApiMovieListResponse>($"{_baseUrl}/movie/popular?language=en-US&page={page}&api_key={_apiKey}");
            var movie_list = response.Results;
            var moviesToSave = new List<Movie>();

            foreach (var m in movie_list)
            {
                var movie = new Movie
                {
                    TmdbId = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    ImageUrl = m.ImageUrl,
                    Language = m.Language,
                    VoteCount = m.VoteCount,
                    VoteAverage = m.VoteAverage,
                    Popularity = m.Popularity,
                    ReleaseDate = DateTime.TryParse(m.ReleaseDate, out var date) ? date : DateTime.MinValue,
                    Genres = new List<Genre>()
                };

                foreach (var genre_id in m.GenreIds)
                {
                    var genre = await _genreRepository.GetByIdAsync(genre_id);
                    if (!movie.Genres.Any(g => g.TmdbId == genre.TmdbId))
                    {
                        movie.Genres.Add(genre);
                    }
                }
                moviesToSave.Add(movie);
            }

            return moviesToSave;
        }

        public async Task<List<Movie>> FetchTrendingMoviesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiMovieListResponse>($"{_baseUrl}/trending/movie/week?api_key={_apiKey}");
            var movie_list = response.Results;
            var moviesToSave = new List<Movie>();

            foreach (var m in movie_list)
            {
                var movie = new Movie
                {
                    TmdbId = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    ImageUrl = m.ImageUrl,
                    Language = m.Language,
                    VoteCount = m.VoteCount,
                    VoteAverage = m.VoteAverage,
                    Popularity = m.Popularity,
                    ReleaseDate = DateTime.TryParse(m.ReleaseDate, out var date) ? date : DateTime.MinValue,
                    Genres = new List<Genre>()
                };

                foreach (var genre_id in m.GenreIds)
                {
                    var genre = await _genreRepository.GetByIdAsync(genre_id);
                    if (!movie.Genres.Any(g => g.TmdbId == genre.TmdbId))
                    {
                        movie.Genres.Add(genre);
                    }
                }
                moviesToSave.Add(movie);
            }

            return moviesToSave;
        }

        
        public async Task<Movie> FetchMovieByIdAsync(int id)
        {
            var json = await _httpClient.GetStringAsync($"{_baseUrl}/movie/{id}?api_key={_apiKey}");
            var response = JsonConvert.DeserializeObject<ApiMovieDetailsResponse>(json);
       
            var movieToSave = new Movie{
                TmdbId = response.Id,
                Name = response.Name,
                Description = response.Description,
                ImageUrl = response.ImageUrl,
                Language = response.Language,
                VoteCount = response.VoteCount,
                VoteAverage = response.VoteAverage,
                Popularity = response.Popularity,
                ReleaseDate = DateTime.TryParse(response.ReleaseDate, out var date) ? date : DateTime.MinValue,
                Genres = new List<Genre>()
            };

            var genres = response.Genres;

            foreach(var genre in genres)
            {
                var genreToSave = await _genreRepository.GetByIdAsync(genre.Id);
                if (!movieToSave.Genres.Any(g => g.TmdbId == genreToSave.TmdbId))
                {   
                    movieToSave.Genres.Add(genreToSave);
                }
            }



            return movieToSave;
        }
    }
}