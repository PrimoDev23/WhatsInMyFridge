using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using WhatsInMyFridge.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace WhatsInMyFridge.Models
{
    public class Food : INotifyPropertyChanged
    {
        public AddingType addingType;

        private double _amount = 0;
        public double amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged(null); }
        }

        public List<double> amount_list = new List<double>();

        public string combinedAmountName
        {
            get { return $"{amount.ToString()}{unit} {name}"; }
        }

        public string name { get; set; }
        public string unit { get; set; }
        public string brand { get; set; }
        public string BarCode { get; set; }
        public bool? isInFridge { get; set; }
        public Color selectionColor { get; set; }

        public DateTime expirationDate { get; set; }

        private string _imageUrl;
        public string imageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; main_img = ImageSource.FromUri(new Uri(value)); }
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


        //Default is not selected
        private PathGeometry _checked_path_data = (PathGeometry)VarContainer.pathConverter.ConvertFromInvariantString("M11.99 2C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z");
        [XmlIgnore]
        public PathGeometry checked_path_data
        {
            get { return _checked_path_data; }
            set { _checked_path_data = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum AddingType
    {
        Scanned,
        Manually
    }
}
