using System;
using System.Collections.Generic;
using System.Linq;
using _BASE_.Scripts.Extensions;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _BASE_.Scripts
{
    [Serializable]
    public class OpenedMenuData
    {
        public MenuTags menuTags;
        public MainMenu mainMenu;
    }
    [Serializable]
    public class MenuManager : Manager
    {
        public List<SceneReference> menuData = new();
        [ShowInInspector, ReadOnly]
        private List<OpenedMenuData> _openedMenu = new();
        public async void OpenMenu(MenuTags menuTags, bool foremost = false)
        {
            if (IsMenuOpen(menuTags))
            {
                Debug.Log($"{menuTags} is already open");
                return;
            }
            var menu = menuData.Find(x => x.Name.Equals(menuTags.ToString()));
            Debug.Log($"Loading {menu.Name}");

            var parameters = new LoadSceneParameters(LoadSceneMode.Additive);
            await SceneManager.LoadSceneAsync(menu.Name, parameters);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(menu.Name));

            var scene = SceneManager.GetSceneByName(menu.Name);
            var findMenuInScene = scene.GetRootGameObjects().ToList().FirstOrDefault(x => x.GetComponent<MainMenu>())?.GetComponent<MainMenu>();
            
            if (findMenuInScene != null)
            {
                findMenuInScene.OpenMenu();
                _openedMenu.Add(new OpenedMenuData
                {
                    menuTags = menuTags,
                    mainMenu = findMenuInScene
                });
            }
            else
            {
                Debug.LogError($"No MainMenu script found in {menu.Name}");
                return;
            }
            
            if (foremost)
            {
                var canva = SceneManager.GetSceneByName(menu.Name).GetRootGameObjects().ToList().Find(x => x.GetComponent<Canvas>());
                canva.GetComponent<Canvas>().sortingOrder = 999;
            }
        }

        public async UniTaskVoid CloseMenu(MenuTags menuTags)
        {
            var menu = menuData.Find(x => x.Name == menuTags.ToString());
            Debug.Log($"Closing {menu.Name}");

            var currentMenu = _openedMenu.FirstOrDefault(x => x.menuTags.Equals(menuTags));
            if(currentMenu == null)
            {
                Debug.LogError($"{menuTags} currentMenu not open");
                return;
            }
            
            if (!currentMenu.mainMenu)
            {
                Debug.LogError("Main menu can't be closed");
                return;
            }
            
            await currentMenu.mainMenu.CloseMenu();
            SceneManager.UnloadSceneAsync(menu.Name);
            _openedMenu.Remove(_openedMenu.Find(x => x.menuTags == menuTags));
        }

        private bool IsMenuOpen(MenuTags menuTags)
        {
            return _openedMenu.Count != 0 && _openedMenu.Contains(_openedMenu.Find(x => x.menuTags.Equals(menuTags)));
        }

#if UNITY_EDITOR
        [Button]
        public void CreateTags()
        {
            var getQuestsName = menuData.ConvertAll(x => x.Name);
            getQuestsName.ToArray().GenerateEnumFile("Tags", "Assets/[BASE]/Scripts/Menu System/Enums/MenuEnums.cs", "Menu");

            Debug.Log("Quests tags are created");
        }
#endif

    }
}
