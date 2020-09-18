using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Helper;
using WhatsInMyFridge.Models;
using WhatsInMyFridge.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectItemsPopUp : ContentView
    {
        public SelectedItemsPopUpViewModel viewModel = new SelectedItemsPopUpViewModel();

        private TaskCompletionSource<ValueTuple<ObservableCollection<Food>, bool>> complete;

        public SelectItemsPopUp()
        {
            InitializeComponent();
            viewModel.selectCommand = new Command<Food>(new Action<Food>(selected));

            this.BindingContext = viewModel;
        }

<<<<<<< HEAD
        public async Task<ObservableCollection<Food>> waitForFinish()
        {
            complete = new TaskCompletionSource<ObservableCollection<Food>>();
            return await complete.Task.ConfigureAwait(false);
=======
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "<Ausstehend>")]
        public async Task<ValueTuple<ObservableCollection<Food>, bool>> waitForFinish()
        {
            complete = new TaskCompletionSource<ValueTuple<ObservableCollection<Food>, bool>>();
            return await complete.Task;
>>>>>>> 344ca054ce08d81e62afd0f07f0529028ec93df2
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult((null, false));
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult((new ObservableCollection<Food>(viewModel.SelectedIngredients.Cast<Food>().ToList()), false));
        }

        private void btnLoadRecipe_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult((new ObservableCollection<Food>(viewModel.SelectedIngredients.Cast<Food>().ToList()), true));
        }

        private void selected(Food food)
        {
            if (viewModel.SelectedIngredients.Contains(food))
            {
                viewModel.SelectedIngredients.Remove(food);
                food.checked_path_data = (PathGeometry)VarContainer.pathConverter.ConvertFromInvariantString("M11.99 2C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z");
            }
            else
            {
                food.checked_path_data = (PathGeometry)VarContainer.pathConverter.ConvertFromInvariantString("M16.59 7.58L10 14.17l-3.59-3.58L5 12l5 5 8-8zM12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z");
                viewModel.SelectedIngredients.Add(food);
            }
        }
    }
}