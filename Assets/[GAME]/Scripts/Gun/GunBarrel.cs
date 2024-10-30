using System;
using System.Linq;
using _BASE_.Scripts;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _GAME_.Scripts.Gun
{
    [Serializable]
    public class GunBarrel
    {
        // <summary>
        // The number of bullets fired per shooting.
        // </summary>
        public int perShootingBulletCount;
        public float waitPerFireBullet;
        public BulletBase bullet;
        public GunBarrelFireType fireType;
    
        [BoxGroup("Spread"),OnValueChanged("ChangedAnimationCurve")]
        public AnimationCurve spreadCurveX;
        [BoxGroup("Spread"),OnValueChanged("ChangedAnimationCurve")]
        public AnimationCurve spreadCurveY;
        [BoxGroup("Spread")]
        public float maxSpreadTime;
        public float increasePerShootSpread;
        
        public float bulletSpeed;
        public float range;
        public float angle;
        [BoxGroup("Debug"),ShowInInspector]
        private float _currentSpread;

        private void ChangedAnimationCurve()
        {
            var x = spreadCurveX.keys.Last().value;
            var y = spreadCurveY.keys.Last().value;
            maxSpreadTime = Mathf.Max(x, y);
        }
        
        public Quaternion[] GetForwardRotations()
        {
            var bulletRotations = new Quaternion[perShootingBulletCount];

            switch (fireType)
            {

                case GunBarrelFireType.Triangle:
                    TriangleShoot(bulletRotations);
                    break;
                case GunBarrelFireType.Sphere:
                    SphereShoot(bulletRotations);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        
            return bulletRotations;
        }
        void SphereShoot(Quaternion[] bulletRotations)
        {
            for (var i = 0; i < perShootingBulletCount; i++)
            {
                var getOffset = new Vector2(
                    spreadCurveX.Evaluate(Random.Range(0, _currentSpread)),
                    spreadCurveY.Evaluate(Random.Range(0, _currentSpread))
                );
                Vector3 randomPoint = Random.insideUnitCircle * angle;
                randomPoint.z = range;
                //bulletRotations[i] = Quaternion.Euler(new Vector3(getOffset.y, getOffset.x, 0)) * Quaternion.LookRotation(randomPoint);
                bulletRotations[i] = Quaternion.LookRotation(randomPoint) * Quaternion.Euler(new Vector3(
                    Random.Range(-getOffset.y,getOffset.y), 
                    Random.Range(-getOffset.x,getOffset.x), 0));
            }
            _currentSpread = Mathf.Max(_currentSpread + increasePerShootSpread, maxSpreadTime);
        }
        private void TriangleShoot(Quaternion[] bulletRotations)
        {
            var half = perShootingBulletCount / 2;
            var anglePerBullet = angle / half;

            for (var i = -half; i <= half; i++)
            {
                var getOffset = new Vector2(
                    spreadCurveX.Evaluate(Random.Range(0, _currentSpread)),
                    spreadCurveY.Evaluate(Random.Range(0, _currentSpread))
                );

                bulletRotations[i + half] = Quaternion.Euler(new Vector3(
                    Random.Range(-getOffset.y,getOffset.y),
                    Random.Range(-getOffset.x,getOffset.x) + anglePerBullet * i, 0));
            }

            _currentSpread = Mathf.Max(_currentSpread + increasePerShootSpread, maxSpreadTime);
        }
        
        public void OnGizmosSelected(Transform firePoint)
        {
#if UNITY_EDITOR
            if (fireType == GunBarrelFireType.Triangle)
            {
                var direction = firePoint.forward * range;
                var rotatedDirection = Quaternion.AngleAxis(angle, firePoint.up) * direction;
                var rotatedLine = firePoint.position + rotatedDirection;

                HandlesHelper.DrawLine(firePoint.position, rotatedLine, Color.white);
                rotatedDirection = Quaternion.AngleAxis(angle, -1 * firePoint.up) * direction;
                rotatedLine = firePoint.position + rotatedDirection;
                HandlesHelper.DrawLine(firePoint.position, rotatedLine, Color.white);
            }
        
            if (fireType == GunBarrelFireType.Sphere)
            {
                Handles.DrawWireDisc(firePoint.position + firePoint.forward * range, Vector3.back, angle, 5);
            }
#endif
        }
    }
}