using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movie_explorer.Models
{
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TmdbId { get; set;}
        public string Name { get; set;}

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}