﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WhatsInMyFridge.Models;

namespace WhatsInMyFridge.ViewModels
{
    public class RecipeOverviewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Food> _foodList { get; set; } = new ObservableCollection<Food>();
        public ObservableCollection<Food> foodList
        {
            get
            {
                return _foodList;
            }
            set
            {
                _foodList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Food> selectedFoood { get; set; } = new ObservableCollection<Food>();
        public ObservableCollection<Food> SelectedFood
        {
            get
            {
                return selectedFoood;
            }
            set
            {
                selectedFoood = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RecipeModel> recipeList { get; set; } = new ObservableCollection<RecipeModel>();
        public ObservableCollection<RecipeModel> RecipeList
        {
            get
            {
                return recipeList;
            }
            set
            {
                recipeList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<RecipeModel> filteredRecipeList { get; set; } = new ObservableCollection<RecipeModel>();
        public ObservableCollection<RecipeModel> FilteredRecipeList
        {
            get
            {
                return filteredRecipeList;
            }
            set
            {
                filteredRecipeList = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
