using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace movie_explorer.Models
{
    public class ApiGenreResponse
    {
        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; } = new();
    }
}