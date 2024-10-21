using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME_.Scripts.Gun
{
    [Serializable]
    public class WeaponLevel
    {
        [BoxGroup("Weapon")]
        public AnimationCurve damageCurve;
        [BoxGroup("Weapon")]
        public AnimationCurve fireRateCurve;
        
        [Header("Level"),BoxGroup("Weapon")]
        public int weaponLevel;
        [BoxGroup("Weapon")]
        public float waitPerFireBullet;
        [ShowInInspector]
        public float Damage => damageCurve.Evaluate(weaponLevel);
        [ShowInInspector]
        public float FireRate => fireRateCurve.Evaluate(weaponLevel);
    }
}