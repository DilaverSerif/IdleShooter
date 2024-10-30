using System;
using UnityEngine;
namespace _GAME_.Scripts.Gun
{
    public struct SpawnAlertAreaData
    {
        public AlertAreaType alertAreaType;
        public float duration;
        public Vector3 position;
        public Quaternion rotation;
        public float delay;
        public Action onSpawn;

        public SpawnAlertAreaData(AlertAreaType alertAreaType, float duration, Vector3 position,Quaternion rotation, float delay = 0, Action onSpawn = null)
        {
            this.alertAreaType = alertAreaType;
            this.duration = duration;
            this.position = position;
            this.delay = delay;
            this.onSpawn = onSpawn;
            this.rotation = rotation;
        }

        public SpawnAlertAreaData(SpawnAlertAreaData data)
        {
            alertAreaType = data.alertAreaType;
            duration = data.duration;
            position = data.position;
            delay = data.delay;
            onSpawn = data.onSpawn;
            rotation = data.rotation;
        }
    }
}