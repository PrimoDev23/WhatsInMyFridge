using System;
using System.Collections.Generic;
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
    public partial class UsedPopup : ContentView
    {
        private readonly UsedPopupViewModel viewModel = new UsedPopupViewModel();

        public UsedPopup()
        {
            InitializeComponent();

            viewModel.OKCommand = new Command(new Action(OK_Clicked));
            viewModel.cancelCommand = new Command(new Action(Cancel_Clicked));

            BindingContext = viewModel;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "<Ausstehend>")]
        public async Task showPopup(Food food)
        {
            viewModel.food = food;

            viewModel.complete = new TaskCompletionSource<bool>();

            if (food.brand?.Length > 0)
            {
                lblBrand.Text = food.brand;
                lblBrand.IsVisible = true;
            }
            else
            {
                lblBrand.IsVisible = false;
            }
            lblName.Text = food.name;
            lblUnit.Text = food.unit;
            txtAmount.Text = food.amount.ToString();

            bool finish = await viewModel.complete.Task;

            if (finish)
            {
                VarContainer.fridgePage.txtSearch_TextChanged(null, null);
                SaveHandler.saveFood(VarContainer.fridgePage.viewModel.foodList);
            }
        }

        private void OK_Clicked()
        {
            if (double.TryParse(txtAmount.Text, out double amount) && amount <= viewModel.food.amount)
            {
                if (amount == viewModel.food.amount)
                {
                    VarContainer.fridgePage.viewModel.foodList.Remove(viewModel.food);
                }
                viewModel.food.amount -= amount;
            }
            else
            {
                return;
            }

            viewModel.complete?.TrySetResult(true);
        }

        private void Cancel_Clicked()
        {
            viewModel.complete?.TrySetResult(false);
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(txtAmount.Text, out double amount) && amount <= viewModel.food.amount)
            {
                confirmAmount.Text = null;
                btnOK.IsEnabled = true;
            }
            else
            {
                confirmAmount.Text = "Geben Sie eine gültige Menge ein... Diese darf nicht größer als die vorherige Menge sein!";
                btnOK.IsEnabled = false;
            }
        }
    }
}