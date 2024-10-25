using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace _GAME_.Scripts
{
    public class PlayerTargeting : Targeting<Damageable>
    {
        public Transform aimTarget;
        public Side side => currentTarget.GetSide();

        public override bool HasTarget
        {
            get
            {
                if (currentTarget == null) return false;
                return currentTarget.Alive() && currentTarget.GetSide() != Side.Player;
            }
        }

        void Update()
        {
            FindTarget();
            if (HasTarget)
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
            var damageableList = new List<Tuple<float, Damageable>>();

            foreach (var result in results)
            {
                if (result == null) continue;

                var damageable = result.GetComponent<Damageable>();
                if (damageable != null && damageable.Alive())
                {
                    float distance = Vector3.Distance(transform.position, result.transform.position);
                    damageableList.Add(Tuple.Create(distance, damageable));
                }
            }

            // Sort by distance first, then by side in one go
            damageableList.Sort((a, b) =>
            {
                int distanceComparison = a.Item1.CompareTo(b.Item1);
                return distanceComparison != 0 ? distanceComparison : a.Item2.GetSide().CompareTo(b.Item2.GetSide());
            });

            // Return the sorted list of damageable objects
            return damageableList.Select(item => item.Item2).ToList();
        }

    }
}