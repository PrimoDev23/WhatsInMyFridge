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

            viewModel.usedCommand = new Command<Food>(new Action<Food>(used));

            BindingContext = viewModel;
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

                        //Unit != "x" just says -> quantity found with API
                        ValueTuple<int, DateTime, int> tuple = await afterScanPopup.waitForFinish(found ? food : null, food.unit != "x");

                        //Cancel pressed
                        if (tuple.Item1 == -1 && tuple.Item2 == DateTime.MinValue)
                        {
                            return;
                        }

                        DateTime dt = new DateTime(tuple.Item2.Year, tuple.Item2.Month, tuple.Item2.Day, 23, 59, 59);

                        //unit selected
                        if (tuple.Item3 == 0)
                        {
                            //Amount for one item (like 350g)
                            double amount;
                            if (found)
                            {
                                amount = food.amount / food.amount_list.Count;
                                food.amount += tuple.Item1 * amount;
                            }
                            else
                            {
                                amount = food.amount;
                                food.amount = tuple.Item1 * food.amount;
                            }

                            for (int i = 0; i < tuple.Item1; i++)
                            {
                                food.bestBeforeDate.Add(new BestBeforeDate(dt, food.bestBeforeDate.Count));
                                food.amount_list.Add(amount);
                            }
                        }
                        else //amount selected
                        {
                            if (found)
                            {
                                food.amount += tuple.Item1;
                            }
                            else
                            {
                                food.amount = tuple.Item1;
                                food.unit = "x";
                            }

                            for (int i = 0; i < tuple.Item1; i++)
                            {
                                food.bestBeforeDate.Add(new BestBeforeDate(dt, food.bestBeforeDate.Count));
                                food.amount_list.Add(1);
                            }
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

        public async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
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

        private async void used(Food selected)
        {
            usedPopup.IsVisible = true;

            await usedPopup.showPopup(selected);

            usedPopup.IsVisible = false;
        }
    }
}