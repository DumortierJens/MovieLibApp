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

        private async void LoadPage()
        {
            await LoadMovies();

            cvwMovies.ItemsSource = movies;
            cvwMovies.EmptyView = "No movies found";

            AddEvents();
        }

        private async Task LoadMovies()
        {
            int accountId = await MovieRepository.GetAccountId();
            currentMoviePage = await MovieRepository.GetFavoriteMoviesAsync(accountId);

            movies.Clear();
            AddMovies(currentMoviePage.Movies);
        }

        private void AddMovies(List<Movie> newMovies)
        {
            foreach (var movie in newMovies)
                movies.Add(movie);
        }

        private void AddEvents()
        {
            cvwMovies.SelectionChanged += CvwMovies_SelectionChanged;
            this.Appearing += FavoriteMoviePage_Appearing;

            cvwMovies.RemainingItemsThreshold = 5;
            cvwMovies.RemainingItemsThresholdReached += CvwMovies_RemainingItemsThresholdReached; ;
        }

        private async void CvwMovies_RemainingItemsThresholdReached(object sender, EventArgs e)
        {
            await currentMoviePage.GetNextMoviesAsync();
            AddMovies(currentMoviePage.Movies);
        }

        private async void FavoriteMoviePage_Appearing(object sender, EventArgs e)
        {
            await LoadMovies();
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
    }
}