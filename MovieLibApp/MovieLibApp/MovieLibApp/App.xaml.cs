using MovieLibApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieLibApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            //MainPage = new NavigationPage(new MovieOverviewPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
