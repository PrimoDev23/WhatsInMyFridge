using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AfterScanPopup : ContentView
    {
        private TaskCompletionSource<ValueTuple<int, int>> complete;

        public AfterScanPopup()
        {
            InitializeComponent();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "<Ausstehend>")]
        public async Task<ValueTuple<int, int>> waitForFinish(Food food, bool unit_can_be_used)
        {
            pickerUnit.IsEnabled = true;
            pickerUnit.SelectedIndex = 0;
            txtAmount.Text = "1";

            //If food is null its not already in the list
            if (food != null)
            {
                pickerUnit.IsEnabled = false;

                if (food.unit == "x")
                {
                    pickerUnit.SelectedIndex = 1;
                }
            }
            else
            {
                if (!unit_can_be_used)
                {
                    pickerUnit.SelectedIndex = 1;
                    pickerUnit.IsEnabled = false;
                }
            }

            complete = new TaskCompletionSource<(int, int)>();
            return await complete.Task;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult((-1, -1));
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(txtAmount.Text, out int parsed))
            {
                complete?.TrySetResult((parsed, pickerUnit.SelectedIndex));
            }
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtAmount.Text, out int amount))
            {
                if (amount == 0)
                {
                    AmountValid.Text = "Die eingegebene Anzahl ist ungültig!";
                    btnOK.IsEnabled = false;
                }
                else {
                    AmountValid.Text = null;
                    btnOK.IsEnabled = true;
                }
            }
            else
            {
                AmountValid.Text = "Die eingegebene Anzahl ist ungültig!";
                btnOK.IsEnabled = false;
            }
        }
    }
}