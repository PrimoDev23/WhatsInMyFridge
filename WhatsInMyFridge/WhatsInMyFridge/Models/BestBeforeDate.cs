using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WhatsInMyFridge.Models
{
    public class BestBeforeDate
    {
        public Color dateOver
        {
            get
            {
                if (DateTime.Compare(bestBeforeDate, DateTime.Now) < 0)
                {
                    return Color.Red;
                }
                else
                {
                    return Color.White;
                }
            }
        }
        public DateTime bestBeforeDate { get; set; }

        public string formattedDate
        {
            get { return bestBeforeDate.ToString("d"); }
        }

        public BestBeforeDate(DateTime dt)
        {
            bestBeforeDate = dt;
        }
    }
}
