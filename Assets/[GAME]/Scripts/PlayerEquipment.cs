using System;
using _GAME_.Scripts.Gun;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace _GAME_.Scripts
{
    [Serializable]
    public class PlayerEquipment
    {
        public Gun.Gun currentGun;
        public static Action<Gun.Gun> OnGunEquipped;
        public InventoryItem lastGun;
        
        public void EquipGun(Gun.Gun gun)
        {
            if (currentGun != null)
            {
                currentGun.OnFireBullet = null;
                lastGun = currentGun.inventoryItem;
            }
            currentGun = gun;
            
            OnGunEquipped?.Invoke(gun);
        }
        
        public void EquipGun(InventoryItem gun)
        {
            if (currentGun != null)
                lastGun = currentGun.inventoryItem;

            currentGun = Object.Instantiate(InventoryData.Instance.GetGun(gun));
            
            OnGunEquipped?.Invoke(currentGun);
        }
        
        public void UnequipGun()
        {
            lastGun = currentGun.inventoryItem;
            currentGun = null;
        }
    }
}