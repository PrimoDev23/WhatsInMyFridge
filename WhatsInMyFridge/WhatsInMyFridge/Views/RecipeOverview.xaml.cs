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
using Xamarin.Forms.Internals;
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
                shortDescription = "Chicken tikka masala, oft CTM abgekürzt, ist ein häufig in indischen Restaurants in Europa und Nordamerika angebotenes Currygericht aus gegrillten marinierten Hähnchenfleischstücken in einer würzigen Tomatensauce, das eigentlich der englischen Küche zuzurechnen ist.",
                recipeName = "Chicken Tikka Masala",
                cookingTime = 10,
                kiloCalories = 780,
                mainIngredients = new ObservableCollection<Food>(viewModel.foodList),
                RecipeImageParsed = new UriImageSource() { Uri = new Uri("https://pinchofyum.com/wp-content/uploads/Chicken-Tikka-Masala-Square.jpg") },
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
                    //API Rezepe abrufen zu den ausgewählten Zutaten
                    viewModel.SelectedFood = selectedFood;
                    List<RecipeModel> avaiableRecipes = await APIHelper.getRecipesFromAPI(selectedFood);

                    if(avaiableRecipes != null)
                    {
                        viewModel.RecipeList = new ObservableCollection<RecipeModel>(avaiableRecipes);
                    }
                    else
                    {
                        await DisplayAlert("Achtung", "Zu den gewählten Zutaten wurde kein Rezept gefunden! Wir empfehlen Lieferando.", "OK");
                    }
                }

                txtSearch_TextChanged(null, null);
            }
            finally
            {
                selectItemsPopUp.IsVisible = false;
            }
        }        
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "<Ausstehend>")]
        private async void addRecipe(object sender, EventArgs e)
        {
            try
            {
                addRecipeView.IsVisible = true;

                RecipeModel newRecipe = await addRecipeView.waitForFinish();

                if(newRecipe != null)
                {
                    newRecipe.mainIngredients = viewModel.SelectedFood;
                    newRecipe.mainIngredients.ForEach(y => { y.main_img = null; y.nutrition_img_source = null; });
                    bool success = await APIHelper.addRecipeRequest(newRecipe);
                    if (success)
                    {
                        await DisplayAlert("Meldung", "Das Rezept wurde an die Administration gesendet.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Meldung", "Rezept konnte nicht versendet werden / wurde abgelehnt.", "OK");
                    }
                }
            }
            finally
            {
                addRecipeView.IsVisible = false;
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