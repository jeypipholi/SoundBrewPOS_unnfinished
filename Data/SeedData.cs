using System;
using System.Collections.Generic;

namespace SoundBrewPOS.Data
{
    public static class SeedData
    {
        public static List<MenuCategory> GetMenuCategories()
        {
            return new List<MenuCategory>
            {
                new MenuCategory { Id = 1, Name = "Coffee", Icon = "coffee_cup.png" },
                new MenuCategory { Id = 2, Name = "Snacks and Bakery", Icon = "donut.png" }
            };
        }

        public static List<MenuItem> GetMenuItems()
        {
            return new List<MenuItem>
            {
                // Coffee
                new MenuItem { Id = 1, Name = "Americano", Description = "A bold and robust black coffee.", Icon = "americano.jpg", Price = 59.00m },
                new MenuItem { Id = 2, Name = "Cappuccino", Description = "Espresso topped with creamy milk foam.", Icon = "cappuccino.jpg", Price = 59.00m },
                new MenuItem { Id = 3, Name = "Caramel Macchiato", Description = "Sweet espresso with a touch of caramel.", Icon = "caramel_machiato.jpg", Price = 59.00m },
                new MenuItem { Id = 4, Name = "Salted Caramel", Description = "A savory twist on a sweet classic.", Icon = "salted_caramel.jpg", Price = 59.00m },
                new MenuItem { Id = 5, Name = "Spanish Latte", Description = "A smooth latte sweetened with condensed milk.", Icon = "spanish_latte.jpg", Price = 59.00m },
                new MenuItem { Id = 6, Name = "Hazelnut Latte", Description = "A creamy latte with a nutty hazelnut flavor.", Icon = "hazelnut.jpg", Price = 59.00m },
                new MenuItem { Id = 7, Name = "Mocha", Description = "A delightful mix of chocolate and coffee.", Icon = "mocha.jpg", Price = 59.00m },
                new MenuItem { Id = 8, Name = "White Mocha", Description = "Rich white chocolate blended with espresso.", Icon = "white_mocha.jpg", Price = 59.00m },

                // Snacks and Bakery
                new MenuItem { Id = 9, Name = "Ham and Cheese", Description = "A classic sandwich with ham and melted cheese.", Icon = "ham_cheese.jpg", Price = 30.00m },
                new MenuItem { Id = 10, Name = "Egg Sandwich", Description = "A fluffy egg-filled sandwich, perfect for breakfast.", Icon = "egg_sandwich.jpg", Price = 30.00m },
                new MenuItem { Id = 11, Name = "Grilled Cheese", Description = "Golden and crispy grilled cheese sandwich.", Icon = "grilled_cheese.jpg", Price = 30.00m }
            };
        }

        public static List<MenuItemCategoryMapping> GetMenuItemCategoryMappings()
        {
            return new List<MenuItemCategoryMapping>
            {
                // Coffee Category
                new MenuItemCategoryMapping { MenuCategoryId = 1, MenuItemId = 1 },
                new MenuItemCategoryMapping { MenuCategoryId = 1, MenuItemId = 2 },
                new MenuItemCategoryMapping { MenuCategoryId = 1, MenuItemId = 3 },
                new MenuItemCategoryMapping { MenuCategoryId = 1, MenuItemId = 4 },
                new MenuItemCategoryMapping { MenuCategoryId = 1, MenuItemId = 5 },
                new MenuItemCategoryMapping { MenuCategoryId = 1, MenuItemId = 6 },
                new MenuItemCategoryMapping { MenuCategoryId = 1, MenuItemId = 7 },
                new MenuItemCategoryMapping { MenuCategoryId = 1, MenuItemId = 8 },

                // Snacks and Bakery Category
                new MenuItemCategoryMapping { MenuCategoryId = 2, MenuItemId = 9 },
                new MenuItemCategoryMapping { MenuCategoryId = 2, MenuItemId = 10 },
                new MenuItemCategoryMapping { MenuCategoryId = 2, MenuItemId = 11 }
            };
        }
    }
}
