using MovieLibApp.Models;
using MovieLibApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MovieLibApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            TestMovieRepository();
        }

        private async void TestMovieRepository()
        {
            // Popular movies
            MoviePage popularMovieResult = await MovieRepository.GetPopularMoviesAsync();
            foreach (var item in popularMovieResult.Movies)
            {
                Debug.WriteLine(item.Title);
            }

            MoviePage popularMovieResult2 = await MovieRepository.GetPopularMoviesAsync(2);
            foreach (var item in popularMovieResult2.Movies)
            {
                Debug.WriteLine(item.Title);
            }

            // Search movie
            string searchQuery = "Furious 7";
            MoviePage searchMovieResult = await MovieRepository.SearchMovieAsync(searchQuery);
            foreach (var item in searchMovieResult.Movies)
            {
                Debug.WriteLine(item.Title);
            }

            MoviePage searchMovieResult2 = await MovieRepository.SearchMovieAsync(searchMovieResult.Query, 2);
            foreach (var item in searchMovieResult2.Movies)
            {
                Debug.WriteLine(item.Title);
            }
        }
    }
}
