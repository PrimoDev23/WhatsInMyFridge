using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SelectItemsPopUp : ContentView
    {
        public SelectedItemsPopUpViewModel viewModel = new SelectedItemsPopUpViewModel();

        private TaskCompletionSource<ObservableCollection<Food>> complete;

        public SelectItemsPopUp()
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "<Ausstehend>")]
        public async Task<ObservableCollection<Food>> waitForFinish()
        {
            complete = new TaskCompletionSource<ObservableCollection<Food>>();
            return await complete.Task;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult(null);
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult(new ObservableCollection<Food>(viewModel.SelectedIngredients.Cast<Food>().ToList()));
        }
    }
}