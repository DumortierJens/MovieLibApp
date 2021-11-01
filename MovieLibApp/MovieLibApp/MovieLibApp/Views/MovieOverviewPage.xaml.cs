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
    public partial class MovieOverviewPage : ContentPage
    {
        private readonly ObservableCollection<Movie> movies = new ObservableCollection<Movie>();
        private CancellationTokenSource searchCts = new CancellationTokenSource();

        public MovieOverviewPage()
        {
            InitializeComponent();

            GetMovies();
            cvwMovies.ItemsSource = movies;

            // Events
            searchMovie.TextChanged += SearchMovie_TextChanged;
            cvwMovies.SelectionChanged += CvwMovies_SelectionChanged;
        }

        private void CvwMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Movie selectedMovie = (Movie) (sender as CollectionView).SelectedItem;

            if (selectedMovie != null)
                Navigation.PushAsync(new MovieDetailPage(selectedMovie));
        }

        private void SearchMovie_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = searchMovie.Text;

            Interlocked.Exchange(ref searchCts, new CancellationTokenSource()).Cancel(); // cancel the previous search task if not already executed
            Task.Delay(TimeSpan.FromMilliseconds(500), searchCts.Token) // if no keystroke occurs, carry on after 500ms
                    .ContinueWith(
                        delegate { SearchMovie(query); }, // Pass the changed text to the SearchMovie function
                        CancellationToken.None,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SearchMovie(string query)
        {
            if (!string.IsNullOrEmpty(query))
                GetMovies(query);
            else
                GetMovies();
        }

        private async void GetMovies(string query = "")
        {
            MoviePage currentMoviePage;

            if (string.IsNullOrEmpty(query))
                currentMoviePage = await MovieRepository.GetPopularMoviesAsync();
            else
                currentMoviePage = await MovieRepository.SearchMovieAsync(query);

            movies.Clear();
            foreach (var movie in currentMoviePage.Movies)
                movies.Add(movie);
        }
    }
}