using UnityEngine;
using UnityEngine.Serialization;
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

        [SerializeField]
        private ParticleSystem bulletDecalParticle;
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
        protected virtual void SpawnDecalEffect(ContactPoint contact)
        {
            Vector3 hitPosition = contact.point;
            Vector3 hitNormal = contact.normal;
                
            Vector3 spawnPosition = hitPosition + hitNormal * offsetSpawn;

            bulletDecalParticle.transform.position = spawnPosition;
            bulletDecalParticle.transform.forward = hitNormal;
            bulletDecalParticle.transform.SetParent(null);
            bulletDecalParticle.Play();
        }

        public float offsetSpawn;
        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.TryGetComponent(out Damageable damageable))
            {
                if(bulletDecalParticle) SpawnDecalEffect(other.contacts[0]);
                damageable.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    
    }
}