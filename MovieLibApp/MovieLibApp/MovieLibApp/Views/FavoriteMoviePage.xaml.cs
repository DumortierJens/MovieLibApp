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
        private int accountId;
        private readonly ObservableCollection<Movie> movies = new ObservableCollection<Movie>();

        public FavoriteMoviePage()
        {
            InitializeComponent();

            LoadPage();
        }

        private async void LoadPage()
        {
            accountId = await MovieRepository.GetAccountId();

            await LoadMovies();

            cvwMyFavoriteMovies.ItemsSource = movies;
            cvwMyFavoriteMovies.EmptyView = "No movies found";

            AddEvents();
        }

        private async Task LoadMovies()
        {
            movies.Clear();
            IMoviePage moviePage = await MovieRepository.GetFavoriteMoviesAsync(accountId);
            foreach (var movie in moviePage.Movies)
                movies.Add(movie);
        }

        private void AddEvents()
        {
            cvwMyFavoriteMovies.SelectionChanged += CvwMyFavoriteMovies_SelectionChanged;
            this.Appearing += FavoriteMoviePage_Appearing;
        }

        private async void FavoriteMoviePage_Appearing(object sender, EventArgs e)
        {
            await LoadMovies();
        }

        private void CvwMyFavoriteMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
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