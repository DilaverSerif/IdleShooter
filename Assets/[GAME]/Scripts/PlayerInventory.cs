using System;
using System.Collections.Generic;
using _GAME_.Scripts.Gun;
using UnityEngine;
using UnityEngine.Serialization;

namespace _GAME_.Scripts
{
    [Serializable]
    public struct InventoryItemData
    {
        public InventoryItem item;
        public int count;
    }
    
    public class PlayerInventory : MonoBehaviour
    {
        public PlayerEquipment playerEquipment;
        public List<InventoryItemData> inventoryItems;
        
        public void AddItem(InventoryItem item, int count)
        {
            var itemData = inventoryItems.Find(x => x.item == item);
            itemData.count += count;
        }
        
        public void RemoveItem(InventoryItem item, int count)
        {
            var itemData = inventoryItems.Find(x => x.item == item);
            itemData.count -= count;
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