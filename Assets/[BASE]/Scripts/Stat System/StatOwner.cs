using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _BASE_.Scripts.Stat_System
{
    public abstract class StatOwner: MonoBehaviour
    {
        public List<Stat> haveStats;
        private List<IStatOwner> _userStats;

        protected virtual void Awake()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            _userStats = GetComponentsInChildren<MonoBehaviour>().OfType<IStatOwner>().ToList();
            
            foreach (var userStat in _userStats)
            {
                userStat.Initializing(this);
            }
        }

        public void AddOrUpdateStat(StatTransferData stat)
        {
            var findStat = haveStats.FirstOrDefault(x => x.statTag == stat.StatTag);
            
            if (findStat != null)
            {
                switch (stat.AddType)
                {
                    case MathType.Add:
                        findStat.value += stat.Value;
                        break;
                    case MathType.Subtract:
                        findStat.value -= stat.Value;
                        break;
                    case MathType.Multiply:
                        findStat.value *= stat.Value;
                        break;
                    case MathType.Divide:
                        findStat.value /= stat.Value;
                        break;
                    default:
                        Debug.LogError("MathType is not valid");
                       break;
                }
            }
            else
            {
                findStat = new Stat(stat.StatTag, stat.Value);
                haveStats.Add(findStat);
            }
            
            foreach (var userStat in _userStats)
                userStat.UpdateStat(findStat);
            
            if (findStat.value <= 0)
                haveStats.Remove(findStat);
        }

#if UNITY_EDITOR
        [Button]
        public void AddStatTest(StatTags tag,float value,MathType mathType)
        {
            AddOrUpdateStat(new StatTransferData(tag,value,mathType));
        }
#endif
    }
    
    public enum MathType
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}