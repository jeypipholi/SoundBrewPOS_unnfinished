
using CommunityToolkit.Mvvm.ComponentModel;
using SoundBrewPOS.Data;

namespace SoundBrewPOS.Models
{
    public partial class MenuITemModel : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private decimal _price;

        [ObservableProperty]
        private string _icon;

        [ObservableProperty]
        private string _description;

      

        


    }
}
