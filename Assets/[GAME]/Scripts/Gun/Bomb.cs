using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
namespace _GAME_.Scripts.Gun
{
    public class Bomb : Bullet<Damageable>
    {
        private Vector3 _startPosition;

        void OnEnable()
        {
            _startPosition = transform.position;
        }

        public override void Fire(BulletFireData bulletFireData = default)
        {
            var alertAreaData = new SpawnAlertAreaData
            {
                position = bulletFireData.TargetPosition,
                alertAreaType = AlertAreaType.Circle,
                duration = 1f,
                delay = 0.15f,
                onSpawn = () => Rigidbody.DOJump(bulletFireData.TargetPosition,
                    _startPosition.y + Random.Range(1, 3f), 1,
                    bulletFireData.ThrowSpeed).SetEase(Ease.Linear)
            };
            
            
            AlertAreaManager.Instance.SpawnAlertArea(alertAreaData);

            // // Hedefe doğru yönü hesapla
            // Vector3 direction = bulletFireData.TargetPosition - _startPosition;
            //
            // // Yatay ve dikey mesafeyi ayır
            // float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
            // float verticalDistance = direction.y;
            //
            //
            // var throwSpeed = bulletFireData.ThrowSpeed;
            // // Hedefe ulaşmak için gerekli açı ve hızı hesapla
            // float throwAngleRad = Mathf.Atan((Mathf.Pow(throwSpeed, 2) + Mathf.Sqrt(Mathf.Pow(throwSpeed, 4) - _gravity * (_gravity * Mathf.Pow(horizontalDistance, 2) + 2 * verticalDistance * Mathf.Pow(throwSpeed, 2)))) / (_gravity * horizontalDistance));
            //
            // // Hızı ve açıyı hedef yönünde ayarla
            // Vector3 launchVelocity = new Vector3(direction.x, throwAngleRad, direction.z).normalized * throwSpeed;
            //
            // // Rigidbody'ye hızı uygula
            // Rigidbody.velocity = launchVelocity;
        }
    }
}