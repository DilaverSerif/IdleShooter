using System;
using Sirenix.OdinInspector;

namespace _GAME_.Scripts
{
    [Serializable]
    public class PlayerLevel : CharacterLevel
    {
        [ShowInInspector, BoxGroup("Debug")]
        public override float NeededExperience(int level)
        {
            return GameSettings.Instance.gameOptions.playerNextLevelExperience[level];
        }
    }
}