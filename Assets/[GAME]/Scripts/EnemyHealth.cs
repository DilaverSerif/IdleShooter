using UnityEngine;

namespace _GAME_.Scripts
{
    public class EnemyHealth : Damageable
    {
        private EnemyBrain _enemyBrain;
        protected override void Awake()
        {
            _enemyBrain = GetComponent<EnemyBrain>();
            base.Awake();
        }
        public override Side GetSide()
        {
            return Side.Enemy;
        }
        
        protected override void Die()
        {
            base.Die();
            _enemyBrain?.SetState(EnemyState.Dead);
        }

    }
}