using System.Threading;
using _GAME_.Scripts;
using _GAME_.Scripts.Gun;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Melee : Damager
{
    public float attackRange;
    public float attackAngle;
    
    public override async UniTask Fire(Damageable target)
    {
        while (true)
        {
            if(target == null) return;
            if(Vector3.Distance(target.transform.position, owner.position) > attackRange) return;
            if(Vector3.Angle(target.transform.position - owner.position, owner.forward) > attackAngle) return;
        
            OnFireBullet?.Invoke();
            target.TakeDamage(weaponLevel.Damage);
        
            await UniTask.WaitForSeconds(weaponLevel.FireRate);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(owner == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(owner.position, attackRange);
        Gizmos.DrawRay(owner.position, owner.forward * attackRange);
        Gizmos.DrawRay(owner.position, Quaternion.Euler(0, attackAngle, 0) * owner.forward * attackRange);
        Gizmos.DrawRay(owner.position, Quaternion.Euler(0, -attackAngle, 0) * owner.forward * attackRange);
    }
}