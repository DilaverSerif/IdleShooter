using _GAME_.Scripts.Gun;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME_.Scripts
{
    [CreateAssetMenu(menuName = "Data/WeaponData")]
    public class WeaponData: ScriptableObject
    {
        [BoxGroup("Generally")]
        public Damager damager;
        [BoxGroup("Generally")]
        public Sprite icon;
    
        [BoxGroup("Animation")]
        public GunIKData leftHandIK;
        [BoxGroup("Animation")]
        public GunIKData rightHandIK;
    
        [BoxGroup("Animation")]
        public Vector3 weaponPosition;
        [BoxGroup("Animation")]
        public Vector3 weaponRotation;
        [BoxGroup("Animation")]
        public Vector3 weaponScale;
    }
}