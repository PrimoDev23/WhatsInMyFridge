using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Models;
using Xamarin.Forms;

namespace WhatsInMyFridge.ViewModels
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool found = false;

        public Food food { get; set; }

        public TaskCompletionSource<bool> complete;

        public Command captureCommand { get; set; }
        public Command cancelCommand { get; set; }
        public Command okCommand { get; set; }
    }
}
