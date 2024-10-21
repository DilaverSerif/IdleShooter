using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _GAME_.Scripts.Gun
{
    public abstract class Damager : MonoBehaviour
    {
        public Transform owner;
        public Action OnFireBullet;
        private CancellationTokenSource _cancellationTokenSource;
        
        [BoxGroup("Generally")]
        public InventoryItem inventoryItem;
        [BoxGroup("Generally")]
        public WeaponType weaponType;
        
        public WeaponLevel weaponLevel;
        
        public delegate bool CanAttack();
        public List<CanAttack> canAttack = new List<CanAttack>();
        public abstract UniTask Fire(Damageable target);
    }
}