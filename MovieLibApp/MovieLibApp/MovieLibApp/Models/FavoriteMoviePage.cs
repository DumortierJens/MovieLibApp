using MovieLibApp.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibApp.Models
{
    public class FavoriteMoviePage : IMoviePage
    {
        public int Page { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public List<Movie> Movies { get; set; }

        public int AccountId { get; set; }

        public async Task GetNextMoviesAsync()
        {
            if (Page < TotalPages)
            {
                Page++;
                IMoviePage newMoviePage = await MovieRepository.GetFavoriteMoviesAsync(AccountId, Page++);
                Movies = newMoviePage.Movies;
            }
            else
            {
                Movies = new List<Movie>();
            }
        }
    }
}
