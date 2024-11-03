using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _BASE_.Scripts
{
    public abstract class SceneStarter: MonoBehaviour
    { 
        public abstract UniTask OnLoadedScene(CancellationToken token);
    }
}