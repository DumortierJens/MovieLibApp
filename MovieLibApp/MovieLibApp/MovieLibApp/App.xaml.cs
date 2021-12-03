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
            InitializeComponent();

            // Get licensing key for Syncfusion rating plugin (Community license)
            string SyncfusionLicensingKey = UserSecretsRepository.Settings["SyncfusionLicensing"];
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(SyncfusionLicensingKey);

            // Set the mainpage
            MainPage = new AppShell();

            // Add event to check if the internet connection is changed
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            // If there is no internet, go to the no network page
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                Shell.Current.GoToAsync($"noNetwork").Wait();
        }

        /// <summary>
        /// If the internet connection changed and there is no internet, go to the no network page
        /// </summary>
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
