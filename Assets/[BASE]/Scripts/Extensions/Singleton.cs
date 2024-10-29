using UnityEngine;
namespace _BASE_.Scripts.Extensions
{
    public class Singleton<T>: MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
                    }
                }
                return _instance;
            }
        }
        //
        // private static T GetInstance<T>() where T : MonoBehaviour
        // {
        //     if (_instance == null)
        //     {
        //         _instance = FindObjectOfType<T>();
        //         if (_instance == null)
        //         {
        //             Debug.LogError("An instance of " + typeof(T) + " is needed in the scene, but there is none.");
        //         }
        //     }
        //     return _instance;
        // }
    }
}
