using System;
using System.Collections.Generic;
using _BASE_.Scripts;
using _GAME_.Scripts.Gun;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts
{
    [Serializable]
    public class InventoryItemData
    {
        public InventoryItem item;
        public int count;
    }
    
    public class PlayerInventory : MonoBehaviour
    {
        public static Action<InventoryItem> OnItemAdded;
        public PlayerEquipment playerEquipment;
        public List<InventoryItemData> inventoryItems;
        
        public void AddItem(InventoryItem item, int count)
        {
            var itemData = inventoryItems.Find(x => x.item == item);
            itemData.count += count;
            
            OnItemAdded?.Invoke(item);
        }
        
        public bool HasItem(InventoryItem item)
        {
            var itemData = inventoryItems.Find(x => x.item == item);
            return itemData.count > 0;
        }

        public void AddEquipment(InventoryItem weaponType)
        {
            AddItem(weaponType, 1);
        }
      
    }
}