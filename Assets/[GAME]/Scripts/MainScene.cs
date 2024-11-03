using _BASE_.Scripts;
using UnityEngine;
using UnityEngine.UI;
namespace _GAME_.Scripts
{
    public class MainScene : MonoBehaviour
    {
        public Button startButton;

        private void Start()
        {
            RedManager.Instance.GetManager<MenuManager>().OpenMenu(MenuTags.StatusBar, true);
            startButton.onClick.AddListener(
                () => RedManager.Instance.GetManager<LevelManager>().LoadNextLevel("TestArea") 
            );
            
            RedManager.Instance.GetManager<MenuManager>().OpenMenu(MenuTags.LevelBar);

        }
    }
}
