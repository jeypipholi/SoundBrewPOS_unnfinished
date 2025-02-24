using SoundBrewPOS.ViewModels;

namespace SoundBrewPOS;

public partial class OrdersPage : ContentPage
{
	private readonly OrdersViewModel _ordersViewModel;
	public OrdersPage(OrdersViewModel ordersViewModel)
	{
		InitializeComponent();
		_ordersViewModel = ordersViewModel;
		BindingContext = _ordersViewModel;
		InitializedViewModelaAsync();

    }
	private async void InitializedViewModelaAsync()
	{
		await _ordersViewModel.InitializedAsync();
	}
}