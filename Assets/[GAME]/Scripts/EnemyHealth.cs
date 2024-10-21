using UnityEngine;

namespace _GAME_.Scripts
{
    public class EnemyHealth : Damageable
    {

        public override Side GetSide()
        {
            return Side.Enemy;
        }
        public override void TakeDamage(float damage)
        {
            Debug.Log("Enemy took " + damage + " damage");
        }
    }
}