using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movie_explorer.Models
{
    public class Genre
    {
        public int Id { get; set;}
        public string Name { get; set;}

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}