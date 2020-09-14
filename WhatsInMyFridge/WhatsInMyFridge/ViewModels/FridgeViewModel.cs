﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using WhatsInMyFridge.Models;
using Xamarin.Forms;

namespace WhatsInMyFridge.ViewModels
{
    public class FridgeViewModel : INotifyPropertyChanged
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
        
        private ObservableCollection<Food> filteredFoodList { get; set; } = new ObservableCollection<Food>();
        public ObservableCollection<Food> FilteredFoodList
        {
            get
            {
                return filteredFoodList;
            }
            set
            {
                filteredFoodList = value;
                OnPropertyChanged();
            }
        }

        private string searchText { get; set; }

        public string SearchText
        { 
            get { return searchText; } 
            set 
            { 
                searchText = value;
                if (string.IsNullOrEmpty(value))
                {
                    FilteredFoodList = new ObservableCollection<Food>(foodList);
                }
                else
                {
                    FilteredFoodList = new ObservableCollection<Food>(foodList.Where(y => y.Name.Contains(searchText)).ToList());
                }

            } 
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
