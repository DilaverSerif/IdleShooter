using System;
using Sirenix.OdinInspector;

namespace _BASE_.Scripts.Stat_System
{
    [Serializable]
    public class Stat
    {
        [ReadOnly]
        public StatTags statTag;
        public float value;
        
        public Stat(StatTags statTag, float value)
        {
            this.statTag = statTag;
            this.value = value;
        }
    }
}