using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace movie_explorer.Models
{
    public class ApiMovieResponse
    {
        [JsonPropertyName("results")]
        public List<ApiMovie> Results { get; set; } = new();
    }
}