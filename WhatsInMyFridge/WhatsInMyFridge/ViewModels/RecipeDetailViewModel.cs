using System.ComponentModel;
using System.Runtime.CompilerServices;
using WhatsInMyFridge.Models;
using Xamarin.Forms;

namespace WhatsInMyFridge.ViewModels
{
    public class RecipeDetailViewModel : INotifyPropertyChanged
    {
        public RecipeModel mainRecipe { get; set; }

        public GridLength gridHeight { get; set;}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
