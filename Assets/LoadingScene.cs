using System.Threading;
using _BASE_.Scripts;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LoadingScene : SceneStarter
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI loadingText;
    public override async UniTask OnLoadedScene(CancellationToken token)
    {
        string nextScene = RedManager.Instance.GetManager<LevelManager>().GetNextLevelName();
        var loading = SceneManager.LoadSceneAsync(nextScene);
        
        if (loading != null)
            loading.allowSceneActivation = false;
        else
        {
            Debug.LogError("Scene not found");
            return;
        }

        while (!token.IsCancellationRequested)
        {
            if (loading.progress >= 0.9f)
            {
                slider.value = 1;
                loadingText.text = "STARING...";

                await UniTask.WaitForSeconds(1f, cancellationToken: token);
                loading.allowSceneActivation = true;
                break;
            }
            else
            {
                slider.value = loading.progress;
                loadingText.text = $"Loading {nextScene} {loading.progress * 100}%";
            }
            
            await UniTask.Yield(cancellationToken: token);
        }
    }
}
