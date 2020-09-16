using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using WhatsInMyFridge.Helper;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace WhatsInMyFridge.Models
{
    public class BestBeforeDate : INotifyPropertyChanged
    {
        public int Index { get; set; }

        [XmlIgnore]
        public Color dateOver
        {
            get
            {
                DateTime default_dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                if (DateTime.Compare(bestBeforeDate, default_dt) < 0)
                {
                    return Color.Red;
                }
                else
                {
                    return Color.Black;
                }
            }
        }

        public DateTime bestBeforeDate { get; set; }

        [XmlIgnore]
        public string formattedDate
        {
            get { return bestBeforeDate.ToString("d"); }
        }

        //Default is not selected
        private PathGeometry _checked_path_data = (PathGeometry)VarContainer.pathConverter.ConvertFromInvariantString("M11.99 2C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z");
        [XmlIgnore]
        public PathGeometry checked_path_data
        {
            get { return _checked_path_data; }
            set { _checked_path_data = value; OnPropertyChanged(); }
        }

        public BestBeforeDate(DateTime dt, int index)
        {
            bestBeforeDate = dt;
            Index = index;
        }

        public BestBeforeDate()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
