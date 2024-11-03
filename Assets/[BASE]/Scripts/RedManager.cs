using System;
using System.Collections.Generic;
using System.Linq;
using _BASE_.Scripts.Extensions;
using Sirenix.OdinInspector;

namespace _BASE_.Scripts
{
    public class RedManager: Singleton<RedManager>
    {
        public List<Manager> managers;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            FindManagers();
            
            foreach (var manager in managers)
                manager.Initialized();
        }
        
        public T GetManager<T>()
        {
            return managers.OfType<T>().FirstOrDefault();
        }
        
        private void FindManagers()
        {
            managers = FindObjectsOfType<Manager>(true).ToList();
            managers.Sort((a, b) => a.priority.CompareTo(b.priority));
        }

#if UNITY_EDITOR
        
        [Button]
        private void FindManagersEditor()
        {
            FindManagers();
        }
        
#endif
    }
}