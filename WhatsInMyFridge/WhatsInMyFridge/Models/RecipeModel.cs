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
            get { return $"{mainIngredients.Count(y => y.isInFridge)}/{mainIngredients.Count}"; }
        }

        public string CookingTimePlaceholder
        {
            get { return $"{cookingTime} min";  }
        }

        public string KilocaloriesPlaceholder
        {
            get { return $"{kiloCalories} kcal";  }
        }

        public string InstructionsPlaceholder
        {
            get 
            { 
                if (instructions != null) {
                    string retVal = string.Empty;
                    for(int i = 0; i < instructions.Count; i++)
                    {
                        retVal += instructions[i] + "\n";
                    }
                    return retVal;
                }
                else 
                {
                    return ""; 
               
                } 
            }
        }


        public string recipeName { get; set; }
        public string shortDescription { get; set; }
        public int cookingTime { get; set; }
        public int kiloCalories { get; set; }

        private string _recipeImage { get; set; }
        public string recipeImage 
        { 
            get { return _recipeImage; }
            set { _recipeImage = value; RecipeImageParsed = new UriImageSource().Uri = new Uri(value); } 
        }

        public ImageSource RecipeImageParsed { get; set; }

        public ObservableCollection<Food> mainIngredients { get; set; } = new ObservableCollection<Food>();

        public List<string> instructions { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
