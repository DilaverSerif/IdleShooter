using _BASE_.Scripts;
using UnityEngine;

namespace _GAME_.Scripts
{
    [CreateAssetMenu(menuName = "Create GameSettings", fileName = "GameSettings", order = 0)]
    public class GameSettings : SingletonScriptableObject<GameSettings>
    {
        public GameOptions gameOptions;
    }

   
}