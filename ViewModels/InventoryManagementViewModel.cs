using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundBrewPOS.Data;
using SoundBrewPOS.Models;
using System.Diagnostics;


namespace SoundBrewPOS.ViewModels
{
    public partial class InventoryManagementViewModel :ObservableObject
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        private MenuCategoryModel[] _categories = [];

        [ObservableProperty]
        private string _newItemName;

        [ObservableProperty]
        private decimal _newItemPrice;

        [ObservableProperty]
        private string _newItemDescription;

        [ObservableProperty]
        private string _newItemIconPath; // For the image path of the new item

        private bool _isInitialized;

        [ObservableProperty]
        private int _newItemCategoryId;

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty]
        private MenuCategoryModel? _selectedCategory;

        [ObservableProperty]
        private Data.MenuItem? _selectedMenuItem;

        [ObservableProperty]
        private Data.MenuItem[] _menuItems = [];

        [ObservableProperty] 
        private MenuITemModel[] allMenuItems;

        public InventoryManagementViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadAllMenuItems();
        }
        public async ValueTask InitializeAsync()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;
            IsBusy = true;

            // Fetch categories from the database
            Categories = (await _databaseService.GetMenuCategoriesAsync())
                .Select(MenuCategoryModel.FromEntity)
                .ToArray();         

            IsBusy = false;
        }


        private async void LoadAllMenuItems()
        {
            AllMenuItems = await _databaseService.GetAllMenuItemsAsync();
        }

        [RelayCommand]
        private async Task DeleteMenuItemAsync()
        {
            // Ensure that a menu item is selected before attempting to delete
            if (SelectedMenuItem == null)
            {
                await Shell.Current.DisplayAlert("Error", "Please select a menu item to delete.", "OK");
                return;
            }

            try
            {
                Debug.WriteLine($"Deleting menu item: {SelectedMenuItem.Name}");
                // Call the DeleteMenuItem method to delete the selected menu item from the database
                await _databaseService.DeleteMenuItem(SelectedMenuItem);

                // Clear the selection
                SelectedMenuItem = null;

                // Show a success message
                await Shell.Current.DisplayAlert("Success", "Menu item deleted successfully!", "OK");

                // Refresh the menu items for the selected category
                if (SelectedCategory != null)
                {
                    MenuItems = await _databaseService.GetMenuItemsByCategoryAsync(SelectedCategory.Id);
                }
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting menu item: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "An error occurred while deleting the menu item. Please try again.", "OK");
            }
        }



    }
}
    

