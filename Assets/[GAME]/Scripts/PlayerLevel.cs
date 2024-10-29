using System;
using _GAME_.Scripts.LevelSystem;
using Sirenix.OdinInspector;

namespace _GAME_.Scripts
{
    [Serializable]
    public class PlayerLevel : CharacterLevel
    {
        [ShowInInspector, BoxGroup("Debug")]
        public override float NeededExperience(int level)
        {
            return GameSettings.Instance.GetNextLevelExperience(level);
        }
    }
}