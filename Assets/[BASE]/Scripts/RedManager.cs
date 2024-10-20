using UnityEngine;

namespace _BASE_.Scripts
{
    public class RedManager: MonoBehaviour
    {
        public LevelManager levelManager;
        private async void Start()
        {
           await levelManager.LoadScenes();
        }
    }
}