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
            Debug.WriteLine("Popular Movies");
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
            Debug.WriteLine("Search Movie: Furious 7");
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

            // Favorite movies
            Debug.WriteLine("Favorite Movies");
            int accountId = await MovieRepository.GetAccountId();
            MoviePage favoriteMovies = await MovieRepository.GetFavoriteMoviesAsync(accountId);
            foreach (var item in favoriteMovies.Movies)
            {
                Debug.WriteLine(item.Title);
            }

            Debug.WriteLine("Add Favorite Movie");
            await MovieRepository.UpdateMovieAsFavoriteAsync(accountId, searchMovieResult.Movies[0].Id, true);
            MoviePage favoriteMovies1 = await MovieRepository.GetFavoriteMoviesAsync(accountId);
            foreach (var item in favoriteMovies1.Movies)
            {
                Debug.WriteLine(item.Title);
            }

            Debug.WriteLine("Remove Favorite Movie");
            await MovieRepository.UpdateMovieAsFavoriteAsync(accountId, searchMovieResult.Movies[0].Id, false);
            MoviePage favoriteMovies2 = await MovieRepository.GetFavoriteMoviesAsync(accountId);
            foreach (var item in favoriteMovies2.Movies)
            {
                Debug.WriteLine(item.Title);
            }
        }
    }
}
