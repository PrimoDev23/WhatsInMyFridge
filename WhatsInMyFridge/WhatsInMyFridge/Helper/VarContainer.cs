﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WhatsInMyFridge.ViewModels;
using WhatsInMyFridge.Views;

namespace WhatsInMyFridge.Helper
{
    public static class VarContainer
    {
        public static string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string food_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "food.xml");
        public static FridgePage fridgePage;
    }
}
