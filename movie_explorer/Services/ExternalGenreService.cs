using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using Microsoft.Extensions.Configuration;

namespace movie_explorer.Services
{
    public class ExternalGenreService : IExternalGenreService
    {
        public readonly HttpClient _httpClient;
        public readonly string _apiKey;
        public readonly string _baseUrl;

        public ExternalGenreService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TMDB:ApiKey"] ?? string.Empty;
            _baseUrl = configuration["MovieApi:BaseUrl"]; 
        }

        public async Task<List<Genre>> FetchAllGenresAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ApiGenreResponse>($"{_baseUrl}/genre/movie/list?api_key={_apiKey}");
            
            var genresToSave = response.Genres.Select(g => new Genre
            {
                TmdbId = g.TmdbId,
                Name = g.Name,
            }).ToList();

            return genresToSave;
        }
    }
}