using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhatsInMyFridge.Models
{
    public class RecipeModel : INotifyPropertyChanged
    {
        public string IngredientPlaceholder
        {
            get { return $"3/{MainIngredients.Count}"; }
        }

        public string CookingTimePlaceholder
        {
            get { return $"{CookingTime} min";  }
        }

        public string KilocaloriesPlaceholder
        {
            get { return $"{Kilocalories} kcal";  }
        }


        public string RecipeName { get; set; }
        public string ShortDescription { get; set; }
        public int CookingTime { get; set; }
        public int Kilocalories { get; set; }
        public ImageSource RecipeImage { get; set; }

        public ObservableCollection<Food> MainIngredients { get; set; } = new ObservableCollection<Food>();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
