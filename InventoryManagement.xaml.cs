using CommunityToolkit.Mvvm.ComponentModel;
using SoundBrewPOS.Data;
using SoundBrewPOS.ViewModels;

namespace SoundBrewPOS;

public partial class InventoryManagement : ContentPage
{
	
	private readonly InventoryManagementViewModel _inventoryViewModel;
    
    public InventoryManagement(InventoryManagementViewModel inventoryViewModel)
	{
		InitializeComponent();
		_inventoryViewModel = inventoryViewModel;
        BindingContext = _inventoryViewModel;
        Initialize();
	}

    private async void Initialize()
    {
        await _inventoryViewModel.InitializeAsync();
    }


}