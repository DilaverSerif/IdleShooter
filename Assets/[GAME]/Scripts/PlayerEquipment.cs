using System;
using _GAME_.Scripts.Gun;
using Object = UnityEngine.Object;

namespace _GAME_.Scripts
{
    public class PlayerEquipment
    {
        public Gun.Gun CurrentGun;
        public static Action<Gun.Gun> OnGunEquipped;
        public InventoryItem lastGun;
        
        public void EquipGun(Gun.Gun gun)
        {
            if (CurrentGun != null)
            {
                CurrentGun.OnFireBullet = null;
                lastGun = CurrentGun.inventoryItem;
            }
            CurrentGun = gun;
            
            OnGunEquipped?.Invoke(gun);
        }
        
        public void EquipGun(InventoryItem gun)
        {
            if (CurrentGun != null)
                lastGun = CurrentGun.inventoryItem;

            CurrentGun = Object.Instantiate(InventoryData.Instance.GetGun(gun));
            
            OnGunEquipped?.Invoke(CurrentGun);
        }
        
        public void UnequipGun()
        {
            lastGun = CurrentGun.inventoryItem;
            CurrentGun = null;
        }
    }
}