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
        TaskCompletionSource<ValueTuple<double, DateTime>> complete = new TaskCompletionSource<(double, DateTime)>();

        object valid_lock = new object();
        bool valid = false;

        public AfterScanPopup()
        {
            InitializeComponent();
        }

        public async Task<ValueTuple<double, DateTime>> waitForFinish()
        {
            return await complete.Task;
        }

        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            complete.TrySetResult((-1, DateTime.MinValue));
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

            complete.TrySetResult((Double.Parse(txtAmount.Text), DateTime.Parse(txtDate.Text)));
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                double parsed;
                if (!Double.TryParse(entry.Text, out parsed))
                {
                    AmountValid.IsVisible = true;

                    lock (valid_lock)
                    {
                        valid = false;
                    }
                }
                else
                {
                    AmountValid.IsVisible = false;

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
                DateTime parsed;
                if (!DateTime.TryParse(entry.Text, out parsed))
                {
                    DateValid.IsVisible = true;

                    lock (valid_lock)
                    {
                        valid = false;
                    }
                }
                else
                {
                    DateValid.IsVisible = false;

                    lock (valid_lock)
                    {
                        valid = true;
                    }
                }
            }
        }
    }
}