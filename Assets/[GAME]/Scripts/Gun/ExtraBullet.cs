using UnityEngine.Serialization;

namespace _GAME_.Scripts.Gun
{
    public class ExtraBullet : GunPlugin
    {
        public int extraBulletCount;
    
        public override void Effect(Gun gun)
        {
            gun.gunBarrel.perShootingBulletCount += extraBulletCount;
        }

        public override void UnEffect(Gun gun)
        {
            gun.gunBarrel.perShootingBulletCount -= extraBulletCount;
        }
    }
}