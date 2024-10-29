namespace _BASE_.Scripts.Stat_System
{
    public class SaveableStat: Stat
    {
        public int SaveValue
        {
            get => ES3.Load($"Base{statTag}",defaultValue:0);
            set => ES3.Save($"Base{statTag}", value);
        }
        
        public SaveableStat(StatTags statTag, float value) : base(statTag, value)
        {
            value = SaveValue;
        }
    }
}