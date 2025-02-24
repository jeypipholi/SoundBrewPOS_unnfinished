using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundBrewPOS.Data;
using SoundBrewPOS.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using MenuItem = SoundBrewPOS.Data.MenuItem;

namespace SoundBrewPOS.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly OrdersViewModel ordersViewModel;

        [ObservableProperty]
        private string? _selectedImagePath;

        [ObservableProperty]
        private MenuItem? _selectedMenuItem;

        // Observable Properties
        [ObservableProperty]
        private MenuCategoryModel[] _categories = [];

        [ObservableProperty]
        private MenuItem[] _menuItems = [];

        [ObservableProperty]
        private MenuCategoryModel? _selectedCategory;

        public ObservableCollection<CartModel> CartItems { get; set; } = new();

        [ObservableProperty]
        private bool _isBusy;

        [ObservableProperty, NotifyPropertyChangedFor(nameof(TaxAamount))]
        [NotifyPropertyChangedFor(nameof(Total))]
        private decimal _subtotal;


        [ObservableProperty, NotifyPropertyChangedFor(nameof(TaxAamount))]
        [NotifyPropertyChangedFor(nameof(Total))]
        private int _taxPercentage;


        public decimal TaxAamount => (Subtotal * TaxPercentage) / 100;

        public decimal Total => Subtotal + TaxAamount;

        [ObservableProperty]
        private decimal _amountPaid; // New property for binding AmountPaid Entry

        [ObservableProperty]
        private decimal _change;


        // Relay Command for selecting a category
        public IRelayCommand<MenuCategoryModel> OnCategorySelectedCommand { get; }

        private bool _isInitialized;



        // Constructor
        public HomeViewModel(DatabaseService databaseService, OrdersViewModel ordersViewModel)
        {
            _databaseService = databaseService;
            this.ordersViewModel = ordersViewModel;
            OnCategorySelectedCommand = new RelayCommand<MenuCategoryModel>(OnCategorySelected);
            //PickImageCommand = new AsyncRelayCommand(PickImageAsync);
            CartItems.CollectionChanged += CartItems_CollectionChanged;

          
        }

        private void CartItems_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RecalculateAmounts();
        }

        // Initialization Method
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

        // Command Action for selecting a category
        private void OnCategorySelected(MenuCategoryModel selectedCategory)
        {
            SelectedCategory = selectedCategory;

            // Update IsSelected for each category
            foreach (var category in Categories)
            {
                category.IsSelected = category == selectedCategory;
            }
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
        private void AddToCart(MenuItem menuItem)
        {
            var cartItem = CartItems.FirstOrDefault(c => c.ItemId == menuItem.Id);

            if (cartItem == null)
            {
                //does not exist, add to cart
                cartItem = new CartModel
                {
                    ItemId = menuItem.Id,
                    Icon = menuItem.Icon,
                    Name = menuItem.Name,
                    Price = menuItem.Price,
                    Quantity = 1
                };
                CartItems.Add(cartItem);


            }
            else
            {
                //already exist, increment the quantity
                cartItem.Quantity++;


            }

        }
        [RelayCommand]
        private void IncreaseQuantity(CartModel cartItem)
        {
            cartItem.Quantity++;
            RecalculateAmounts();
        }
        [RelayCommand]
        private void DecreaseQuantity(CartModel cartItem)
        {
            cartItem.Quantity--;
            if (cartItem.Quantity == 0)
                CartItems.Remove(cartItem);
            else
                RecalculateAmounts();
        }

        [RelayCommand]
        private void RemoveItemFromCart(CartModel cartItem)
        {
            CartItems.Remove(cartItem);

        }

        private void RecalculateAmounts()
        {
            Subtotal = CartItems.Sum(c => c.Amount);
            /* if (AmountPaid >= Total)
             {
                 Change = AmountPaid - Total;
             }
             else
             {
                 Change = 0; // Prevent negative change
             }*/
        }

        [RelayCommand]
        private void UpdateAmountPaid(string input)
        {
            if (decimal.TryParse(input, out var parsedAmount))
            {
                AmountPaid = parsedAmount;
                RecalculateAmounts();
            }
            else
            {
                Shell.Current.DisplayAlert("Invalid Input", "Please enter a valid number.", "OK");
            }
        }

        [RelayCommand]
        private async Task TaxPercentageClickAsync()
        {
            var result = await Shell.Current.DisplayPromptAsync("Tax Percentage", "Enter applicable Tax percentage", placeholder: "10",
                initialValue: TaxPercentage.ToString());

            if (!string.IsNullOrWhiteSpace(result))
            {
                if (!int.TryParse(result, out int taxPercentage))
                {
                    await Shell.Current.DisplayAlert("InvalidValue", "Invalid Input", "OK");
                    return;
                }
                if (taxPercentage > 100)
                {
                    await Shell.Current.DisplayAlert("Invalid Value", "Please Enter not more than 100%", "OK");
                    return;
                }
                TaxPercentage = taxPercentage;
            }
        }

        /*[RelayCommand]
        private void Pay()
        {
            
            // Ensure calculations are done before resetting properties
            if (AmountPaid < Total)
            {
                // Display a warning if AmountPaid is less than Total
                Shell.Current.DisplayAlert("Insufficient Payment", "The amount paid is less than the total.", "OK");
                Change = 0; // No change if insufficient
                return;
            }

            // Calculate change only when the payment is valid
            Change = AmountPaid - Total;

            // Display the change
            Shell.Current.DisplayAlert("Payment Successful", $"Change: {Change:C}", "OK");

            // Clear the cart and reset values
            DonePaying();
        }*/

        private void DonePaying()
        {
            CartItems.Clear(); // Clear all cart items
            Subtotal = 0;      // Reset subtotal
            AmountPaid = 0;    // Clear amount paid

        }

        [RelayCommand]
        private async Task PlaceOrderAsync()
        {
            //IsBusy = true;

            if (AmountPaid < Total)
            {
                // Display a warning if AmountPaid is less than Total
                Shell.Current.DisplayAlert("Insufficient Payment", "The amount paid is less than the total.", "OK");
                Change = 0; // No change if insufficient
                return;
            }

            // Calculate change only when the payment is valid
            Change = AmountPaid - Total;

            // Display the change
            Shell.Current.DisplayAlert("Payment Successful", $"Change: ₱{Change:N2}", "OK");

            // Clear the cart and reset values
            await ordersViewModel.PlaceOrderAsync([.. CartItems]);
            DonePaying();
        }


        //MenuManagement view model

        [RelayCommand]
        private async Task EditMenuItemsAsync(MenuItem menuItem)
        {
            if (menuItem == null) return;

            // Set the selected menu item for editing
            SelectedMenuItem = menuItem;

            // Optionally display a message
            await Shell.Current.DisplayAlert("Edit Menu Item", $"Editing: {menuItem.Name}", "OK");
        }

        [RelayCommand]
        private void CancelEdit()
        {
            // Clear the selected menu item to reset the form
            SelectedMenuItem = null;
        }

        [RelayCommand]
        private async Task SaveMenuItemAsync()
        {
            if (SelectedMenuItem == null) return;

            // Save the updated menu item to the database
            await _databaseService.UpdateMenuItemAsync(SelectedMenuItem);
            await _databaseService.AddMenuItemAsync(SelectedMenuItem);

            // Clear the selection
            SelectedMenuItem = null;


            await Shell.Current.DisplayAlert("Success", "Menu item saved successfully!", "OK");


            // Refresh the menu items
            MenuItems = await _databaseService.GetMenuItemsByCategoryAsync(SelectedCategory.Id);

        }

        [RelayCommand]
        private async Task PickImageAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync();
                if (result != null)
                {
                    SelectedImagePath = result.FullPath;
                    SelectedMenuItem.Icon = SelectedImagePath; // Bind the image path to the menu item

                    // Debug log
                    Debug.WriteLine($"Picked Image Path: {SelectedImagePath}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Image picking failed: {ex.Message}");
            }
        }

    }
}
