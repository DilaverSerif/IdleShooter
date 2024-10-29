using System;
using _GAME_.Scripts.Gun;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyWeaponAttackSystem : AttackSystem
{
    public Gun currentGun;
    
    public bool TargetInRange(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) < currentGun.gunBarrel.range;
    }
    
    private void Awake()
    {
        currentGun = GetComponentInChildren<Gun>();
    }

    public override void Attack()
    {
        Debug.Log("EnemyWeaponAttackSystem");
        currentGun.AutoFire(null).Forget();
    }

    public override void StopAttack()
    {
        currentGun.StopFire();
    }
}

public abstract class AttackSystem : MonoBehaviour
{
    public abstract void Attack();
    public abstract void StopAttack();
}
