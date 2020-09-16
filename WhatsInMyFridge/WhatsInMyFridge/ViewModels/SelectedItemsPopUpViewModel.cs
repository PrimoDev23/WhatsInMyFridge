using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Models;
using Xamarin.Forms;

namespace WhatsInMyFridge.ViewModels
{
    public class SelectedItemsPopUpViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Command OKCommand { get; set; }
        public Command<Food> selectCommand { get; set; }

        private ObservableCollection<Food> currentIngredients { get; set; } = new ObservableCollection<Food>();

        public ObservableCollection<Food> CurrentIngredients
        {
            get { return currentIngredients;  }
            set
            {
                currentIngredients = value;
                OnPropertyChanged();
            }
        }

        private IList<object> selectedIngredients { get; set; }

        public IList<object> SelectedIngredients
        {
            get { return selectedIngredients; }
            set
            {
                selectedIngredients = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
