using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME_.Scripts.Gun
{
    [Serializable]
    public struct GunIKData
    {
        [GUIColor(0.5f, 0.5f, 1)]
        public Vector3 TargetPosition;
        [GUIColor(0.5f, 1, 0.5f)]
        public Vector3 TargetRotation;
        
        [GUIColor(0.5f, 0.5f, 1)]
        public Vector3 HintPosition;
        [GUIColor(0.5f, 1, 0.5f)]
        public Vector3 HintRotation;
    }
}