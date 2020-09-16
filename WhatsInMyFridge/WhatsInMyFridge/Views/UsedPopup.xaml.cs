using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Helper;
using WhatsInMyFridge.Models;
using WhatsInMyFridge.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsedPopup : ContentView
    {
        private readonly UsedPopupViewModel viewModel = new UsedPopupViewModel();

        public UsedPopup()
        {
            InitializeComponent();

            viewModel.OKCommand = new Command(new Action(OK_Clicked));
            viewModel.selectCommand = new Command<BestBeforeDate>(new Action<BestBeforeDate>(selected));

            BindingContext = viewModel;
        }

        public async Task showPopup(Food food)
        {
            viewModel.food = food;

            viewModel.complete = new TaskCompletionSource<bool>();

            viewModel.bestBeforeDates.Clear();

            await viewModel.complete.Task;

            for(int i = 0;i < viewModel.bestBeforeDates.Count; i++)
            {
                food.bestBeforeDate.Remove(viewModel.bestBeforeDates[i]);
                food.Amount--;
            }
        }

        private void OK_Clicked()
        {
            viewModel.complete?.TrySetResult(true);
        }

        private void selected(BestBeforeDate bbd)
        {
            if (viewModel.bestBeforeDates.Contains(bbd))
            {
                viewModel.bestBeforeDates.Remove(bbd);
                bbd.checked_path_data = (PathGeometry)VarContainer.pathConverter.ConvertFromInvariantString("M11.99 2C6.47 2 2 6.48 2 12s4.47 10 9.99 10C17.52 22 22 17.52 22 12S17.52 2 11.99 2zM12 20c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z");
            }
            else
            {
                bbd.checked_path_data = (PathGeometry)VarContainer.pathConverter.ConvertFromInvariantString("M16.59 7.58L10 14.17l-3.59-3.58L5 12l5 5 8-8zM12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm0 18c-4.42 0-8-3.58-8-8s3.58-8 8-8 8 3.58 8 8-3.58 8-8 8z");
                viewModel.bestBeforeDates.Add(bbd);
            }
        }
    }
}