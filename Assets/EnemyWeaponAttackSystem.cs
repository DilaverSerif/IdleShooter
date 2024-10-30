using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _GAME_.Scripts;
using _GAME_.Scripts.Gun;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyWeaponAttackSystem : AttackSystem
{
    public Gun currentGun;
    
    public override bool TargetInRange(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) < currentGun.gunBarrel.range;
    }
    
    private void Awake()
    {
        currentGun = GetComponentInChildren<Gun>();
    }

    public override void Attack()
    {
        if (!AttackDelegates.All(attack => attack()))
        {
            return;
        }
        Debug.Log("EnemyWeaponAttackSystem");
        currentGun.AutoFire(null).Forget();
    }
    
    public override void StopAttack()
    {
        currentGun.StopFire();
        base.StopAttack();
    }
    public override async UniTask TryAttack(Transform target)
    {
        TryAttackToken?.Cancel();
        TryAttackToken = new CancellationTokenSource();
        var token = TryAttackToken.Token;
        while (TryAttackToken.Token.IsCancellationRequested == false)
        {
            await UniTask.WaitUntil(()=> AttackDelegates.All(attack => attack()), cancellationToken: token);
            Debug.Log("Starting Attack");
            currentGun.AutoFire(target.GetComponent<Damageable>()).Forget();
            break;
        }
    }
}

public abstract class AttackSystem : MonoBehaviour
{
    public delegate bool AttackDelegate();
    protected readonly List<AttackDelegate> AttackDelegates = new List<AttackDelegate>();
    
    protected CancellationTokenSource TryAttackToken;
    void OnDisable()
    {
        StopAttack();
    }
    public abstract void Attack();
    public virtual void StopAttack()
    {
        TryAttackToken?.Cancel();
    }
    public abstract UniTask TryAttack(Transform target );
    public abstract bool TargetInRange(Transform target);
    
    public virtual void AddRequirement(AttackDelegate attackDelegate)
    {
        AttackDelegates.Add(attackDelegate);
    }
    
    public virtual void ClearRequirements()
    {
        AttackDelegates.Clear();
    }

}
