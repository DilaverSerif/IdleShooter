namespace _BASE_.Scripts.Stat_System
{
    public class StatTransferData
    {
        public readonly StatTags StatTag;
        public readonly float Value;
        public readonly MathType AddType;
    
        public StatTransferData(StatTags statTag, float value, MathType addType)
        {
            StatTag = statTag;
            Value = value;
            AddType = addType;
        }
    }
}