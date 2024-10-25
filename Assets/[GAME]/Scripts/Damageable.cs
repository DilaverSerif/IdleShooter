using System;
using UnityEngine;
namespace _GAME_.Scripts
{
    public enum LifeState
    {
        Alive,
        Dead
    }
    public abstract class Damageable : MonoBehaviour
    {
        public LifeState lifeState;
        protected float CurrentHealth;
        public float maxHealth;
        public abstract Side GetSide();
        protected virtual void Awake()
        {
            lifeState = LifeState.Alive;
            CurrentHealth = maxHealth;
        }

        public virtual void TakeDamage(float damage)
        {
            if (lifeState == LifeState.Dead) return;
            CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, maxHealth);
            
            if (CurrentHealth == 0)
            {
                Die();
            }
            else Hit();
        }
        
        protected virtual void Die()
        {
            lifeState = LifeState.Dead;
            Destroy(gameObject);
        }
        
        protected virtual void Hit()
        {
        }
        
        public virtual void Respawn()
        {
            lifeState = LifeState.Alive;
            CurrentHealth = maxHealth;
        }

        public bool Alive()
        {
            return lifeState == LifeState.Alive;
        }
        
    }
}