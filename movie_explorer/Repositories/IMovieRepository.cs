using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;

namespace movie_explorer.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        public Task<Movie?> GetByTmdbIdAsync(int id);
    }
}