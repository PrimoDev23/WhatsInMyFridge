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

            List<Food> debug = new List<Food>();

            Food tomate = new Food()
            {
                name = "Tomate",
                unit = "kg",
                amount = 0.8,
                imageUrl = "https://www.boeschbodenspies.com/wp-content/uploads/2017/08/tomato.png",
                isInFridge = true,
            };
            debug.Add(tomate);

            Food kartoffel = new Food()
            {
                name = "Kartoffeln",
                unit = "kg",
                amount = 2.2,
                imageUrl = "https://www.alimentarium.org/de/system/files/thumbnails/image/AL027-01_pomme_de_terre_0.jpg",
                isInFridge = true,
            };
            debug.Add(kartoffel);

            Food milch = new Food()
            {
                name = "Milch",
                unit = "l",
                amount = 0.4,
                imageUrl = "https://cdn.webshopapp.com/shops/47345/files/132823412/image.jpg",
                isInFridge = false,
            };
            debug.Add(milch);

            viewModel.RecipeList.Add(new RecipeModel()
            {
                shortDescription = "Chicken tikka masala, oft CTM abgekürzt, ist ein häufig in indischen Restaurants in Europa und Nordamerika angebotenes Currygericht aus gegrillten marinierten Hähnchenfleischstücken in einer würzigen Tomatensauce, das eigentlich der englischen Küche zuzurechnen ist.",
                recipeName = "Chicken Tikka Masala",
                cookingTime = 10,
                kiloCalories = 780,
                mainIngredients = new ObservableCollection<Food>(debug),
                RecipeImageParsed = new UriImageSource() { Uri = new Uri("https://pinchofyum.com/wp-content/uploads/Chicken-Tikka-Masala-Square.jpg") },
                instructions = new List<string>() { "1. Hühnchen in Topf" },
            });
            viewModel.FilteredRecipeList = new ObservableCollection<RecipeModel>(viewModel.RecipeList);

            BindingContext = viewModel;
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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (sender is FridgeGrid grid)
            {
                Navigation.PushAsync(new RecipeDetailView(grid.RecipeItem));
            }
        }
    }
}