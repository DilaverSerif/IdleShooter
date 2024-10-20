using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME_.Scripts.Gun
{
    [Serializable]
    public class Magazine
    {
        public bool infinite;
        [ShowIf("infinite",false)]
        public int capacity;
        [ShowInInspector]
        private int _currentCapacity;
        public float reloadTime;
    
        public int CurrentCapacity
        {
            get => _currentCapacity;
            set => _currentCapacity = Mathf.Clamp(value, 0, capacity);
        }

        public bool HaveBullet()
        {
            if (infinite) return true;
            return capacity > 0;
        }
    
        public void GizmosSelected(Transform firePoint)
        {
#if UNITY_EDITOR
            var magazineText = "Current Bullet:" + capacity +"/"+ CurrentCapacity;
            if(infinite) magazineText = "Current Bullet: ∞";
#endif
        
        }
    }
}