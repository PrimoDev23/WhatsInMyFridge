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
        private TaskCompletionSource<ValueTuple<double, DateTime>> complete;

        DateTime default_dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);

        public AfterScanPopup()
        {
            InitializeComponent();

            date.Date = default_dt;
        }

        public async Task<ValueTuple<double, DateTime>> waitForFinish()
        {
            complete = new TaskCompletionSource<(double, DateTime)>();
            ValueTuple<double, DateTime> tuple = await complete.Task;
            txtAmount.Text = "";
            date.Date = default_dt;
            return tuple;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult((-1, DateTime.MinValue));
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            double parsed;
            DateTime dt;

            if (double.TryParse(txtAmount.Text, out parsed))
            {
                complete?.TrySetResult((parsed, date.Date));
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
    }
}