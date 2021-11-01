using MovieLibApp.Models;
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
        public Movie CurrentMovie;

        public MovieDetailPage(Movie movie)
        {
            InitializeComponent();

            CurrentMovie = movie;
            ShowMovie();
        }

        private void ShowMovie()
        {
            imgBackdrop.Source = CurrentMovie.BackdropImage;
            lblTitle.Text = CurrentMovie.Title;
            lblDescription.Text = CurrentMovie.Description;
            lblRating.Text = $"{CurrentMovie.Rating} / 10";
            lblReleaseYear.Text = (CurrentMovie.ReleaseDate == null ? new DateTime(0, 0, 0) : (DateTime)CurrentMovie.ReleaseDate).Year.ToString();
        }
    }
}