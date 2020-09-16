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
        private double _Amount = 0;
        public double Amount
        {
            get { return _Amount; }
            set { _Amount = value; OnPropertyChanged(null); }
        }

        public string combinedAmountName
        {
            get { return $"{Amount.ToString()}x {Name}"; }
        }

        public string Name { get; set; }
        public string brand { get; set; }
        public string BarCode { get; set; }

        private string _main_img_url;
        public string main_img_url
        {
            get { return _main_img_url; }
            set { _main_img_url = value; main_img = ImageSource.FromUri(new Uri(value)); }
        }
        [XmlIgnore]
        public ImageSource main_img { get; set; }

        private string _nutrition_img_url;
        public string nutrition_img_url
        {
            get { return _nutrition_img_url; }
            set { _nutrition_img_url = value; nutrition_img_source = ImageSource.FromUri(new Uri(value)); }
        }
        [XmlIgnore]
        public ImageSource nutrition_img_source { get; set; }

        public ObservableCollection<BestBeforeDate> bestBeforeDate { get; set; } = new ObservableCollection<BestBeforeDate>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
