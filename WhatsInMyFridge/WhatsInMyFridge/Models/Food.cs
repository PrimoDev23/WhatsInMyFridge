using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace WhatsInMyFridge.Models
{
    public class Food : INotifyPropertyChanged
    {
        private int _Amount = 0;
        public int Amount
        {
            get { return _Amount; }
            set { _Amount = value; OnPropertyChanged(null); }
        }

        public string combinedAmountName
        {
            get { return $"{Amount.ToString()}x {Name}"; }
        }

        public string Name { get; set; }
        public string ingredients_string { get; set; }
        public string brand { get; set; }
        public ImageSource main_img { get; set; }
        public ImageSource nutrition_img_source { get; set; }

        public ObservableCollection<BestBeforeDate> bestBeforeDate { get; set; } = new ObservableCollection<BestBeforeDate>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
