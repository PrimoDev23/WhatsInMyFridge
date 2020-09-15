using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Controls;
using WhatsInMyFridge.Helper;
using WhatsInMyFridge.Models;
using WhatsInMyFridge.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeOverview : ContentPage
    {
        private readonly RecipeOverviewModel viewModel = new RecipeOverviewModel();

        public RecipeOverview()
        {
            InitializeComponent();

            viewModel.RecipeList.Add(new RecipeModel()
            {
                ShortDescription = "Chicken tikka masala, oft CTM abgekürzt, ist ein häufig in indischen Restaurants in Europa und Nordamerika angebotenes Currygericht aus gegrillten marinierten Hähnchenfleischstücken in einer würzigen Tomatensauce, das eigentlich der englischen Küche zuzurechnen ist.",
                RecipeName = "Chicken Tikka Masala",
                CookingTime = 10,
                Kilocalories = 780,
                MainIngredients = new ObservableCollection<Food>(viewModel.foodList),
                RecipeImage = new UriImageSource() { Uri = new Uri("https://pinchofyum.com/wp-content/uploads/Chicken-Tikka-Masala-Square.jpg") },
            });
            viewModel.FilteredRecipeList = new ObservableCollection<RecipeModel>(viewModel.RecipeList);


            BindingContext = viewModel;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender is FridgeGrid grid)
            {
                Navigation.PushAsync(new FoodDetailPage(grid.FoodItem));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "<Ausstehend>")]
        private async void fab_Clicked(object sender, EventArgs e)
        {
            try
            {
                viewModel.foodList = VarContainer.fridgePage.viewModel.foodList;

                selectItemsPopUp.viewModel.CurrentIngredients = viewModel.foodList;
                selectItemsPopUp.IsVisible = true;

                ObservableCollection<Food> selectedFood = await selectItemsPopUp.waitForFinish();

                if (selectedFood != null)
                {
                    //API Fetch
                    viewModel.SelectedFood = selectedFood;

                    APIHelper.getRecipesFromAPI(selectedFood);



                }

                txtSearch_TextChanged(null, null);
                //SaveHandler.saveFood(viewModel.foodList);
            }
            finally
            {
                selectItemsPopUp.IsVisible = false;
            }
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(txtSearch.Text?.Length > 0))
            {
                void run()
                {
                    viewModel.FilteredRecipeList = new ObservableCollection<RecipeModel>(viewModel.RecipeList);
                }
                await Task.Run(new Action(run)).ConfigureAwait(false);
            }
            else
            {
                void run()
                {
                    viewModel.FilteredRecipeList = new ObservableCollection<RecipeModel>(viewModel.RecipeList.getRecipesBayName(txtSearch.Text));
                }
                await Task.Run(new Action(run)).ConfigureAwait(false);
            }
        }
    }
}