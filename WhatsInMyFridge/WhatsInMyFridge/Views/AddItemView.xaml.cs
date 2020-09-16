using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsInMyFridge.Helper;
using WhatsInMyFridge.Models;
using WhatsInMyFridge.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WhatsInMyFridge.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemView : ContentView
    {
        private readonly AddItemViewModel viewModel = new AddItemViewModel();

        public AddItemView()
        {
            InitializeComponent();

            viewModel.captureImageCommand = new Command(new Action(captureImage));

            BindingContext = viewModel;
        }

        public async Task showDialog()
        {
            viewModel.food = new Food() { addingType = AddingType.Manually };
        }

        private async void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            void run()
            {
                viewModel.food = VarContainer.fridgePage.viewModel.foodList.foodAlreadyAddedName(txtName.Text);
            }
            await Task.Run(new Action(run)).ConfigureAwait(false);
        }

        private async void captureImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions());

            viewModel.food.imageUrl = file.Path;
        }
    }
}