using System;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace _GAME_.Scripts
{
    [Serializable]
    public class PlayerLevel : CharacterLevel
    {
        public static UnityEvent<PlayerLevel> OnPlayerExpChanged = new UnityEvent<PlayerLevel>();
        [ShowInInspector, BoxGroup("Debug")]
        public override float NeededExperience(int level)
        {
            return GameSettings.Instance.gameOptions.playerNextLevelExperience[level];
        }

        public override void AddExperience(float experience, bool invokeAction = false)
        {
            base.AddExperience(experience, invokeAction);
            
            if (invokeAction)
                OnPlayerExpChanged?.Invoke(this);
        }

        #if UNITY_EDITOR

        [Button]
        private void LogPrint()
        {
            UnityEngine.Debug.Log($"Level: {Level} Experience: {Experience} NeededExperience: {NeededExperience(Level)}");
        }
        
  #endif
    }
}