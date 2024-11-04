using System;

namespace _GAME_.Scripts
{
    [Serializable]
    public class GameOptions
    {
        public float hexagonScaleMultiplier = 1.5f;
        public float[] playerNextLevelExperience = {
            100,
            200,
            300,
            400,
            500,
            600,
            700,
            800,
            900,
            1000
        };
    }
}