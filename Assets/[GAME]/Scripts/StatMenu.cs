using _BASE_.Scripts;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
namespace _GAME_.Scripts
{
    public class StatMenu : MainMenu
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Button closeButton;

        [SerializeField, BoxGroup("Buy Buttons")]
        private UpgradeButton[] upgradeButtons;
        private void Awake()
        {
            closeButton.onClick.AddListener(() =>
            {
                _ = RedManager.Instance.GetManager<MenuManager>().CloseMenu(MenuTags.StatShop);
            });
            
            foreach (var upgradeButton in upgradeButtons)
                upgradeButton.Init();
        }
        public override Tween OpenMenu()
        {
            return canvasGroup.DOFade(1, 0.5f).OnComplete(
                () =>
                {
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.ignoreParentGroups = true;
                    canvasGroup.interactable = true;
                }
            );
        }
        public override Tween CloseMenu()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.ignoreParentGroups = false;
            canvasGroup.interactable = false;
            return canvasGroup.DOFade(0, 0.5f);
        }
    }
}