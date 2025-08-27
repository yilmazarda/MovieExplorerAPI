using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using movie_explorer.Data;

namespace movie_explorer.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieContext context) : base(context)
        {}
    }
}