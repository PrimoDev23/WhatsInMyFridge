using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using WhatsInMyFridge.Models;
using Xamarin.Forms;

namespace WhatsInMyFridge.ViewModels
{
    public class AddItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Command captureImageCommand { get; set; }

        public Food food { get; set; }
    }
}
