using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _BASE_.Scripts
{
    [Serializable]
    public class SaveManager:Manager
    {
        public List<ISave> SaveData = new();
        
        public void Save()
        {
            foreach (var save in SaveData)
            {
                save.Save();
            }
        }
        
        public void Load()
        {
            foreach (var save in SaveData)
            {
                save.Load();
            }
        }
        
        public void OnDisable()
        {
            Save();
        }

        public override void Initialized()
        {
            base.Initialized();
            SaveData = FindObjectsOfType<MonoBehaviour>(true).OfType<ISave>().ToList();
            Load();
        }
    }
}