using MovieLibApp.Repositories;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieLibApp
{
    public partial class App : Application
    {
        public App()
        {
            Application.Current.UserAppTheme = OSAppTheme.Light;

            InitializeComponent();

            // Get licensing key for Syncfusion rating plugin (Community license)
            string SyncfusionLicensingKey = UserSecretsRepository.Settings["SyncfusionLicensing"];
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(SyncfusionLicensingKey);

            MainPage = new AppShell();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                Shell.Current.GoToAsync($"noNetwork").Wait();
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
