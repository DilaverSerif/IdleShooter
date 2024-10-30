using UnityEngine;
namespace _GAME_.Scripts.Gun
{
    public struct BulletFireData
    {
        public readonly float ThrowSpeed;
        public Vector3 TargetPosition;
        public readonly float BulletDamage;

        public BulletFireData(float throwSpeed, Vector3 targetPosition = default, float bulletDamage = 0)
        {
            ThrowSpeed = throwSpeed;
            TargetPosition = targetPosition;
            this.BulletDamage = bulletDamage;
        }

    }
}