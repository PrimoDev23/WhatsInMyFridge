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
    public partial class RecipeOverview : ContentPage
    {
        RecipeOverviewModel viewModel = new RecipeOverviewModel();

        public RecipeOverview()
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

            viewModel.foodList.Add(new Food()
            {
                Amount = 1,
                bestBeforeDate = new ObservableCollection<BestBeforeDate>() { new BestBeforeDate(DateTime.Now.AddDays(5)) },
                Name = "Tomate",
                main_img = new UriImageSource() { Uri = new Uri("https://www.boeschbodenspies.com/wp-content/uploads/2017/08/tomato.png") },
            });

            viewModel.foodList.Add(new Food()
            {
                Amount = 1,
                bestBeforeDate = new ObservableCollection<BestBeforeDate>() { new BestBeforeDate(DateTime.Now.AddDays(5)) },
                Name = "Tomate",
                main_img = new UriImageSource() { Uri = new Uri("https://www.boeschbodenspies.com/wp-content/uploads/2017/08/tomato.png") },
            });


            viewModel.RecipeList.Add(new RecipeModel()
            {
                ShortDescription = "Chicken tikka masala, oft CTM abgekürzt, ist ein häufig in indischen Restaurants in Europa und Nordamerika angebotenes Currygericht aus gegrillten marinierten Hähnchenfleischstücken in einer würzigen Tomatensauce, das eigentlich der englischen Küche zuzurechnen ist.",
                RecipeName = "Chicken Tikka Masala",
                CookingTime = 10,
                Kilocalories = 780,
                MainIngredients = new ObservableCollection<Food>(viewModel.foodList),
                RecipeImage = new UriImageSource() { Uri = new Uri("https://pinchofyum.com/wp-content/uploads/Chicken-Tikka-Masala-Square.jpg") },
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