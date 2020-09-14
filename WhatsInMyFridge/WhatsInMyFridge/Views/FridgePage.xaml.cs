using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Controls;
using WhatsInMyFridge.Models;
using WhatsInMyFridge.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FridgePage : ContentPage
    {
        FridgeViewModel viewModel = new FridgeViewModel();

        public FridgePage()
        {
            InitializeComponent();

            BindingContext = viewModel;

            viewModel.foodList.Add(new Food()
            {
                Amount = 2,
                bestBeforeDate = new ObservableCollection<BestBeforeDate>() { new BestBeforeDate(DateTime.Now.AddDays(10)), new BestBeforeDate(DateTime.Now.AddDays(-1)) },
                Name = "Milch",
                main_img = new UriImageSource() { Uri = new Uri("https://frogcoffee.de/media/image/83/ab/c2/x1034352938avFptQAoOWw0Bi_600x600.png.pagespeed.ic.dBXAjKUA1N.png") },
            });

            viewModel.foodList.Add(new Food()
            {
                Amount = 1,
                bestBeforeDate = new ObservableCollection<BestBeforeDate>() { new BestBeforeDate(DateTime.Now.AddDays(5)) },
                Name = "Tomate",
                main_img = new UriImageSource() { Uri = new Uri("https://www.boeschbodenspies.com/wp-content/uploads/2017/08/tomato.png") },
            });

            viewModel.FilteredFoodList = new ObservableCollection<Food>(viewModel.foodList);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender is FridgeGrid grid)
            {
                Navigation.PushAsync(new FoodDetailPage(grid.FoodItem));
            }
        }
    }
}