using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Helper;
using WhatsInMyFridge.Models;
using WhatsInMyFridge.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemView : ContentView
    {
        private readonly AddItemViewModel viewModel = new AddItemViewModel();

        public AddItemView()
        {
            InitializeComponent();

            viewModel.captureCommand = new Command(new Action(capture));
            viewModel.okCommand = new Command(new Action(btnOK_Clicked));
            viewModel.cancelCommand = new Command(new Action(btnCancel_Clicked));

            BindingContext = viewModel;
        }

        public async Task<Food> showDialog()
        {
            pickerUnit.SelectedIndex = 0;
            txtName.Text = null;
            txtAmount.Text = null;
            viewModel.complete = new TaskCompletionSource<bool>();

            bool success = await viewModel.complete.Task.ConfigureAwait(false);

            return success && !viewModel.found ? viewModel.food : null;
        }

        private async void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            void run()
            {
                viewModel.food = VarContainer.fridgePage.viewModel.foodList.foodAlreadyAddedName(txtName.Text);

                if(viewModel.food != null)
                {
                    viewModel.found = true;
                    void lock_picker()
                    {
                        switch (viewModel.food.unit)
                        {
                            case "x":
                                pickerUnit.SelectedIndex = 0;
                                break;
                            case "ml":
                                pickerUnit.SelectedIndex = 1;
                                break;
                            case "l":
                                pickerUnit.SelectedIndex = 2;
                                break;
                            case "g":
                                pickerUnit.SelectedIndex = 3;
                                break;
                            case "kg":
                                pickerUnit.SelectedIndex = 4;
                                break;
                        }
                        pickerUnit.IsEnabled = false;
                    }
                    Device.InvokeOnMainThreadAsync(new Action(lock_picker));
                }
                else
                {
                    viewModel.found = false;
                    void unlock()
                    {
                        pickerUnit.IsEnabled = true;
                    }
                    Device.InvokeOnMainThreadAsync(new Action(unlock));
                    viewModel.food = new Food() { addingType = AddingType.Manually };
                }
            }
            await Task.Run(new Action(run)).ConfigureAwait(false);
        }

        private async void capture()
        {
            await CrossMedia.Current.Initialize().ConfigureAwait(false);

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()).ConfigureAwait(false);

            viewModel.food.imageUrl = file.Path;
        }

        public void btnCancel_Clicked()
        {
            viewModel.complete.TrySetResult(false);
        }

        public void btnOK_Clicked()
        {
            if (txtName.Text?.Length > 0 && int.TryParse(txtAmount.Text, out int amount))
            {
                viewModel.food.name = txtName.Text;

                if(pickerUnit.SelectedIndex == 0)
                {
                    if (viewModel.found)
                    {
                        viewModel.food.amount += amount;
                    }
                    else
                    {
                        viewModel.food.amount = amount;
                        viewModel.food.unit = "x";
                        viewModel.food.single_amount = 1;
                    }
                }
                else
                {
                    if (viewModel.found)
                    {
                        viewModel.food.amount += amount;
                    }
                    else
                    {
                        viewModel.food.amount = amount;
                        viewModel.food.unit = pickerUnit.SelectedItem.ToString();
                    }
                }

                viewModel.complete.TrySetResult(true);
            }
        }

        private bool name_filled = false;
        private bool amount_filled = false;

        private void txtName_Unfocused(object sender, FocusEventArgs e)
        {
            if (txtName.Text?.Length > 0)
            {
                name_filled = true;
                confirmName.Text = null;
            }
            else
            {
                name_filled = false;
                confirmName.Text = "Bitte geben Sie einen Namen ein";
            }

            btnOK.IsEnabled = name_filled && amount_filled;
        }

        private void txtAmount_Unfocused(object sender, FocusEventArgs e)
        {
            if (int.TryParse(txtAmount.Text, out int amount))
            {
                if (amount == 0)
                {
                    confirmAmount.Text = "Bitte geben Sie eine gültige Menge ein!";
                    amount_filled = false;
                }
                else
                {
                    confirmAmount.Text = null;
                    amount_filled = true;
                }
            }
            else
            {
                confirmAmount.Text = "Bitte geben Sie eine gültige Menge ein!";
                amount_filled = false;
            }

            btnOK.IsEnabled = name_filled && amount_filled;
        }
    }
}