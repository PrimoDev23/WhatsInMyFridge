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
        FridgeViewModel viewModel = new FridgeViewModel();

        public FridgePage()
        {
            InitializeComponent();

            void read()
            {
                viewModel.foodList = SaveHandler.readFood();
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

        public void Scan_Clicked(object o)
        {

        }

        public void Insert_Clicked(object o)
        {

        }

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

                        ValueTuple<double, DateTime> tuple = await afterScanPopup.waitForFinish();

                        if (tuple.Item1 == 0 || tuple.Item2 == DateTime.MinValue)
                        {
                            return;
                        }

                        double amount = tuple.Item1;
                        DateTime dt = tuple.Item2;

                        food.Amount += amount;

                        for (int i = 0; i < amount; i++)
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
                SaveHandler.saveFood(viewModel.foodList);
            }
            finally
            {
                afterScanPopup.IsVisible = false;
            }
        }
    }
}