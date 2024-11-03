using System;
using System.Collections.Generic;
namespace _GAME_.Scripts
{
    [Serializable]
    public class PlayerSave
    {
        public List<InventoryItemData> inventoryItemData = new List<InventoryItemData>();
        public List<UpgradeSaveData> upgrades = new List<UpgradeSaveData>();
    }
    
    [Serializable]
    public class UpgradeSaveData
    {
        public StatTags statTag;
        public int level;
    }

}

