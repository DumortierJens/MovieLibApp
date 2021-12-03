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
        private ToolbarItem btnDelete;
        public MovieReview MovieReview;

        public EditMovieReviewPage(Movie movie)
        {
            InitializeComponent();

            this.movie = movie;
            MovieReview = movie.MovieReview;
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
            btnCancel.Clicked += BtnCancel_Clicked;
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            SaveMovieReview();
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void SaveMovieReview(bool delete = false)
        {
            string oldReview = MovieReview.Review;
            MovieReview.Review = delete ? "" : editor.Text;

            if (string.IsNullOrEmpty(oldReview) && !delete)
                await MovieReviewRepository.AddMovieReviewAsync(movie.Id, MovieReview);

            else if (!string.IsNullOrEmpty(MovieReview.Review) && !delete)
                await MovieReviewRepository.UpdateMovieReviewAsync(movie.Id, MovieReview);

            else
                await MovieReviewRepository.DeleteMovieReviewAsync(movie.Id, MovieReview);

            await Navigation.PopAsync();
        }

        private void ShowReview()
        {
            editor.Text = MovieReview.Review;
            if (!string.IsNullOrEmpty(MovieReview.Review))
            {
                btnDelete = new ToolbarItem()
                {
                    StyleId = "btnDelete",
                    IconImageSource = ImageSource.FromFile("icon_delete.png"),
                };
                this.ToolbarItems.Add(btnDelete);
                btnDelete.Clicked += BtnDelete_Clicked;
            }
        }

        private void BtnDelete_Clicked(object sender, EventArgs e)
        {
            SaveMovieReview(delete: true);
        }
    }
}