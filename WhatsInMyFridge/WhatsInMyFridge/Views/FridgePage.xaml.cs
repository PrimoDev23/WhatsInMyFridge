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
                        ValueTuple<int, int> tuple = await afterScanPopup.waitForFinish(found ? food : null, food.unit != "x");

                        //Cancel pressed
                        if (tuple.Item1 == -1)
                        {
                            return;
                        }

                        //unit selected
                        if (tuple.Item2 == 0)
                        {
                            //Amount for one item (like 350g)
                            if (found)
                            {
                                food.amount += tuple.Item1 * food.single_amount;
                            }
                            else
                            {
                                food.amount = tuple.Item1 * food.single_amount;
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
                                food.single_amount = 1;
                                food.amount = tuple.Item1;
                                food.unit = "x";
                            }
                        }

                        if (!found)
                        {
                            viewModel.foodList.Add(food);
                        }

                        break;
                    case "Manuell eingeben":
                        addItemView.IsVisible = true;
                        Food new_food = await addItemView.showDialog();
                        addItemView.IsVisible = false;

                        if(new_food != null)
                        {
                            viewModel.foodList.Add(new_food);
                        }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "<Ausstehend>")]
        private async void used(Food selected)
        {
            usedPopup.IsVisible = true;

            await usedPopup.showPopup(selected);

            usedPopup.IsVisible = false;
        }
    }
}