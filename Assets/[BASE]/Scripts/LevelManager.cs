using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _BASE_.Scripts
{
    [Serializable]
    public class LevelManager:Manager
    {
        public SceneReference mainScene;
        public SceneReference loadingScene;
        public SceneReference[] levelOrders;
        private CancellationTokenSource _cancellationTokenSource;
        private string NextLevelName
        {
            get => ES3.Load("NextLevelName",defaultValue:"");
            set => ES3.Save("NextLevelName", value);
        }
    
        public void LoadNextLevel(string nextLevelName)
        {
            NextLevelName = nextLevelName;
            SceneManager.LoadScene(loadingScene.Name);
        }

        public async UniTask LoadScenes()
        {
            if(mainScene == null) return;
            if(mainScene.Name != SceneManager.GetActiveScene().name) return;
            var token = _cancellationTokenSource.Token;
            foreach (var sceneReference in levelOrders)
            {
                var loadingAsync = SceneManager.LoadSceneAsync(sceneReference.Name, LoadSceneMode.Additive);
                await UniTask.WaitUntil(() => loadingAsync is { isDone: true }, cancellationToken: token);

                await SceneManager.GetSceneByName(sceneReference.Name).GetRootGameObjects()[0]
                    .GetComponent<SceneStarter>().LoadingScene(token);
            
                Debug.Log($"Loaded {sceneReference.Name}");
            }
        
            Debug.Log("All scenes loaded");
        }

        public void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void OnDisable()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}