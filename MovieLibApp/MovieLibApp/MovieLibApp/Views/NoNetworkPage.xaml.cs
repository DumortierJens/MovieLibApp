using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MovieLibApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoNetworkPage : ContentPage
    {
        public NoNetworkPage()
        {
            InitializeComponent();

            // Add event to check if the internet connection is changed
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        /// <summary>
        /// If the internet connection changed and there is internet, go to the main page
        /// </summary>
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                App.Current.MainPage = new AppShell();
            }
        }

        /// <summary>
        /// Override the backbutton to prevent going back
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}