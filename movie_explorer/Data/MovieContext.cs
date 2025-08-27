using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movie_explorer.Models;
using Microsoft.EntityFrameworkCore;

namespace movie_explorer.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}