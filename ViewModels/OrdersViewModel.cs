using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SoundBrewPOS.Data;
using SoundBrewPOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundBrewPOS.ViewModels
{
    public partial class OrdersViewModel :ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public OrdersViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }


        public ObservableCollection<OrderModel> Orders { get; set; } = [];

        private bool _isInitialized;

        [ObservableProperty]
        private bool _isLoading;
        public async ValueTask InitializedAsync()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            IsLoading = true;
            var dbOrders = await _databaseService.GetOrdersAsync();
            var orders = dbOrders.Select(o => new OrderModel
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmountPaid = o.TotalAmountPaid,
                TotalItemsCount = o.TotalItemsCount,

            });
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
            IsLoading = false;
        }

        [ObservableProperty]
        private OrderItem[] _orderItems;

        [RelayCommand]
        public async Task SelectOrderAsync(OrderModel? order)
        {
            var preSelectedOrder = Orders.FirstOrDefault(o=> o.IsSelected);
            if(preSelectedOrder != null)
            {
                preSelectedOrder.IsSelected = false;
            }
            if (order == null || order.Id == 0)
            {
                OrderItems = [];
                return;
            }
            order.IsSelected = true;
            IsLoading = true;
            OrderItems = await _databaseService.GetOrderItemsAsync(order.Id);
            IsLoading = false;
        }
        
        public async Task PlaceOrderAsync(CartModel[] cartItems)
        {
            var orderItems = cartItems.Select(c => new OrderItem
            {
                Icon = c.Icon,
                ItemId = c.ItemId,
                Name = c.Name,
                Price = c.Price,
                Quantity = c.Quantity,
            }).ToArray();
            var orderModel = new OrderModel
            {
                OrderDate = DateTime.Now,
                TotalAmountPaid = cartItems.Sum(c => c.Amount),
                TotalItemsCount = cartItems.Length,
                Items = orderItems
            };

            var errorMessage = await _databaseService.PlaceOrderAsync(orderModel);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                //failed

                await Shell.Current.DisplayAlert("Error", errorMessage, "Ok");
                return;
            }
            Orders.Add(orderModel);
             
        }
    }
}
