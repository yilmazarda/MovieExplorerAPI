using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using movie_explorer.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace movie_explorer.Services
{
    public class GenreService : IGenreService
    {
        public readonly IDistributedCache _cache;
        public readonly IGenreRepository _repository;
        public readonly IExternalGenreService _externalGenreService;

        public GenreService(IGenreRepository repository, IDistributedCache cache, IExternalGenreService externalGenreService) 
        { 
            _repository = repository;
            _cache = cache;
            _externalGenreService = externalGenreService;
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            List<Genre> genresList = new List<Genre>();
            var genres = await _repository.GetAllAsync();
            if (genres.Count() == 0)
            {
                genresList = await _externalGenreService.FetchAllGenresAsync();
                await _repository.AddAllAsync(genresList);
            }
            else
            {
                genresList = genres.ToList();
            }
            return genresList;
        }
    }
}