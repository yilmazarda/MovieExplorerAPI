using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;

namespace movie_explorer.Services
{
    public interface IGenreService
    {
        Task<List<Genre>> GetAllGenresAsync();
    }
}