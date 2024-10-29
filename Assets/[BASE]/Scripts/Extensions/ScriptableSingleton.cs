using UnityEngine;
namespace _BASE_.Scripts.Extensions
{
    public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(typeof(T).Name);
                }
                return _instance;
            }
        }
    }
}
