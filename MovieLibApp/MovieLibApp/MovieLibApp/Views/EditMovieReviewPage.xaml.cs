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
    public partial class EditMovieReviewPage : ContentPage
    {
        private readonly Movie movie;
        public MovieReview movieReview;

        public EditMovieReviewPage(int accountId, Movie movie)
        {
            InitializeComponent();

            this.movie = movie;
            movieReview = new MovieReview()
            {
                AccountId = accountId,
                MovieId = movie.Id
            };
            LoadMovieReviewEditor();
        }

        private void LoadMovieReviewEditor()
        {
            ShowReview();
            AddEvents();
        }

        private void AddEvents()
        {
            btnSave.Clicked += BtnSave_Clicked;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            movieReview.Review = editor.Text;

            if (string.IsNullOrEmpty(movie.Review))
                await MovieReviewRepository.AddMovieReviewAsync(movieReview);

            else if (!string.IsNullOrEmpty(movieReview.Review))
                await MovieReviewRepository.UpdateMovieReviewAsync(movieReview);

            else
                await MovieReviewRepository.DeleteMovieReviewAsync(movieReview);

            await Navigation.PopModalAsync();
        }

        private void ShowReview()
        {
            editor.Text = movie.Review;
        }
    }
}