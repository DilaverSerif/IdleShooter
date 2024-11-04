using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
namespace _GAME_.Scripts
{
    [Serializable]
    public class PlayerSave
    {
        public PlayerLevel playerLevel = new PlayerLevel();
        
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

