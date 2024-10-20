using _GAME_.Scripts.Gun;
using UnityEngine;

namespace _GAME_.Scripts
{
    public class WeaponTrigger : MonoBehaviour
    {
        public InventoryItem weaponType;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerInventory playerInventory))
            {
                playerInventory.AddItem(weaponType, 1);
                playerInventory.PlayerEquipment.EquipGun(weaponType);
                Destroy(gameObject);
            }
        }
    }
}