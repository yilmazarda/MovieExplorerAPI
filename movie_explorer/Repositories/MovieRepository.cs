using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using Microsoft.EntityFrameworkCore;
using movie_explorer.Data;

namespace movie_explorer.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieContext context) : base(context)
        {}

        public async Task<Movie?> GetByTmdbIdAsync(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.TmdbId == id);
        }
    }
}