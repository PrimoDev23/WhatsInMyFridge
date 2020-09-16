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
        private TaskCompletionSource<ValueTuple<int, DateTime, int>> complete;

        private readonly DateTime default_dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

        public AfterScanPopup()
        {
            InitializeComponent();

            date.Date = default_dt;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1090:Call 'ConfigureAwait(false)'.", Justification = "<Ausstehend>")]
        public async Task<ValueTuple<int, DateTime, int>> waitForFinish(Food food, bool unit_can_be_used)
        {
            pickerUnit.SelectedIndex = 0;

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

            complete = new TaskCompletionSource<(int, DateTime, int)>();
            ValueTuple<int, DateTime, int> tuple = await complete.Task;

            pickerUnit.IsEnabled = true;
            pickerUnit.SelectedIndex = 0;
            txtAmount.Text = "1";
            date.Date = default_dt;
            return tuple;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult((-1, DateTime.MinValue, -1));
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            if (int.TryParse(txtAmount.Text, out int parsed))
            {
                complete?.TrySetResult((parsed, date.Date, pickerUnit.SelectedIndex));
            }
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(txtAmount.Text?.Length > 0))
            {
                AmountValid.Text = null;
                return;
            }

            if (!int.TryParse(txtAmount.Text, out _))
            {
                AmountValid.Text = "Die eingegebene Anzahl ist ungültig!";
            }
            else
            {
                AmountValid.Text = null;
            }
        }
    }
}