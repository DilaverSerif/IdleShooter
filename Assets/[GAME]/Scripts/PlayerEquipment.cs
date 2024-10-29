using System;
using _GAME_.Scripts.Gun;
using UnityEngine;
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
        public Transform gunHolder;
        public void StopFire()
        {
            if (currentGun != null)
                currentGun.StopFire();
        }
        
        public void EquipGun(InventoryItem gun)
        {
            if (currentGun != null)
                UnEquipGun();

            var gunData = InventoryData.Instance.GetGun(gun);
            currentGun = Object.Instantiate(gunData.damager as Gun.Gun);
            var currentGunTransform = currentGun.transform;
            currentGunTransform.SetParent(null);
            currentGunTransform.SetParent(gunHolder);

            currentGunTransform.localScale = gunData.weaponScale;
            currentGunTransform.localPosition = gunData.weaponPosition;
            currentGunTransform.localRotation = Quaternion.Euler(gunData.weaponRotation);
         
            OnGunEquipped?.Invoke(currentGun);
        }
        
        public void UnEquipGun()
        {
            currentGun.StopFire();
            lastGun = currentGun.inventoryItem;
            Object.Destroy(currentGun.gameObject);
            currentGun = null;
        }

    }
}