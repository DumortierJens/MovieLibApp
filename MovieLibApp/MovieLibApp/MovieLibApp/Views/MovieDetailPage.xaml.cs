using MovieLibApp.Models;
using MovieLibApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieLibApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieDetailPage : ContentPage
    {
        private int accountId;
        private Movie movie;

        public MovieDetailPage(int movieId)
        {
            InitializeComponent();

            LoadMovie(movieId);
        }

        /// <summary>
        /// Load the content and events for the page
        /// </summary>
        /// <param name="movieId">The id of the movie</param>
        private async void LoadMovie(int movieId)
        {
            // Get all the details of the movie
            accountId = await MovieRepository.GetAccountId(); // Get the account id
            Movie movieDetails = await MovieRepository.GetMovieDetailsAsync(movieId); // Get the details of the movie
            movie = await MovieRepository.GetMovieAccountDetailsAsync(movieDetails);  // Update IsFavorite
            movie = await MovieReviewRepository.GetMovieReviewAsync(accountId, movie); // Update the movie review

            ShowMovie();
            AddEvents();
        }

        /// <summary>
        /// Show all the details of the movie
        /// </summary>
        private void ShowMovie()
        {
            tbiFavorite.IconImageSource = movie.IsFavorite ? "icon_favorite.png" : "icon_favorite_border.png";
            imgBackdrop.Source = movie.BackdropImage;
            lblTitle.Text = movie.Title;
            lblDescription.Text = movie.Description;
            rating.Value = movie.Rating / (float)2;
            lblReleaseYear.Text = (movie.ReleaseDate == null ? new DateTime(0, 0, 0) : (DateTime)movie.ReleaseDate).Year.ToString();
            btnEditReview.Source = string.IsNullOrEmpty(movie.MovieReview.Review) ? "icon_add" : "icon_edit";
            lblReview.Text = string.IsNullOrEmpty(movie.MovieReview.Review) ? "Write your review" : movie.MovieReview.Review;
        }

        /// <summary>
        /// Add the events for the page
        /// </summary>
        private void AddEvents()
        {
            tbiFavorite.Clicked += TbiFavorite_Clicked;
            btnEditReview.Clicked += BtnEditReview_Clicked;
        }

        /// <summary>
        /// Update IsFavorite if the favorite toolbaritem is clicked
        /// </summary>
        private async void TbiFavorite_Clicked(object sender, EventArgs e)
        {
            movie.IsFavorite = !movie.IsFavorite;
            await MovieRepository.UpdateMovieAsFavoriteAsync(accountId, movie.Id, movie.IsFavorite);
            ShowMovie();
        }

        /// <summary>
        /// Go to the edit movie review page if the edit review button is clicked
        /// </summary>
        private void BtnEditReview_Clicked(object sender, EventArgs e)
        {
            var page = new EditMovieReviewPage(movie);
            Navigation.PushAsync(page);
            page.Disappearing += Page_Disappearing;
        }

        /// <summary>
        /// Edit the current movie review if the edit movie review page is disappearing
        /// </summary>
        private void Page_Disappearing(object sender, EventArgs e)
        {
            var page = sender as EditMovieReviewPage;
            movie.MovieReview = page.MovieReview;
            ShowMovie();
        }
    }
}