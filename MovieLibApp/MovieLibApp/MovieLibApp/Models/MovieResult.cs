using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibApp.Models
{
    public class MovieResult
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public List<Movie> Movies { get; set; }

        public string Query { get; set; }

    }
}
