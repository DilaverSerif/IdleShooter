using UnityEngine;
namespace _GAME_.Scripts
{
    public abstract class Damageable : MonoBehaviour
    {
        private float _currentHealth;
        public float maxHealth;
    
        public abstract Side GetSide();
        public virtual void TakeDamage(float damage)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
        }
    }
}