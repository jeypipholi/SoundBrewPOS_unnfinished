using SoundBrewPOS.Data;
using SoundBrewPOS.Models;
using SoundBrewPOS.ViewModels;

namespace SoundBrewPOS;

public partial class ManageMenuItems : ContentPage
{
    private readonly HomeViewModel _homeViewModel;
    private readonly DatabaseService _databaseService;

    public ManageMenuItems(HomeViewModel homeViewModel, DatabaseService databaseService)
	{
		InitializeComponent();
        _homeViewModel = homeViewModel;
        _databaseService = databaseService;
        BindingContext = _homeViewModel;
        
        //InitializedAsync();
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
        if(sender is Button button)
    {
            // Revert to the original color
            button.BackgroundColor = _originalColor;
        }
    }

    private void Button_Pressed_1(object sender, EventArgs e)
    {
        
    }

  
    /* private async void InitializedAsync() => 
await _manageMenuItemsViewModel.InitializeAsync();*/

}