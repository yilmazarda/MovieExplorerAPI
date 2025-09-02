using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace movie_explorer.Models
{
    public class ApiMovieDetailsResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Name { get; set; }

        [JsonProperty("overview")]
        public string Description { get; set; }

        [JsonProperty("poster_path")]
        public string ImageUrl { get; set; }

        [JsonProperty("original_language")]
        public string Language { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }

        [JsonProperty("popularity")]
        public float Popularity { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("genres")]
        public List<ApiGenre> Genres { get; set; }
    }
}