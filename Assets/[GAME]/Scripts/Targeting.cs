using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _GAME_.Scripts
{
    public abstract class Targeting<T> : MonoBehaviour where T: MonoBehaviour
    {
        public float viewRange = 10;
        public float exitViewRange = 12;
        public float dangerRange = 2;
        public float viewAngle = 35f;
        
        public LayerMask targetLayer;
        public LayerMask blockLayer;
        public T currentTarget;

        public bool HaveDangerousArea;
        public bool HaveBlockArea;
        public bool HaveViewAngle;
        public bool selectClosestTarget;
        
        public virtual bool HasTarget => currentTarget != null;

        //<summary>
        // Returns the closest target in the attack range.
        // </summary>
        public virtual void FindTarget()
        {
            var results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(transform.position, viewRange, results, targetLayer);
    
            if (NoFindAnything(size)) return;

            if (TargetSelector(OrderTargets(results))) return;
            
            if (HaveTarget()) return;

            currentTarget = null;
        }
        bool HaveTarget()
        {
            if (currentTarget == null) return false; //Eğer target varsa 
            var currentTargetTransform = currentTarget.transform;
            if (!CheckBlock(currentTargetTransform))
            {
                if (Vector3.Distance(currentTargetTransform.position, currentTarget.transform.position) < exitViewRange)
                    return true;
            }
            return false;
        }
        protected virtual bool TargetSelector(List<T> sortedList)
        {
            foreach (var iDamageable in sortedList)
            {
                if (iDamageable == null) continue;
                if (CheckBlock(iDamageable.transform)) continue;

                var currentPosition = transform.position;
                var distance = Vector3.Distance(currentPosition, iDamageable.transform.position);

                if (HaveDangerousArea && IsTargetInDangerousRange(distance))
                {
                    currentTarget = iDamageable;
                    return true;
                }

                if (HaveViewAngle && IsTargetInViewAngle(iDamageable))
                {
                    currentTarget = iDamageable;
                    return true;
                }

                if (currentTarget == null)
                {
                    currentTarget = iDamageable;
                    return true;
                }

                if (selectClosestTarget)
                {
                    currentTarget = iDamageable;
                    return true;
                }
            }
            return false;
        }
        protected virtual List<T> OrderTargets(Collider[] results)
        {
            return results
                .Where(damageable => damageable != null)
                .OrderBy(damageable => Vector3.Distance(transform.position, damageable.transform.position)).Select(
                    damageable => damageable.GetComponent<T>()).ToList();
            
        }
        protected virtual bool NoFindAnything(int size)
        {
            if (size != 0) return false;
            if (currentTarget == null) return true;

            if (Vector3.Distance(currentTarget.transform.position, transform.position) < exitViewRange)
            {
                if (CheckBlock(currentTarget.transform))
                {
                    currentTarget = null;
                    return true;
                }
            }
            else
            {
                currentTarget = null;
                return true;
            }

            return true;
        }

        private bool IsTargetInViewAngle(T iDamageable)
        {
            var direction = iDamageable.transform.position - transform.position;
            var angle = Vector3.Angle(transform.forward, direction);
            return angle < viewAngle;
        }

        private bool IsTargetInDangerousRange(float distance)
        {
            return distance < dangerRange;
        }

        private bool CheckBlock(Transform target)
        {
            var raycastHit = new RaycastHit[1];
            var direction = target.position - transform.position;
            var size = Physics.RaycastNonAlloc(transform.position, direction, raycastHit, direction.magnitude, blockLayer);
            
            return size > 0;
        }

        #if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, Vector3.up, viewRange);
            Handles.DrawWireDisc(transform.position, Vector3.up, exitViewRange);
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, dangerRange);

            
            //angle gizmos
            var forward = transform.forward;
            var right = Quaternion.Euler(0, viewAngle, 0) * forward;
            var left = Quaternion.Euler(0, -viewAngle, 0) * forward;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + right * viewRange);
            Gizmos.DrawLine(transform.position, transform.position + left * viewRange);

            if (currentTarget)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, currentTarget.transform.position);
            }
        }
  #endif
    }
}