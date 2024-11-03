using System.Linq;
using _GAME_.Scripts.Gun;
namespace _GAME_.Scripts
{
    public static class PlayerSaveExtension
    {
        public static int GetUpgradeLevel(this PlayerSave playerSave, StatTags statTag)
        {
            for (int i = 0; i < playerSave.upgrades.Count; i++)
            {
                if (playerSave.upgrades[i].statTag == statTag)
                {
                    return playerSave.upgrades[i].level;
                }
            }
            return 0;
        }

        public static int GetGold(this PlayerSave playerSave)
        {
            var firstOrDefault = playerSave.inventoryItemData.FirstOrDefault(x => x.item == InventoryItem.Gold);
            return firstOrDefault?.count ?? 0;
        }

        public static void AddCoin(this PlayerSave playerSave, int count = 1)
        {
            var firstOrDefault = playerSave.inventoryItemData.FirstOrDefault(x => x.item == InventoryItem.Gold);
            if (firstOrDefault == null)
            {
                playerSave.inventoryItemData.Add(new InventoryItemData
                {
                    item = InventoryItem.Gold,
                    count = count
                });
            }
            else
            {
                firstOrDefault.count += count;
            }
            
            PlayerInventory.OnItemAdded?.Invoke(InventoryItem.Gold);
        }

        public static int GetItemAmount(this PlayerSave playerSave, InventoryItem item)
        {
            var firstOrDefault = playerSave.inventoryItemData.FirstOrDefault(x => x.item == item);
            return firstOrDefault?.count ?? 0;
        }
    }
}
