using MovieLibApp.Models;
using MovieLibApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieLibApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoriteMoviePage : ContentPage
    {
        private readonly ObservableCollection<Movie> movies = new ObservableCollection<Movie>();
        private IMoviePage currentMoviePage;

        public FavoriteMoviePage()
        {
            InitializeComponent();

            LoadPage();
        }

        /// <summary>
        /// Load the content and events for the page
        /// </summary>
        private async void LoadPage()
        {
            // Load the movies
            await LoadMovies();

            // Add the movies to the collectionview
            cvwMovies.ItemsSource = movies;
            cvwMovies.EmptyView = "No movies found";

            // Add events to the page
            AddEvents();
        }

        /// <summary>
        /// Load the favorite movies and add them to the movie collection
        /// </summary>
        private async Task LoadMovies()
        {
            // Get the favorite movies
            int accountId = await MovieRepository.GetAccountId();
            currentMoviePage = await MovieRepository.GetFavoriteMoviesAsync(accountId);

            // Clear the collectionview and add the favorite movies
            movies.Clear();
            AddMovies(currentMoviePage.Movies);
        }

        /// <summary>
        /// Add each movie to the movie collection
        /// </summary>
        /// <param name="newMovies">List of movies</param>
        private void AddMovies(List<Movie> newMovies)
        {
            foreach (var movie in newMovies)
                movies.Add(movie);
        }

        /// <summary>
        /// Add the events for the page
        /// </summary>
        private void AddEvents()
        {
            cvwMovies.SelectionChanged += CvwMovies_SelectionChanged;
            this.Appearing += FavoriteMoviePage_Appearing;
            
            cvwMovies.RemainingItemsThreshold = 5; 
            cvwMovies.RemainingItemsThresholdReached += CvwMovies_RemainingItemsThresholdReached; ;
        }

        /// <summary>
        /// Go to the seleted movie detail page
        /// </summary>
        private void CvwMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Movie movie = (Movie)cvwMovies.SelectedItem;

            if (movie != null)
            {
                Navigation.PushAsync(new MovieDetailPage(movie.Id));
                cvwMovies.SelectedItem = null; // Deselect the selected movie
            }
        }

        /// <summary>
        /// Reload the favorite movies when page is appearing
        /// </summary>
        private async void FavoriteMoviePage_Appearing(object sender, EventArgs e)
        {
            await LoadMovies();
        }


        /// <summary>
        /// Get next page of movies and add them to the movie collection
        /// </summary>
        private async void CvwMovies_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            await currentMoviePage.GetNextMoviesAsync();
            AddMovies(currentMoviePage.Movies);
        }
    }
}