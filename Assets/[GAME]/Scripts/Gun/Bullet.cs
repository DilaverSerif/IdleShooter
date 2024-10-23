using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME_.Scripts.Gun
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Bullet<T> : BulletBase
    {
        protected T CurrentTarget;
    }

    public abstract class BulletBase : MonoBehaviour
    {
        public int damage;
        public InventoryItem inventoryItem;
        public Vector2 bulletRangeMinMax;
        public Vector2 bulletLifeTimeMinMax;
    
        protected Rigidbody Rigidbody;
        protected Collider BaseCollider;
        protected float BulletRange()
        {
            return Random.Range(bulletRangeMinMax.x, bulletRangeMinMax.y);
        }

        protected float BulletLifeTime()
        {
            return Random.Range(bulletLifeTimeMinMax.x, bulletLifeTimeMinMax.y);
        }
    
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            BaseCollider = GetComponent<Collider>();
        }
    
        public abstract void Fire(float extraDamage = 0);

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out Damageable damageable))
            {
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    
    }
}