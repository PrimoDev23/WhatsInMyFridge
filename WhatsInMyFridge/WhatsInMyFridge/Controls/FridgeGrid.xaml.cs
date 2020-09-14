using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FridgeGrid : Grid
    {
        #region Properties

        public Food FoodItem
        {
            get { return (Food)GetValue(FoodItemProperty); }
            set { SetValue(FoodItemProperty, value); }
        }

        public static readonly BindableProperty FoodItemProperty = BindableProperty.Create(nameof(FoodItem), typeof(Food), typeof(FridgeGrid), null);

        #endregion Properties

        public FridgeGrid()
        {
            InitializeComponent();
        }
    }
}