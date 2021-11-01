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

            LoadMovies();
        }

        private async void LoadMovies()
        {
            accountId = await MovieRepository.GetAccountId();

            MoviePage moviePage = await MovieRepository.GetFavoriteMoviesAsync(accountId);
            foreach (var movie in moviePage.Movies)
                movies.Add(movie);

            cvwMyFavoriteMovies.ItemsSource = movies;
            cvwMyFavoriteMovies.EmptyView = "No movies found";

            AddEvents();
        }

        private void AddEvents()
        {
            cvwMyFavoriteMovies.SelectionChanged += CvwMyFavoriteMovies_SelectionChanged;
        }

        private void CvwMyFavoriteMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Movie selectedMovie = (Movie)(sender as CollectionView).SelectedItem;

            if (selectedMovie != null)
            {
                Navigation.PushAsync(new MovieDetailPage(selectedMovie));
                (sender as CollectionView).SelectedItem = null;
            }
        }
    }
}