using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SoundBrewPOS.Data;
using SoundBrewPOS.Models;
using MenuItem = SoundBrewPOS.Data.MenuItem;

namespace SoundBrewPOS.ViewModels
{
    public partial class ManageMenuItemsViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly HomeViewModel _homeViewModel;
        public ManageMenuItemsViewModel(DatabaseService databaseService,HomeViewModel homeViewModel)
        {
            _databaseService = databaseService;
            _homeViewModel = homeViewModel;
        }

        [ObservableProperty]
        private MenuCategoryModel[] _categories = [];

        [ObservableProperty]
        private MenuItem[] _menuItems = [];

        [ObservableProperty]
        private MenuCategoryModel? _selectedCategory;

        [ObservableProperty]
        private bool _isBusy;

        private bool _isInitialized;

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

            // Set the first category as selected by default
            if (Categories.Any())
            {
                Categories[0].IsSelected = true;
                SelectedCategory = Categories[0];
            }

            MenuItems = await _databaseService.GetMenuItemsByCategoryAsync(SelectedCategory.Id);

            IsBusy = false;
        }


        [RelayCommand]
        private async Task SelectCategoryAsync(int categoryId)
        {
            if (SelectedCategory.Id == categoryId)
                return;

            IsBusy = true;
            var existingSelectedCategory = Categories.First(c => c.IsSelected);
            existingSelectedCategory.IsSelected = false;

            var newlySelectedCategory = Categories.First(c => c.Id == categoryId);
            newlySelectedCategory.IsSelected = true;

            SelectedCategory = newlySelectedCategory;

            MenuItems = await _databaseService.GetMenuItemsByCategoryAsync(SelectedCategory.Id);
            IsBusy = false;
        }

        [RelayCommand]
        private async Task EditMenuItemsAsync(MenuItem menuItem)
        {
            await Shell.Current.DisplayAlert("Edit", "Edit menu item", "Ok");
        }

    }
}
