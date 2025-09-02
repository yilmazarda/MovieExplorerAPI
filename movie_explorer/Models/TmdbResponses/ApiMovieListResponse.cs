using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace movie_explorer.Models
{
    public class ApiMovieListResponse
    {
        [JsonProperty("results")]
        public List<ApiMovieResponse> Results { get; set; } = new();
    }
}