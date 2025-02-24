using SoundBrewPOS.Models;
using SQLite;


namespace SoundBrewPOS.Data
{
    public class DatabaseService : IAsyncDisposable
    {
        private readonly SQLiteAsyncConnection _connection;
        public DatabaseService()
        {
            /* var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SoundBrewPOS.db");
             _connection = new SQLiteAsyncConnection(dbPath,
                 SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache); */
            _connection = new SQLiteAsyncConnection(
                 Path.Combine(FileSystem.AppDataDirectory, "SoundBrew.db1"),SQLiteOpenFlags.ReadWrite |
                 SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        }

        public async Task InitializeDatabaseAsync()
        {
            await _connection.CreateTableAsync<MenuCategory>();
            await _connection.CreateTableAsync<MenuItem>();
            await _connection.CreateTableAsync<MenuItemCategoryMapping>();
            await _connection.CreateTableAsync<Order>();
            await _connection.CreateTableAsync<OrderItem>();

            await SeedDataAsync();

            var menuItems = await GetMenuItemsByCategoryAsync(1);

           
           
        }

        private async Task SeedDataAsync()
        {
            // Check if there are already categories in the database
            var existingCategories = await _connection.Table<MenuCategory>().CountAsync();
            if (existingCategories == 0)
            {
                var categories = SeedData.GetMenuCategories();
                await _connection.InsertAllAsync(categories);
            }

            // Check if there are already menu items in the database
            var existingMenuItems = await _connection.Table<MenuItem>().CountAsync();
            if (existingMenuItems == 0)
            {
                var menuItems = SeedData.GetMenuItems();
                await _connection.InsertAllAsync(menuItems);
            }

            // Check if there are already mappings in the database
            var existingMappings = await _connection.Table<MenuItemCategoryMapping>().CountAsync();
            if (existingMappings == 0)
            {
                var mappings = SeedData.GetMenuItemCategoryMappings();
                await _connection.InsertAllAsync(mappings);
            }
        }

        public async Task<MenuCategory[]>GetMenuCategoriesAsync()=>
            await _connection.Table<MenuCategory>().ToArrayAsync();

        public async Task<MenuItem[]>GetMenuItemsByCategoryAsync(int categoryId)
        {
            var query = @"
                        SELECT menu.* 
                        FROM MenuItem AS menu
                        INNER JOIN MenuItemCategoryMapping as mapping ON menu.Id = mapping.MenuItemId
                        WHERE mapping.MenuCategoryId = ?
                        ";
            var menuItems = await _connection.QueryAsync<MenuItem>(query, categoryId);
            return [..menuItems];
        }

        public async Task<Order[]>GetOrdersAsync() => 
            await _connection.Table<Order>().ToArrayAsync();

        public async Task<string?> PlaceOrderAsync(OrderModel model)
        {
            var order = new Order
            {

                OrderDate = model.OrderDate,
                TotalAmountPaid = model.TotalAmountPaid,
                TotalItemsCount = model.TotalItemsCount,
            };
            if(await _connection.InsertAsync(order) > 0)
            {
                foreach (var items in model.Items)
                {
                    items.OrderId = order.Id;
                }
                if(await _connection.InsertAllAsync(model.Items) == 0)
                {
                    await _connection.DeleteAsync(order);
                    return "Error in Inserting order items";
                }
            }
            else
            {
                return "Error in inserting the order";
            }
            model.Id = order.Id;
            return null;
        }

        public async Task<OrderItem[]> GetOrderItemsAsync(long orderId) =>
            await _connection.Table<OrderItem>().Where(oi => oi.OrderId == orderId).ToArrayAsync();


        public async ValueTask DisposeAsync()
        {
            if (_connection != null)
                await _connection.CloseAsync();
        }


        public async Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            // Use the existing _connection object
            await _connection.UpdateAsync(menuItem); // Update the record in the SQLite database
        }

        public async Task AddMenuItemAsync(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            await _connection.InsertAsync(menuItem); // Insert the record into the SQLite database asynchronously
        }

        public async Task DeleteMenuItem(MenuItem menuItem)
        {
            if(menuItem == null) throw new ArgumentNullException(nameof( menuItem));

            await _connection.DeleteAsync(menuItem);
        }

        public async Task<MenuITemModel[]> GetAllMenuItemsAsync()
        {
            // Fetch all items from the database
            var items = await _connection.Table<MenuItem>().ToListAsync();

            // Project them into MenuItemModel objects
            return items.Select(item => new MenuITemModel
            {
                Name = item.Name,
                Price = item.Price,
                Description = item.Description,
                Icon = item.Icon
            }).ToArray();
        }

    }
}
