using SoundBrewPOS.ViewModels;

namespace SoundBrewPOS
{
    public partial class MainPage : ContentPage
    {
        private readonly HomeViewModel _homeViewModel;
        public MainPage(HomeViewModel homeViewModel)
        {
            InitializeComponent();
            _homeViewModel = homeViewModel;
            BindingContext = _homeViewModel;
            Initialize();
        }

        private async void Initialize()
        {
            await _homeViewModel.InitializeAsync();
        }

        private Color _originalColor;

        private void Button_Pressed(object sender, EventArgs e)
        {

            if (sender is Button button)
            {
                // Save the original color
                _originalColor = button.BackgroundColor;
                // Change the color when pressed
                button.BackgroundColor = Colors.DarkGray;
            }

        }

        private void Button_Released(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                // Revert to the original color
                button.BackgroundColor = _originalColor;
            }
        }


    }

}
