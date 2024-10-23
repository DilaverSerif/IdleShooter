using _GAME_.Scripts.Gun;

namespace _GAME_.Scripts
{
    public class TestBullet : Bullet<EnemyHealth>
    {
        public override void Fire(float extraDamage = 0)
        {
            Rigidbody.velocity = transform.forward * BulletRange();
        }
    }
}
