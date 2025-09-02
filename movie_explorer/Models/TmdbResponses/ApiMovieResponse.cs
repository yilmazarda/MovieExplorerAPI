using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using movie_explorer.Models;


namespace movie_explorer.Models
{
    public class ApiMovieResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Name { get; set; }

        [JsonPropertyName("overview")]
        public string Description { get; set; }

        [JsonPropertyName("poster_path")]
        public string ImageUrl { get; set; }

        [JsonPropertyName("original_language")]
        public string Language { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }

        [JsonPropertyName("vote_average")]
        public float VoteAverage { get; set; }

        [JsonPropertyName("popularity")]
        public float Popularity { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }

        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; } = new List<int>(); 
    }
}

