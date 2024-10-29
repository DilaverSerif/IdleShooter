using System;
using System.Collections.Generic;
using _BASE_.Scripts.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _BASE_.Scripts
{
    [Serializable]
    public class MenuManager:Manager
    {
        public List<MenuData> menuData = new();
        [ShowInInspector,ReadOnly]
        private List<MenuTags> _openedMenu;
        public void OpenMenu(MenuTags menuTags)
        {
            if (IsMenuOpen(menuTags))
            {
                Debug.Log($"{menuTags} is already open");
                return;
            }
            var menu = menuData.Find(x => x.menuName == menuTags.ToString());
            Debug.Log($"Loading {menu.menuName}");
            
            SceneManager.LoadScene(menu.SceneReference.Name,LoadSceneMode.Additive);
            _openedMenu.Add(menuTags);
        }
        
        public void CloseMenu(MenuTags menuTags)
        {
            if (!IsMenuOpen(menuTags))
            {
                Debug.Log($"{menuTags} is already closed");
                return;
            }
            var menu = menuData.Find(x => x.menuName == menuTags.ToString());
            Debug.Log($"Closing {menu.menuName}");
            
            SceneManager.UnloadSceneAsync(menu.SceneReference.Name);
            _openedMenu.Remove(menuTags);
        }
        
        private bool IsMenuOpen(MenuTags menuTags)
        {
            return _openedMenu.Contains(menuTags);
        }

#if UNITY_EDITOR
        [Button]
        public void CreateTags()
        {
            var getQuestsName = menuData.ConvertAll(x => x.menuName);
            getQuestsName.ToArray().GenerateEnumFile("Tags", "Assets/[BASE]/Scripts/Menu System/Enums/MenuEnums.cs", "Menu");
            
            Debug.Log("Quests tags are created");
        }
#endif

    }
}