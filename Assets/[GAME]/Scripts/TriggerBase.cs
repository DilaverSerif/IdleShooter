using _GAME_.Scripts.Gun;
using UnityEngine;

namespace _GAME_.Scripts
{
    public abstract class TriggerBase : MonoBehaviour
    {
        public InventoryItem weaponType;
        private Collider _collider;
        
        protected virtual void Awake()
        {
            _collider = GetComponent<Collider>();
        }
        
        protected abstract void PlayerTrigger(PlayerInventory playerInventory);
        
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerInventory playerInventory)) return;
            
            PlayerTrigger(playerInventory);
            Destroy(gameObject);
        }

        public void OpenTrigger(bool open = true)
        {
            _collider.enabled = open;
        }
    }
}