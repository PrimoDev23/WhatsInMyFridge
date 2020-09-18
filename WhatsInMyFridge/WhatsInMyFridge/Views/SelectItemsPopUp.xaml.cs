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

        private TaskCompletionSource<ObservableCollection<Food>> complete;

        public SelectItemsPopUp()
        {
            InitializeComponent();
            viewModel.selectCommand = new Command<Food>(new Action<Food>(selected));

            this.BindingContext = viewModel;
        }

        public async Task<ObservableCollection<Food>> waitForFinish()
        {
            complete = new TaskCompletionSource<ObservableCollection<Food>>();
            return await complete.Task.ConfigureAwait(false);
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult(null);
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult(new ObservableCollection<Food>(viewModel.SelectedIngredients.Cast<Food>().ToList()));
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