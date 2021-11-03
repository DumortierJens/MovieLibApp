using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibApp.Models
{
    interface IMoviePage
    {
        int Page { get; set; }
        int TotalPages { get; set; }
        List<Movie> Movies { get; set; }

        Task GetNextMoviesAsync();
    }
}
