using System;
using Sirenix.OdinInspector;
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
        [ShowInInspector,ReadOnly]
        protected float CurrentHealth;
        
        public LifeState lifeState;
        public float maxHealth;
        
        public Action OnDie;
        public Action OnHit;
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
                Die();
            
            Hit();
        }
        
        protected virtual void Die()
        {
            OnDie?.Invoke();
            lifeState = LifeState.Dead;
        }
        
        protected virtual void Hit()
        {
            OnHit?.Invoke();
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
        
        public float GetCurrentHealth()
        {
            return CurrentHealth;
        }
    }
}