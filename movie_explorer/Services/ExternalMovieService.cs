using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using movie_explorer.Models;
using System.Text.Json;

namespace movie_explorer.Services
{
    public class ExternalMovieService : IExternalMovieService
    {
        public readonly HttpClient _httpClient;
        public readonly string _apiKey;
        public readonly string _baseUrl;

        public ExternalMovieService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TMDB:ApiKey"] ?? string.Empty;
            _baseUrl = configuration["MovieApi:BaseUrl"]; 
        }

        public async Task<List<Movie>> FetchPopularMoviesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiMovieResponse>($"{_baseUrl}/movie/popular?language=en-US&page=1&api_key={_apiKey}");

            var moviesToSave = response.Results.Select(m => new Movie
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
            }).ToList();

            return moviesToSave;
        }

        public async Task<List<Movie>> FetchTrendingMoviesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiMovieResponse>($"{_baseUrl}/trending/movie/week?api_key={_apiKey}");

            var moviesToSave = response.Results.Select(m => new Movie
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
            }).ToList();

            return moviesToSave;
        }

        
        public async Task<Movie> FetchMovieByIdAsync(int id)
        {
           var response = await _httpClient.GetFromJsonAsync<ApiMovie>($"{_baseUrl}/movie/{id}?api_key={_apiKey}");

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
           };

           return movieToSave;
        }
    }
}