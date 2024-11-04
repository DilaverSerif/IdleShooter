using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _GAME_.Scripts
{
    public abstract class CharacterLevel
    {
        public Action OnLevelUp;
        public Action OnExperienceChanged;
        
        private int _level;
        [ShowInInspector, BoxGroup("Debug"),ReadOnly]
        private float _experience;

        [ShowInInspector, BoxGroup("Debug")]
        public abstract float NeededExperience(int level);
        
        [ShowInInspector,BoxGroup("Debug")]
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                _experience = 0;
            }
        }
        
        [ShowInInspector,BoxGroup("Debug")]
        public float Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                OnExperienceChanged?.Invoke();
                
                while (_experience >= NeededExperience(_level))
                {
                    OnLevelUp?.Invoke();
                    _experience -= NeededExperience(_level);
                    _level++;
                }
            }
        }

        private float SavedExperience
        {
            get => ES3.Load("PlayerExperience",0f);
            set => ES3.Save("PlayerExperience",value);
        }
        private int SavedLevel
        {
            get => ES3.Load("PlayerLevel",1);
            set => ES3.Save("PlayerLevel",value);
        }

        public void Save()
        {
            SavedExperience = Experience;
            SavedLevel = Level;
        }
        
        public void Load()
        {
            Experience = SavedExperience;
            Level = SavedLevel;
        }
        
        public void AddExperience(float experience)
        {
            Experience += experience;
        }
        
        public void SetExperience(float experience)
        {
            Experience = experience;
        }
        
        public void SetLevel(int level)
        {
            Level = level;
        }
        
        public void Reset()
        {
            Level = 0;
            Experience = 0;
        }
      
    }
}