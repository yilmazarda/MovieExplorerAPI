using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace movie_explorer.Services
{
    public class ExternalGenreService : IExternalGenreService
    {
        public readonly HttpClient _httpClient;
        public readonly string _apiKey;
        public readonly string _baseUrl;
        public readonly ILogger<ExternalGenreService> _logger;

        public ExternalGenreService(HttpClient httpClient, IConfiguration configuration, ILogger<ExternalGenreService> logger)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TMDB:ApiKey"] ?? string.Empty;
            _baseUrl = configuration["MovieApi:BaseUrl"]; 
            _logger = logger;
        }

        public async Task<List<Genre>> FetchAllGenresAsync()
        {
            _logger.LogInformation("Api call to fetch genres...");
            
            try
            {
                var responseString = await _httpClient.GetStringAsync($"{_baseUrl}/genre/movie/list?api_key={_apiKey}");
                var response = JsonConvert.DeserializeObject<ApiGenreResponse>(responseString);

                var genresToSave = response.Genres.Select(g => new Genre
                {
                    TmdbId = g.TmdbId,
                    Name = g.Name,
                }).ToList();

                return genresToSave;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occured while fetching genres from API");
                return null;
            }
        }
    }
}