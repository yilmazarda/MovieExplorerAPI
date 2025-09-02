using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace movie_explorer.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(1, int.MaxValue, ErrorMessage = "TmdbId must be greater than 0")]
        public int TmdbId { get; set;}
        
        [Required]
        public string Name { get; set;}

        [Required]
        public string Description { get; set;}

        [Required]
        public string ImageUrl { get; set;}

        [Required]
        public string Language { get; set;}

        [Required]
        public int VoteCount { get; set;}

        [Required]
        public float VoteAverage { get; set;}

        [Required]
        public float Popularity { get; set;}

        [Required]
        public DateTime ReleaseDate { get; set;}

        [Required]
        public DateTime LastUpdated { get; set;}

        [JsonIgnore]
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    }
}