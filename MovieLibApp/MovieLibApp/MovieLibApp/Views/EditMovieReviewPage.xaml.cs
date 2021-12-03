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
        private ToolbarItem tbiDelete;
        public MovieReview MovieReview;

        public EditMovieReviewPage(Movie movie)
        {
            InitializeComponent();

            this.movie = movie;
            this.MovieReview = movie.MovieReview;

            LoadPage();
        }

        /// <summary>
        /// Load the content and events for the page
        /// </summary>
        private void LoadPage()
        {
            ShowReview();
            AddEvents();
        }

        /// <summary>
        /// Show the content for the page
        /// </summary>
        private void ShowReview()
        {
            // Set the previous review in the text editor
            editor.Text = MovieReview.Review;

            // Add a delete button to the toolbar if there wa already a review
            if (!string.IsNullOrEmpty(MovieReview.Review))
            {
                // Create a delete button in the toolbar
                tbiDelete = new ToolbarItem()
                {
                    StyleId = "btnDelete",
                    IconImageSource = ImageSource.FromFile("icon_delete.png"),
                };
                this.ToolbarItems.Add(tbiDelete);
                tbiDelete.Clicked += BtnDelete_Clicked;
            }
        }

        /// <summary>
        /// Add the events for the page
        /// </summary>
        private void AddEvents()
        {
            btnSave.Clicked += BtnSave_Clicked;
            btnCancel.Clicked += BtnCancel_Clicked;
        }


        /// <summary>
        /// Save the movie when the save button is clicked
        /// </summary>
        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            SaveMovieReview();
        }

        /// <summary>
        /// Go to the previous page when the cancel button is clicked
        /// </summary>
        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        /// <summary>
        /// Delete the movie when the delete toolbaritem is clicked
        /// </summary>
        private void BtnDelete_Clicked(object sender, EventArgs e)
        {
            SaveMovieReview(delete: true); // Delete the review
        }

        /// <summary>
        /// Save the movie review
        /// </summary>
        /// <param name="delete">If true, delete the movie review</param>
        private async void SaveMovieReview(bool delete = false)
        {
            string oldReview = MovieReview.Review;
            MovieReview.Review = delete ? "" : editor.Text;

            // Add review
            if (string.IsNullOrEmpty(oldReview) && !delete)
                await MovieReviewRepository.AddMovieReviewAsync(movie.Id, MovieReview);

            // Update review
            else if (!string.IsNullOrEmpty(MovieReview.Review) && !delete)
                await MovieReviewRepository.UpdateMovieReviewAsync(movie.Id, MovieReview);

            // Delete review
            else
                await MovieReviewRepository.DeleteMovieReviewAsync(movie.Id, MovieReview);

            // Go to the previous page
            await Navigation.PopAsync();
        }
    }
}