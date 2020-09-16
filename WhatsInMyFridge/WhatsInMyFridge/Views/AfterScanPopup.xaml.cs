using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AfterScanPopup : ContentView
    {
        private TaskCompletionSource<ValueTuple<double, DateTime, string>> complete;

        DateTime default_dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

        static List<string> UnitOfMeasure = new List<string>() { "Anzahl", "Liter", "Gramm", "Kilogramm", "Mililiter", "Tonne" };

        public AfterScanPopup()
        {
            InitializeComponent();

            date.Date = default_dt;

            pickerUnit.ItemsSource = UnitOfMeasure;

            pickerUnit.SelectedIndex = 0;
        }

        public async Task<ValueTuple<double, DateTime, string>> waitForFinish()
        {
            complete = new TaskCompletionSource<(double, DateTime, string)>();
            ValueTuple<double, DateTime, string> tuple = await complete.Task;
            txtAmount.Text = "";
            pickerUnit.SelectedItem = null;
            date.Date = default_dt;
            return tuple;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult((-1, DateTime.MinValue, null));
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            //TODO: Rwmove selecteditem check
            if (double.TryParse(txtAmount.Text, out double parsed))
            {
                complete?.TrySetResult((parsed, date.Date, pickerUnit.SelectedItem.ToString()));
            }
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(txtAmount.Text?.Length > 0))
            {
                AmountValid.Text = null;
                return;
            }

            if (!double.TryParse(txtAmount.Text, out _))
            {
                AmountValid.Text = "Die eingegebene Anzahl ist ungültig!";
            }
            else
            {
                AmountValid.Text = null;
            }
        }

        private void pickerUnit_Unfocused(object sender, FocusEventArgs e)
        {
            if(pickerUnit.SelectedItem != null)
            {
                txtAmount.Placeholder = $"Menge ({pickerUnit.SelectedItem})";
            }
        }
    }
}