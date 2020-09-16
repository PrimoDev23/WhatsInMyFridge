using System.ComponentModel;
using System.Runtime.CompilerServices;
using WhatsInMyFridge.Models;

namespace WhatsInMyFridge.ViewModels
{
    public class RecipeDetailViewModel : INotifyPropertyChanged
    {
        public RecipeModel mainRecipe { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
