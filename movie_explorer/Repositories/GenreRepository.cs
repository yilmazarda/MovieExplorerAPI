using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movie_explorer.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieContext options) : base(options) { }

    }
}