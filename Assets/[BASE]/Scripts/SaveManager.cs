using System;
using System.Collections.Generic;
using System.Linq;
using _GAME_.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace _BASE_.Scripts
{
    [Serializable]
    public class SaveManager:Manager
    {
        public PlayerSave playerSave;
        private List<ISave> _saveData = new List<ISave>();

        private void Save()
        {
            ES3.Save("PlayerSave", playerSave);
            foreach (var save in _saveData)
                save.Save();
            
            playerSave.playerLevel.Save();
        }

        private void Load()
        {
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