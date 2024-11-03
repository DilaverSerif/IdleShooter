using System;
using _BASE_.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class BottomBar : MonoBehaviour
{
    [Serializable]
    public class MenuTriggerButton
    {
        public Button button;
        public MenuTags menu;
        
        public void Init()
        {
            button.onClick.AddListener(() => menu.OpenMenu());
        }
    }
    
    public MenuTriggerButton[] menuTriggerButtons;
    
    private void Awake()
    {
        foreach (var menuTriggerButton in menuTriggerButtons)
        {
            menuTriggerButton.Init();
        }
    }
}
