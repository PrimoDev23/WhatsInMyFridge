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
    public partial class AddRecipeView : ContentView
    {

        private TaskCompletionSource<RecipeModel> complete;
        private AddRecipeViewModel viewModel = new AddRecipeViewModel();


        public AddRecipeView()
        {
            InitializeComponent();
            this.BindingContext = viewModel;
            btnOK.IsEnabled = false;
        }

        public async Task<RecipeModel> waitForFinish()
        {
            complete = new TaskCompletionSource<RecipeModel>();
            RecipeModel retVal = await complete.Task;
            return retVal;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult(null);
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult(new RecipeModel()
            {
                recipeName = txt_recipeName.Text,
                shortDescription = txt_desc.Text,
                cookingTime = int.Parse(txt_cookingTime.Text),
                kiloCalories = int.Parse(txt_kiloCalories.Text),
                recipeImage = txt_recipeUrl.Text,
                instructions = edt_instruct.Text.Split('\n').ToList(),
            });
            txt_cookingTime.Text = string.Empty;
            txt_desc.Text = string.Empty;
            txt_kiloCalories.Text = string.Empty;
            txt_recipeName.Text = string.Empty;
            txt_recipeUrl.Text = string.Empty;
            edt_instruct.Text = string.Empty;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (validateInput())
            {
                btnOK.IsEnabled = true;
            }
            else
            {
                btnOK.IsEnabled = false;
            }
        }

        private bool validateInput()
        {
            int notFilledBoxes = 0;

            if(txt_recipeName.Text == null || txt_recipeName.Text?.Length < 1)
            {
                txt_recipeName.BackgroundColor = Color.IndianRed;
                notFilledBoxes++;
            }
            else
            {
                txt_recipeName.BackgroundColor = Color.White;
            }

            if(txt_recipeUrl.Text == null || txt_recipeUrl.Text?.Length < 1)
            {
                txt_recipeUrl.BackgroundColor = Color.IndianRed;
                notFilledBoxes++;
            }
            else
            {
                txt_recipeUrl.BackgroundColor = Color.White;
            }

            if (txt_cookingTime.Text == null || txt_cookingTime.Text?.Length < 1)
            {
                txt_cookingTime.BackgroundColor = Color.IndianRed;
                notFilledBoxes++;
            }
            else
            {
                txt_cookingTime.BackgroundColor = Color.White;
            }

            if (txt_desc.Text == null || txt_desc.Text?.Length < 1)
            {
                txt_desc.BackgroundColor = Color.IndianRed;
                notFilledBoxes++;
            }
            else
            {
                txt_desc.BackgroundColor = Color.White;
            }

            if (txt_kiloCalories.Text == null || txt_kiloCalories.Text?.Length < 1)
            {
                txt_kiloCalories.BackgroundColor = Color.IndianRed;
                notFilledBoxes++;
            }
            else
            {
                txt_kiloCalories.BackgroundColor = Color.White;
            }

            if (edt_instruct.Text == null || edt_instruct.Text?.Length < 1)
            {
                edt_instruct.BackgroundColor = Color.IndianRed;
                notFilledBoxes++;
            }
            else
            {
                edt_instruct.BackgroundColor = Color.White;
            }

            if (notFilledBoxes == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}