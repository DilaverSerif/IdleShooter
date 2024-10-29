using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _GAME_.Scripts
{
    public abstract class CharacterLevel: MonoBehaviour
    {
        public UnityEvent onLevelUp;
        public UnityEvent onExperienceChanged;
        
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
                onExperienceChanged?.Invoke();
                
                while (_experience >= NeededExperience(_level))
                {
                    onLevelUp?.Invoke();
                    _experience -= NeededExperience(_level);
                    _level++;
                }
            }
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