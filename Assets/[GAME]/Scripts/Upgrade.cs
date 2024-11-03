using System;
namespace _GAME_.Scripts
{
    [Serializable]
    public class Upgrade
    {
        public StatTags name;
        public int currentLevel;
        public int[] price;
        public int SaveLevel
        {
            get => ES3.Load("Upgrade_" + name, "PlayerSave", 0);
            set => ES3.Save("Upgrade_" + name, value, "PlayerSave");
        }
        public Upgrade(StatTags name, int currentLevel, int[] price)
        {
            this.name = name;
            this.currentLevel = currentLevel;
            this.price = price;
        }

        public int GetPrice()
        {
            return price[currentLevel];
        }
        public void LevelUp()
        {
            currentLevel++;
            SaveLevel = currentLevel;
        }
    }
}
