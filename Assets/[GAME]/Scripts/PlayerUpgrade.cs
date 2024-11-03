using System;
using _BASE_.Scripts;
using UnityEngine;
namespace _GAME_.Scripts
{
    [CreateAssetMenu(menuName = "Create PlayerUpgrade", fileName = "PlayerUpgrade", order = 0)]
    public class PlayerUpgrade : SingletonScriptableObject<PlayerUpgrade>
    {
        public Upgrade[] upgrades;
        private bool _isInitialized;
        private void OnEnable()
        {
            if (!_isInitialized)
            {
                LoadSave();
                _isInitialized = true;
            }
        }

        private void OnDisable()
        {
            _isInitialized = false;
        }

        private void LoadSave()
        {
            foreach (var upgrade in upgrades)
            {
                upgrade.currentLevel = upgrade.SaveLevel;
            }
        }

        public bool BuyUpgrade(StatTags statTag)
        {
            var playerSave = RedManager.Instance.GetManager<SaveManager>().playerSave;

            for (int i = 0; i < upgrades.Length; i++)
            {
                if (upgrades[i].name == statTag)
                {
                    var upgrade = upgrades[i];
                    var price = upgrades[i].GetPrice();
                    if (price <= playerSave.GetGold())
                    {
                        playerSave.AddCoin(-price);
                        upgrade.LevelUp();
                        return true;
                    }
                }
            }

            return false;
        }
        public (int, float) GetUpgrade(StatTags statTag)
        {
            if (!_isInitialized)
            {
                LoadSave();
                _isInitialized = true;
            }
            
            foreach (var upgradeData in upgrades)
            {
                if (upgradeData.name == statTag)
                {
                    return (upgradeData.currentLevel, upgradeData.GetPrice());
                }
            }
            return (0, 0);
        }
    }
}
