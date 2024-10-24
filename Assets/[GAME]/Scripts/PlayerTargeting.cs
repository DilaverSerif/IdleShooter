using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace _GAME_.Scripts
{
    public class PlayerTargeting : Targeting<Damageable>
    {
        public Transform aimTarget;
        public Side side => currentTarget.GetSide();
        void Update()
        {
            FindTarget();
            if (currentTarget)
            {
                aimTarget.position = currentTarget.transform.position;
            }
            else
            {
                var pos = transform.position + transform.forward * 5;
                pos.y = 1f;
                aimTarget.position = pos;
            }
        }

        protected override List<Damageable> OrderTargets(Collider[] results)
        {
            return results
                .Where(damageable => damageable != null)
                .OrderBy(damageable => Vector3.Distance(transform.position, damageable.transform.position)).Select(
                    damageable => damageable.GetComponent<Damageable>()).OrderBy(damageable => damageable.GetSide()).ToList();
        }
    }
}