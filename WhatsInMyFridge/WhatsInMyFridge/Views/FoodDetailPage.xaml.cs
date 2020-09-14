using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Models;
using WhatsInMyFridge.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FoodDetailPage : ContentPage
    {
        FoodDetailViewModel viewModel = new FoodDetailViewModel();

        public FoodDetailPage(Food food)
        {
            InitializeComponent();

            viewModel.item = food;

            Title = food.Name;

            BindingContext = viewModel;
        }
    }
}