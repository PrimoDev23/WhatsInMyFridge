using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using WhatsInMyFridge.Models;

namespace WhatsInMyFridge.ViewModels
{
    public class FoodDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Food item { get; set; }
        public ObservableCollection<BestBeforeDate> BestBeforeDates
        {
            get { return item.bestBeforeDate; }
        }
    }
}
