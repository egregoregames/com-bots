using System.Linq;

public partial class PersistentGameData
{
    public static class Inventory
    {
        /// <summary>
        /// Add an item to the player's inventory. Will throw a warning if you 
        /// exceed the item's max quantity. This is very undesirable. 
        /// Check <see cref="StaticGameData.GetMaxInventoryItemQuantity(int)"/> 
        /// and <see cref="GetAmount(int)"/> first
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount">How many of the item to add. Will throw a warning 
        /// if you exceed the item's max quantity. This is very undesirable. 
        /// Check <see cref="StaticGameData.GetMaxInventoryItemQuantity(int)"/> 
        /// and <see cref="GetAmount(int)"/> first
        /// </param>
        public static async void AddItem(int itemId, int amount)
        {
            var instance = await GetInstanceAsync();

            var item = instance.PlayerInventoryItemData
                .FirstOrDefault(x => x.ItemId == itemId);

            if (item == null)
            {
                item = new InventoryItemDatum()
                {
                    ItemId = itemId
                };

                instance.PlayerInventoryItemData.Add(item);
            }

            var staticData = await item.GetStaticDataAsync();
            item.Quantity += amount;

            if (staticData.MaxQuantity < item.Quantity)
            {
                item.Quantity = staticData.MaxQuantity;

                var message = $"Tried to add too many of item type " +
                    $"{staticData.ItemName} to player inventory";

                Instance.Log(message, LogLevel.Warning);
            }

            _onInventoryItemUpdated?.Invoke(item);
        }

        /// <summary>
        /// Remove an item from the player's inventory. Will log a warning if the 
        /// end quantity is lower than 0. This would be very undsirable. 
        /// Check <see cref="GetAmount"/> first
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="amount">The amount to remove. Will log a warning if the 
        /// end quantity is lower than 0. This would be very undsirable. 
        /// Check <see cref="GetAmount"/> first</param>
        public static async void RemoveItem(int itemId, int amount)
        {
            var instance = await GetInstanceAsync();

            var item = instance.PlayerInventoryItemData
                .FirstOrDefault(x => x.ItemId == itemId);

            if (item == null)
            {
                string message = $"Tried to remove an item from the player's " +
                    $"inventory (ID: {itemId}) that did not exist";

                Instance.Log(message, LogLevel.Warning);
                return;
            }

            item.Quantity -= amount;

            if (item.Quantity < 0)
            {
                string message = $"Took too many of item {item.ItemId} from " +
                    $"player's inventory";

                Instance.Log(message, LogLevel.Warning);

                item.Quantity = 0;
            }

            _onInventoryItemUpdated?.Invoke(item);
        }

        /// <returns>An integer representing how many of an itemId the user has</returns>
        public static int GetAmount(int itemId)
        {
            var item = Instance.PlayerInventoryItemData
                .FirstOrDefault(x => x.ItemId == itemId);

            if (item == null)
            {
                return 0;
            }

            return item.Quantity;
        }
    }
}