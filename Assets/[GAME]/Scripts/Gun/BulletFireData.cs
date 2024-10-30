using UnityEngine;
namespace _GAME_.Scripts.Gun
{
    public struct BulletFireData
    {
        public readonly float ThrowSpeed;
        public Vector3 TargetPosition;
        public float bulletDamage;

        public BulletFireData(float throwSpeed, Vector3 targetPosition = default, float bulletDamage = 0)
        {
            ThrowSpeed = throwSpeed;
            TargetPosition = targetPosition;
            this.bulletDamage = bulletDamage;
        }

    }
}