using _GAME_.Scripts.Gun;
using UnityEngine;

namespace _GAME_.Scripts
{
    public class WeaponTrigger : MonoBehaviour
    {
        public InventoryItem weaponType;
        private Collider _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerInventory playerInventory)) return;
            
            playerInventory.playerEquipment.EquipGun(weaponType);
            Destroy(gameObject);
        }

        public void OpenTrigger(bool open = true)
        {
            _collider.enabled = open;
        }
     
    }
}