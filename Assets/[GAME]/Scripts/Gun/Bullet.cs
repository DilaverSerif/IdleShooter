using Sirenix.OdinInspector;
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
        [BoxGroup("Generally")]
        public float damage;
        [BoxGroup("Generally")]
        public InventoryItem inventoryItem;
        
        [BoxGroup("Particle")]
        public float offsetSpawn;
        [SerializeField,BoxGroup("Particle")]
        private ParticleSystem bulletDecalParticle;
        
        protected Rigidbody Rigidbody;
        protected Collider BaseCollider;
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            BaseCollider = GetComponent<Collider>();
        }
    
        public abstract void Fire(BulletFireData bulletFireData = default);
        protected virtual void SpawnDecalEffect(ContactPoint contact)
        {
            var hitPosition = contact.point;
            var hitNormal = contact.normal;
                
            var spawnPosition = hitPosition + hitNormal * offsetSpawn;

            var particleTransform = bulletDecalParticle.transform;
            particleTransform.position = spawnPosition;
            particleTransform.forward = hitNormal;
            
            bulletDecalParticle.transform.SetParent(null);
            bulletDecalParticle.Play();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent(out Damageable damageable)) return;
            Debug.Log("Bullet hit " + damageable.name);
            if(bulletDecalParticle) SpawnDecalEffect(other.contacts[0]);
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    
    }
}