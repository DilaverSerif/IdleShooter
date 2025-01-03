using _GAME_.Scripts.Gun;

namespace _GAME_.Scripts
{
    public class TestBullet : Bullet<EnemyHealth>
    {
        public override void Fire(BulletFireData bulletFireData = default)
        {
            damage = bulletFireData.BulletDamage;
            Rigidbody.velocity = transform.forward * bulletFireData.ThrowSpeed;
        }
    }
}
