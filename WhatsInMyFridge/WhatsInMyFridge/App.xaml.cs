using System;
using WhatsInMyFridge.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Device.SetFlags(new string[] { "Shapes_Experimental", "SwipeView_Experimental" });

            MainPage = new NavigationPage(new MenuPage());
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
