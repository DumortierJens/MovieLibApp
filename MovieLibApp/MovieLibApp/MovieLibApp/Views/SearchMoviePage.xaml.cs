using MovieLibApp.Models;
using MovieLibApp.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieLibApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchMoviePage : ContentPage
    {
        private readonly ObservableCollection<Movie> movies = new ObservableCollection<Movie>();
        private CancellationTokenSource searchCts = new CancellationTokenSource();
        private IMoviePage currentMoviePage;

        public SearchMoviePage()
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
        /// Load the popular/searched movies and add them to the movie collection
        /// </summary>
        /// <param name="searchQuery">Query for searching a movie</param>
        private async Task LoadMovies(string searchQuery = "")
        {
            // Popular movies
            if (string.IsNullOrEmpty(searchQuery))
            {
                currentMoviePage = await MovieRepository.GetPopularMoviesAsync();
                lblSubtitle.Text = "Popular Search";
            }

            // Search movies
            else
            {
                currentMoviePage = await MovieRepository.SearchMovieAsync(searchQuery);
                lblSubtitle.Text = "Search Results";
            }

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
            searchMovie.TextChanged += SearchMovie_TextChanged;
            cvwMovies.SelectionChanged += CvwMovies_SelectionChanged;
            
            cvwMovies.RemainingItemsThreshold = 5;
            cvwMovies.RemainingItemsThresholdReached += CvwMovies_RemainingItemsThresholdReached;
        }

        /// <summary>
        /// Search the movie if the searchbar text changed
        /// and cancel the previous search task if timespan < 500ms
        /// </summary>
        private void SearchMovie_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchQuery = searchMovie.Text;

            Interlocked.Exchange(ref searchCts, new CancellationTokenSource()).Cancel(); // Cancel the previous search task if not already executed
            Task.Delay(TimeSpan.FromMilliseconds(500), searchCts.Token) // If no keystroke occurs, carry on after 500ms
                    .ContinueWith(
                        delegate { _ = LoadMovies(searchQuery); }, // Search the movie 
                        CancellationToken.None,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.FromCurrentSynchronizationContext());
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
                cvwMovies.SelectedItem = null;
            }
        }

        /// <summary>
        /// Load next movie page if remaining items threshhold is reached
        /// </summary>
        private async void CvwMovies_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            await currentMoviePage.GetNextMoviesAsync();
            AddMovies(currentMoviePage.Movies);
        }
    }
}