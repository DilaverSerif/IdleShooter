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
        Handgun = 1,
        Rifle = 2,
        Shotgun = 3,
        Sniper = 4,
        MachineGun = 5,
        RocketLauncher = 6,
        GrenadeLauncher = 7,
        Melee = 0,
    }
    
    public enum GunState
    {
        Idle = 0,
        Fire = 1,
        Reload = 2,
    }
    
    public class Gun : Damager
    {
        [BoxGroup("Generally")]
        public GunIKData leftHandIK;
        [BoxGroup("Generally")]
        public GunIKData rightHandIK;
        
        public Magazine magazine;
        public GunBarrel gunBarrel;
        [BoxGroup("Generally")]
        public ParticleSystem muzzleFlash;
        [BoxGroup("Generally")]
        public Transform firePoint;
        [BoxGroup("Generally")]
        public GunState gunState;
        
        
        public List<GunPlugin> weaponPlugins = new();

        protected CancellationTokenSource _autoFireToken;
        private void OnDrawGizmos()
        {
            if(firePoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(firePoint.position, 0.025f);
        }

        private void OnDrawGizmosSelected()
        {
            if(firePoint == null) return;
            gunBarrel.OnGizmosSelected(firePoint);
            magazine.GizmosSelected(firePoint);
        }

        protected virtual async UniTask SpawnBullets(CancellationToken token = default, Transform target = null)
        {
            gunState = GunState.Fire;
            var bulletCount = gunBarrel.perShootingBulletCount;
            var getBulletObject = gunBarrel.bullet;
            
            var forwards = gunBarrel.GetForwardRotations();
            
            for (var i = 0; i < bulletCount; i++)
            {
                var spawnedBullet = Instantiate(getBulletObject, firePoint.position, forwards[i] * transform.rotation);
                spawnedBullet.Fire(new BulletFireData(targetPosition: target ? target.position : default
                , throwSpeed:gunBarrel.bulletSpeed, bulletDamage:weaponLevel.Damage));
                
                if(muzzleFlash)
                    muzzleFlash.Play();
                OnFireBullet?.Invoke(i);
                
                if(gunBarrel.waitPerFireBullet > 0)
                    await UniTask.WaitForSeconds(gunBarrel.waitPerFireBullet, cancellationToken: token);
            }
            
            await UniTask.WaitForSeconds(weaponLevel.FireRate, cancellationToken: token);
            
            gunState = GunState.Idle;
        }

        protected bool IsFar(Transform target)
        {
            return Vector3.Distance(firePoint.position, target.position) > gunBarrel.range;
        }
        
        private void OnEnable()
        {
            canAttack.Add(() => gunState == GunState.Idle);
            canAttack.Add(() => magazine.HaveBullet());
        }

        private void OnDisable()
        {
            canAttack.Clear();
            _autoFireToken?.Cancel();
        }

        public override async UniTask AutoFire(Damageable target)
        {
            Debug.Log("AutoFire Started");
            _autoFireToken?.Cancel();
            _autoFireToken = new CancellationTokenSource();
            
            await UniTask.WaitForSeconds(.25f, cancellationToken: _autoFireToken.Token);
            
            while (_autoFireToken.Token.IsCancellationRequested == false)
            {
                if(canAttack.Count > 0)
                    if (!canAttack.All(x => x()))
                    {
                        Debug.Log("Can't Attack");
                        await UniTask.NextFrame(cancellationToken: _autoFireToken.Token);
                        continue;
                    }
                if (target && IsFar(target.transform))
                {
                    Debug.Log("Target is far away");
                    await UniTask.NextFrame(cancellationToken: _autoFireToken.Token);
                    continue;
                }

                await SpawnBullets(_autoFireToken.Token, target ? target.transform : null);
            }
        }
        
        public void AddPlugin(InventoryItem plugin)
        {
            var pluginData = InventoryData.Instance.GetWeaponPlugin(plugin);
            weaponPlugins.Add(pluginData);
            pluginData.Effect(this);
            
            pluginData.transform.SetParent(transform);
            pluginData.transform.localPosition = Vector3.zero;
            pluginData.transform.localRotation = Quaternion.identity;
        }
        
        public void RemovePlugin(InventoryItem plugin)
        {
            var pluginData = weaponPlugins.Find(x => x.pluginType == plugin);
            weaponPlugins.Remove(pluginData);
            pluginData.UnEffect(this);
            Destroy(pluginData.gameObject);
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

        [Button]
        private void TestFire()
        {
            if (Application.isPlaying)
            {
                SpawnBullets().Forget();
            }
        }
        
        [Button]
        private void AutoTestFire()
        {
            if (Application.isPlaying)
            {
                AutoFire(null).Forget();
            }
        }
#endif
        public void StopFire()
        {
            _autoFireToken?.Cancel();
            gunState = GunState.Idle;
        }
    }
}