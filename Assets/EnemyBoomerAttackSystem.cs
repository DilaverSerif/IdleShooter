using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class EnemyBoomerAttackSystem : AttackSystem
{
    public float boomRange;
    
    public override void Attack()
    {
        Destroy(gameObject);
    }
    
    public override async UniTask TryAttack(Transform target = null)
    {
        TryAttackToken?.Cancel();
        TryAttackToken = new CancellationTokenSource();
        
        while (TryAttackToken.Token.IsCancellationRequested == false)
        {
            await UniTask.WaitUntil(() => AttackDelegates.All(attack => attack()), cancellationToken: TryAttackToken.Token);
            Attack();
        }
    }
    public override bool TargetInRange(Transform target)
    {
        return Vector3.Distance(transform.position, target.position) < boomRange;
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, boomRange);
    }
  #endif
}
