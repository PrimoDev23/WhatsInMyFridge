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
using ZXing;
using ZXing.Mobile;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FridgePage : ContentPage
    {
        public readonly FridgeViewModel viewModel = new FridgeViewModel();

        public FridgePage()
        {
            InitializeComponent();

            void read()
            {
                viewModel.foodList = SaveHandler.readFood();
                viewModel.FilteredFoodList = new ObservableCollection<Food>(viewModel.foodList);
            }

            Task.Run(new Action(read));

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
                string res = await DisplayActionSheet(null, "Abbrechen", null, "Scannen", "Manuell eingeben");

                switch (res)
                {
                    case "Scannen":
                        MobileBarcodeScanner scanner = new MobileBarcodeScanner();

                        Result scan_res = await scanner.Scan();

                        if (scan_res == null)
                        {
                            await DisplayAlert(null, "Fehler beim Auswerten der Daten!", "OK");
                            return;
                        }

                        bool found = true;
                        Food food = viewModel.foodList.foodAlreadyAdded(scan_res.Text);

                        if (food == null)
                        {
                            found = false;
                            food = await APIHelper.getFoodFromCode(scan_res.Text);
                        }

                        if (food == null)
                        {
                            await DisplayAlert(null, "Keinen passenden Artikel gefunden", "OK");
                            return;
                        }

                        afterScanPopup.IsVisible = true;

                        ValueTuple<double, DateTime, string> tuple = await afterScanPopup.waitForFinish();

                        if (tuple.Item1 == 0 || tuple.Item2 == DateTime.MinValue)
                        {
                            return;
                        }

                        double amount = tuple.Item1;
                        DateTime dt = tuple.Item2;

                        food.amount += amount;
                        food.unit = tuple.Item3;

                        for (int i = 0; i < tuple.Item1; i++)
                        {
                            food.bestBeforeDate.Add(new BestBeforeDate(dt));
                        }

                        if (!found)
                        {
                            viewModel.foodList.Add(food);
                        }

                        break;
                    case "Manuell eingeben":
                        break;
                }

                txtSearch_TextChanged(null, null);
                SaveHandler.saveFood(viewModel.foodList);
            }
            finally
            {
                afterScanPopup.IsVisible = false;
            }
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(txtSearch.Text?.Length > 0))
            {
                void run()
                {
                    viewModel.FilteredFoodList = new ObservableCollection<Food>(viewModel.foodList);
                }
                await Task.Run(new Action(run)).ConfigureAwait(false);
            }
            else
            {
                void run()
                {
                    viewModel.FilteredFoodList = new ObservableCollection<Food>(viewModel.foodList.getFoodsByName(txtSearch.Text));
                }
                await Task.Run(new Action(run)).ConfigureAwait(false);
            }
        }
    }
}