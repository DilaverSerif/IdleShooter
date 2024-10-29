using System;
using Eflatun.SceneReference;

namespace _BASE_.Scripts
{
    [Serializable]
    public struct MenuData
    {
        public SceneReference SceneReference;
        public string menuName;
        
        public MenuData(SceneReference sceneReference, string menuName)
        {
            SceneReference = sceneReference;
            this.menuName = menuName;
        }
    }
}