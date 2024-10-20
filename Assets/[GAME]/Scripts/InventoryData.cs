using System.Collections.Generic;
using _BASE_.Scripts;
using _GAME_.Scripts.Gun;
using UnityEngine;

namespace _GAME_.Scripts
{
    [CreateAssetMenu(menuName = "Create InventoryData", fileName = "InventoryData", order = 0)]
    public class InventoryData : SingletonScriptableObject<InventoryData> 
    {
        public List<Gun.Gun> guns;
        
        public Gun.Gun GetGun(InventoryItem item)
        {
            return guns.Find(gun => gun.inventoryItem == item);
        }
    }
}