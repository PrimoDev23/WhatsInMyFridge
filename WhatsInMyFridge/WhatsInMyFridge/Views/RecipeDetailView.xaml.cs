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
    public partial class RecipeDetailView : ContentPage
    {
        private readonly RecipeDetailViewModel viewModel = new RecipeDetailViewModel();

        public RecipeDetailView(RecipeModel mainRecipe)
        {
            InitializeComponent();

            viewModel.mainRecipe = mainRecipe;

            mainCollection.HeightRequest = (viewModel.mainRecipe.mainIngredients.Count * 30) + (viewModel.mainRecipe.mainIngredients.Count * 2 * 5);

            viewModel.gridHeight = mainCollection.HeightRequest;

            this.BindingContext = viewModel;
        }
    }
}