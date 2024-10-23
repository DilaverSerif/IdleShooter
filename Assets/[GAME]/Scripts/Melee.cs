using System;
using System.Threading;
using _GAME_.Scripts.Gun;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _GAME_.Scripts
{
    public class Melee : Damager
    {
        public float attackRange;
        public float attackAngle;
        
        private int _hitIndex;
        private Transform _target;
        private CancellationTokenSource _autoFireToken;
        private void OnEnable()
        {
            _hitIndex = 0;
            canAttack.Add(CanHit);
        }

        private void OnDisable()
        {
            _autoFireToken?.Cancel();
        }

        private bool CanHit()
        {
            return _target && 
                   Vector3.Distance(_target.position, owner.position) <= attackRange &&
                   Vector3.Angle(_target.position - owner.position, owner.forward) <= attackAngle;
        }

        public override async UniTask AutoFire(Damageable target)
        {
            _autoFireToken?.Cancel();
            _autoFireToken = new CancellationTokenSource();
            _target = target.transform;
            
            while (_autoFireToken.Token.IsCancellationRequested == false)
            {
                OnFireBullet?.Invoke(_hitIndex);
                target.TakeDamage(weaponLevel.Damage);
                await UniTask.WaitForSeconds(weaponLevel.FireRate);
            }
        }

        void OnDrawGizmosSelected()
        {
            if(owner == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(owner.position, attackRange);
            Gizmos.DrawRay(owner.position, owner.forward * attackRange);
            Gizmos.DrawRay(owner.position, Quaternion.Euler(0, attackAngle, 0) * owner.forward * attackRange);
            Gizmos.DrawRay(owner.position, Quaternion.Euler(0, -attackAngle, 0) * owner.forward * attackRange);
        }
    }
}