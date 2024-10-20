using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME_.Scripts.Gun
{
    public enum WeaponType
    {
        Handgun = 0,
        Rifle = 1,
        Shotgun = 2,
        Sniper = 3,
        MachineGun = 4,
        RocketLauncher = 5,
        GrenadeLauncher = 6,
        Melee = 7,
    }
    
    public class Gun : MonoBehaviour
    {
        public Action OnFireBullet;
        
        [BoxGroup("Generally")]
        public InventoryItem inventoryItem;
        [BoxGroup("Generally")]
        public ParticleSystem muzzleFlash;
        [BoxGroup("Generally")]
        public WeaponType weaponType;
        
        public Transform firePoint;
        public Magazine magazine;
        public GunBarrel gunBarrel;
        public WeaponLevel weaponLevel;
        public delegate bool CanAttack();
        public List<CanAttack> canAttack = new List<CanAttack>();
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(firePoint.position, 0.025f);
        }

        private void OnDrawGizmosSelected()
        {
            gunBarrel.OnGizmosSelected(firePoint);
            magazine.GizmosSelected(firePoint);
        }
        
        private bool _isFiring;
        private async UniTaskVoid SpawnBullets()
        {
            _isFiring = true;
            var bulletCount = gunBarrel.perShootingBulletCount;
            var getBulletObject = gunBarrel.bullet;
            
            var forwards = gunBarrel.GetForwardRotations();
            
            for (var i = 0; i < bulletCount; i++)
            {
                var spawnedBullet = Instantiate(getBulletObject, firePoint.position, forwards[i] * transform.rotation);
                spawnedBullet.Fire(weaponLevel.Damage);
                if(weaponLevel.waitPerFireBullet > 0)
                    await UniTask.WaitForSeconds(weaponLevel.waitPerFireBullet);
                if (!(weaponLevel.waitPerFireBullet > 0)) continue;
                
                if(muzzleFlash)
                    muzzleFlash.Play();
                OnFireBullet?.Invoke();
            }
            
            await UniTask.WaitForSeconds(weaponLevel.FireRate);
            
            _isFiring = false;
        }

        public virtual void Fire()
        {
            if(_isFiring) return;
            if(!magazine.HaveBullet()) return;
            if(canAttack.Count > 0)
                if(!canAttack.All(x => x()))
                    return;
            
            SpawnBullets().Forget();
 
            if (weaponLevel.waitPerFireBullet == 0)
            {
                if(muzzleFlash)
                    muzzleFlash.Play();
                OnFireBullet?.Invoke();
            }
        }

#if UNITY_EDITOR
        [Button]
        private void FindFirePoint()
        {
            if (firePoint == null)
            {
                firePoint = transform.Find("FirePoint");
            }
            
            if (firePoint == null)
            {
                firePoint = new GameObject("FirePoint").transform;
                firePoint.SetParent(transform);
                firePoint.localPosition = Vector3.zero;
            }
        }
        
        [Button]
        private void FindMuzzleFlash()
        {
            muzzleFlash = transform.FindChildObjectByName("MuzzleFlash").GetComponent<ParticleSystem>();
        }
#endif
    }
}