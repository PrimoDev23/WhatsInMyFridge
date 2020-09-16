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
        public bool isInFridge { get; set; }
        public Color selectionColor { get; set; }

        public DateTime expirationDate { get; set; }

        private string _imageUrl;
        public string imageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;
                if (addingType == AddingType.Scanned)
                {
                    main_img = ImageSource.FromUri(new Uri(value));
                }
                else
                {
                    main_img = ImageSource.FromFile(value);
                }
            }
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

    public enum AddingType
    {
        Scanned,
        Manually
    }
}
