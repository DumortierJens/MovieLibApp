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

        public SearchMoviePage()
        {
            InitializeComponent();

            LoadPage();
        }

        private async void LoadPage()
        {
            await LoadMovies();

            cvwMovies.ItemsSource = movies;
            cvwMovies.EmptyView = "No movies found";

            AddEvents();
        }

        private async Task LoadMovies(string query = "")
        {
            IMoviePage currentMoviePage;
            if (string.IsNullOrEmpty(query))
            {
                currentMoviePage = await MovieRepository.GetPopularMoviesAsync();
                lblSubtitle.Text = "Popular Search";
            }
            else
            {
                currentMoviePage = await MovieRepository.SearchMovieAsync(query);
                lblSubtitle.Text = "Search Results";
            }

            movies.Clear();
            foreach (var movie in currentMoviePage.Movies)
                movies.Add(movie);
        }

        private void AddEvents()
        {
            searchMovie.TextChanged += SearchMovie_TextChanged;
            cvwMovies.SelectionChanged += CvwMovies_SelectionChanged;
        }

        private void CvwMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Movie movie = (Movie)(sender as CollectionView).SelectedItem;

            if (movie != null)
            {
                Navigation.PushAsync(new MovieDetailPage(movie.Id));
                (sender as CollectionView).SelectedItem = null;
            }
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

        private async void SearchMovie(string query)
        {
            if (!string.IsNullOrEmpty(query))
                await LoadMovies(query);
            else
                await LoadMovies(query);
        }
    }
}