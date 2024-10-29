using System;
using UnityEngine;

namespace _GAME_.Scripts.Gun
{
    [Serializable]
    public abstract class GunPlugin: MonoBehaviour
    {
        public InventoryItem pluginType;
        public abstract void Effect(Gun gun);
        public abstract void UnEffect(Gun gun);
    }
}