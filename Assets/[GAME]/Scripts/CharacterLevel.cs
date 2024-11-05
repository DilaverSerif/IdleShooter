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
        public UnityEvent<CharacterLevel> OnExperienceChanged;
        
        private int _level;
        [ShowInInspector, BoxGroup("Debug"),ReadOnly]
        private float _experience;
        public float Experience => _experience;

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

        [Button]
        public virtual void AddExperience(float experience,bool invokeAction = false)
        {
            _experience += experience;
            
            while (_experience >= NeededExperience(_level))
            {
                OnLevelUp?.Invoke();
                _experience -= NeededExperience(_level);
                _level++;
            }
            
            if (invokeAction)
                OnExperienceChanged?.Invoke(this);
        }
       
        private float SavedExperience
        {
            get => ES3.Load("PlayerExperience",500f);
            set => ES3.Save("PlayerExperience",value);
        }
        private int SavedLevel
        {
            get => ES3.Load("PlayerLevel",1);
            set => ES3.Save("PlayerLevel",value);
        }

        public void Save()
        {
            SavedExperience = _experience;
            SavedLevel = Level;
        }
        
        public void Load()
        {
            AddExperience(SavedExperience);
            Level = SavedLevel;
        }
    }
}