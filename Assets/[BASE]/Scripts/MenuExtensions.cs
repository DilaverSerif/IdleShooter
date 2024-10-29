using UnityEngine;

namespace _BASE_.Scripts
{
    public static class MenuExtensions
    {
        public static string ToFriendlyString(this MenuTags menuTags)
        {
            return menuTags.ToString().Replace("_", " ");
        }
        
        public static void OpenMenu(this MenuTags menuTags)
        {
            RedManager.Instance.GetManager<MenuManager>().OpenMenu(menuTags);
        }
        
        public static void CloseMenu(this MenuTags menuTags)
        {
            RedManager.Instance.GetManager<MenuManager>().CloseMenu(menuTags);
        }
    }
}
