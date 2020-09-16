﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WhatsInMyFridge.Models;

namespace WhatsInMyFridge.Helper
{
    public static class LINQReplacer
    {
        public static Food foodAlreadyAdded(this ObservableCollection<Food> items, string code)
        {
            for (int i = 0; i < items.Count; i++)
            {
                Food food = items[i];
                if(food.BarCode == code)
                {
                    return food;
                }
            }
            return null;
        }

        public static IEnumerable<Food> getFoodsByName(this ObservableCollection<Food> items, string name)
        {
            string lower = name.ToLower();
            for(int i = 0;i < items.Count; i++)
            {
                Food food = items[i];
                if(food.name.ToLower().Contains(lower)){
                    yield return food;
                }
            }
        }

        public static IEnumerable<RecipeModel> getRecipesBayName(this ObservableCollection<RecipeModel> items, string name)
        {
            string lower = name.ToLower();
            for(int i = 0;i < items.Count; i++)
            {
                RecipeModel recipe = items[i];
                if(recipe.recipeName.ToLower().Contains(lower)){
                    yield return recipe;
                }
            }
        }
    }
}
