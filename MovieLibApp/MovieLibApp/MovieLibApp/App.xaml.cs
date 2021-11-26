using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieLibApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            MainPage = new AppShell();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Shell.Current.GoToAsync($"noNetwork").Wait();
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Shell.Current.GoToAsync($"noNetwork");
            }
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
