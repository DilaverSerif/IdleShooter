using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace _GAME_.Scripts
{
    [Serializable]
    public class UpgradeButton
    {
        public StatTags statTag;
        public Button upgradeButton;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI priceText;
        public void Init()
        {
            UpdateTexts();
            upgradeButton.onClick.AddListener(() =>
            {
                upgradeButton.interactable = false;
                _ = PlayerUpgrade.Instance.BuyUpgrade(statTag)
                    ? SuccessButtonAnimation() : FailButtonAnimation();
            });
        }

        private async UniTask FailButtonAnimation()
        {
            upgradeButton.image.DOColor(Color.red, 0.25f).SetLoops(2, LoopType.Yoyo);
            await upgradeButton.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f);
            upgradeButton.interactable = true;
        }

        private async UniTask SuccessButtonAnimation()
        {
            UpdateTexts();
            await upgradeButton.transform.DOScale(Vector3.one * 0.9f, 0.25f)
                .SetLoops(2, LoopType.Yoyo);
            upgradeButton.interactable = true;
        }

        private void UpdateTexts()
        {
            var playerUpgrade = PlayerUpgrade.Instance;
            var upgrade = playerUpgrade.GetUpgrade(statTag);
            levelText.text = $"Lv.{upgrade.Item1}";
            priceText.text = $"{upgrade.Item2:F2}";
        }
    }
}
