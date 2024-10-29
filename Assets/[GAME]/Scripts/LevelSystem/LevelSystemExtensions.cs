using UnityEngine;

namespace _GAME_.Scripts.LevelSystem
{
    public static class LevelSystemExtensions
    {
        public static float GetNextLevelExperience(this GameSettings settings, int level)
        {
            if (level < 0 || level >= settings.gameOptions.playerNextLevelExperience.Length)
            {
                Debug.LogError("Level out of range");
                return -1;
            }

            return settings.gameOptions.playerNextLevelExperience[level];
        }
    }
}