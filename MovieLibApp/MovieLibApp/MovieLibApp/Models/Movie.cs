using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MovieLibApp.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [JsonProperty("overview")]
        public string Description { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonProperty("vote_average")]
        public double Rating { get; set; }

        [JsonProperty("poster_path")]
        public string PosterImagePath { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropImagePath { get; set; }

        public UriImageSource PosterImage => (UriImageSource)ImageSource.FromUri(new Uri($"https://image.tmdb.org/t/p/w500{PosterImagePath}"));
        public UriImageSource BackdropImage => (UriImageSource)ImageSource.FromUri(new Uri($"https://image.tmdb.org/t/p/w500{BackdropImagePath}"));
    }
}
