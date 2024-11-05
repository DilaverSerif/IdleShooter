using System;
using System.Collections.Generic;
using System.Linq;
using _GAME_.Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _BASE_.Scripts
{
    [Serializable]
    public class SaveManager:Manager
    {
        public PlayerSave playerSave;
        private List<ISave> _saveData = new List<ISave>();

        [Button]
        private void Save()
        {
            Debug.Log("Game Saved");
            ES3.Save("PlayerSave", playerSave);
            foreach (var save in _saveData)
                save.Save();
            
            playerSave.playerLevel.Save();
        }
        
        [Button]
        private void Load()
        {
            Debug.Log("Game Loaded");
            playerSave = ES3.Load("PlayerSave", new PlayerSave());
            foreach (var save in _saveData)
                save.Load();
            
            playerSave.playerLevel.Load();
        }
        
        public void OnDisable()
        {
            Save();
        }

        public override void Initialized()
        {
            base.Initialized();
            _saveData = FindObjectsOfType<MonoBehaviour>(true).OfType<ISave>().ToList();
            Load();
        }
    }
}