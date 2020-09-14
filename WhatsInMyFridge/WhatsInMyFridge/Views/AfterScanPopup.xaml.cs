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

        private readonly object valid_lock = new object();
        private bool valid = false;

        public AfterScanPopup()
        {
            InitializeComponent();
        }

        public async Task<ValueTuple<double, DateTime>> waitForFinish()
        {
            complete = new TaskCompletionSource<(double, DateTime)>();
            ValueTuple<double, DateTime> tuple = await complete.Task;
            txtAmount.Text = "";
            txtDate.Text = "";
            return tuple;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete?.TrySetResult((-1, DateTime.MinValue));
        }

        private void btnOK_Clicked(object sender, EventArgs e)
        {
            lock (valid_lock)
            {
                if (!valid)
                {
                    return;
                }
            }

            complete?.TrySetResult((Double.Parse(txtAmount.Text), string.IsNullOrEmpty(txtDate.Text) ? DateTime.MinValue :  DateTime.Parse(txtDate.Text)));
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                if (!double.TryParse(entry.Text, out _))
                {
                    AmountValid.Text = "Die eingegebene Anzahl ist ungültig!";

                    lock (valid_lock)
                    {
                        valid = false;
                    }
                }
                else
                {
                    AmountValid.Text = null;

                    lock (valid_lock)
                    {
                        valid = true;
                    }
                }
            }
        }

        private void txtDate_Unfocused(object sender, FocusEventArgs e)
        {
            if (sender is Entry entry)
            {
                if (!DateTime.TryParse(entry.Text, out _))
                {
                    DateValid.Text = "Das eingegebene Datum ist üngültig!";

                    lock (valid_lock)
                    {
                        valid = false;
                    }
                }
                else
                {
                    DateValid.Text = "";

                    lock (valid_lock)
                    {
                        valid = true;
                    }
                }
            }
        }
    }
}