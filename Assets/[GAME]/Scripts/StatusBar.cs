using System;
using _BASE_.Scripts;
using _GAME_.Scripts.Gun;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
namespace _GAME_.Scripts
{
    public class StatusBar : MainMenu
    {
        [Serializable]
        public struct StatusText
        {
            public TextMeshProUGUI text;
            public InventoryItem item;
            public string frontText;
            public string endText;
            
            public void SetText()
            {
                var playerInventory = RedManager.Instance.GetManager<SaveManager>().playerSave;
                text.text = frontText + playerInventory.GetItemAmount(item) + endText;
            }
        }
        
        [SerializeField]
        private CanvasGroup canvasGroup;
        public StatusText[] statusTexts;

        private void OnEnable()
        {
            UpdateTexts();
            PlayerInventory.OnItemAdded += UpdateTexts;
        }

        private void OnDisable()
        {
            PlayerInventory.OnItemAdded -= UpdateTexts;
        }
        private void UpdateTexts(InventoryItem obj)
        {
            foreach (var statusText in statusTexts)
            {
                if (statusText.item == obj)
                {
                    statusText.SetText();
                }
            }
        }

        private void UpdateTexts()
        {
            foreach (var statusText in statusTexts)
            {
                statusText.SetText();
            }
        }

        public override Tween OpenMenu()
        {
            UpdateTexts();
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