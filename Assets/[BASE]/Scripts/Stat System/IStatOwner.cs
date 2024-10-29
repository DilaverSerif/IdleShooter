namespace _BASE_.Scripts.Stat_System
{
    public interface IStatOwner
    {
        public StatOwner StatOwner { get; set; }

        public void Initializing(StatOwner statOwner)
        {
            StatOwner = statOwner;
        }

        public void UpdateStat(Stat stat);
    }
}