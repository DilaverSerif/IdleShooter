using UnityEngine;

namespace _BASE_.Scripts
{
    public class SingletonScriptableObject<T>:ScriptableObject where T: ScriptableObject
    {
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
            set => _instance = value;
        }

        private static T _instance;

    }
}