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

        private async void LoadMovie(int movieId)
        {
            accountId = await MovieRepository.GetAccountId();

            Movie movieDetails = await MovieRepository.GetMovieDetailsAsync(movieId);
            movie = await MovieRepository.GetMovieAccountDetailsAsync(movieDetails);

            ShowMovie();
            AddEvents();
        }

        private void AddEvents()
        {
            tbiFavorite.Clicked += TbiFavorite_Clicked;
        }

        private async void TbiFavorite_Clicked(object sender, EventArgs e)
        {
            movie.IsFavorite = !movie.IsFavorite;
            await MovieRepository.UpdateMovieAsFavoriteAsync(accountId, movie.Id, movie.IsFavorite);
            await ShowMovie();
        }

        private async Task ShowMovie()
        {
            tbiFavorite.IconImageSource = movie.IsFavorite ? "icon_favorite.png" : "icon_favorite_border.png";
            imgBackdrop.Source = movie.BackdropImage;
            lblTitle.Text = movie.Title;
            lblDescription.Text = movie.Description;
            rating.Value = movie.Rating / (float)2;
            lblReleaseYear.Text = (movie.ReleaseDate == null ? new DateTime(0, 0, 0) : (DateTime)movie.ReleaseDate).Year.ToString();
            
            MovieReview movieReview = await MovieReviewRepository.GetMovieReviewAsync(accountId, movie);
            lblReview.Text = movieReview.Review;
        }
    }
}