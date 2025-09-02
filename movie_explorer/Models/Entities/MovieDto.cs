using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;


namespace movie_explorer.Models
{
    public class MovieDto
    {
        public string Name { get; set;}
        public string Description { get; set;}
        public string ImageUrl { get; set;}
        public string Language { get; set;}
        public int VoteCount { get; set;}
        public float VoteAverage { get; set;}
        public float Popularity { get; set;}
        public DateTime ReleaseDate { get; set;}
        public List<String> Genres { get; set; } = new List<String>();
    }
}