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
                Name = "Milch"
            });

            viewModel.foodList.Add(new Food()
            {
                Amount = 1,
                bestBeforeDate = new ObservableCollection<BestBeforeDate>() { new BestBeforeDate(DateTime.Now.AddDays(5)) },
                Name = "Ketchup"
            });
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