using System.Threading;
using Cysharp.Threading.Tasks;
using UnityHFSM;

namespace _GAME_.Scripts
{
    public abstract class PlayerState : StateBase<EPlayerState>
    {
        private UniTask _playerState;
        private CancellationTokenSource _cancellationTokenSource;
        protected PlayerBrain _playerBrain;
        protected PlayerState(PlayerBrain playerBrain,bool needsExitTime = false, bool isGhostState = false) : base(needsExitTime, isGhostState)
        {
            _playerBrain = playerBrain;
        }

        public override void OnEnter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }
    
        public override void OnExit()
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}