using _BASE_.Scripts.Stat_System;

namespace _GAME_.Scripts
{
    public class PlayerStatOwner : StatOwner
    {
        public struct StatSave
        {
            public StatTags statTag;
            public float value;
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            AddOrUpdateStat(new StatTransferData(StatTags.BaseDamage,1,MathType.Add));
            AddOrUpdateStat(new StatTransferData(StatTags.BaseHealth,1,MathType.Add));
            AddOrUpdateStat(new StatTransferData(StatTags.BaseSpeed,1,MathType.Add));
        }
    }
}