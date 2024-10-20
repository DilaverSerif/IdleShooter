using _BASE_.Scripts;

namespace _GAME_.Scripts
{
    public class TargetMoveState : PlayerState
    {
        public TargetMoveState(PlayerBrain playerBrain, bool needsExitTime = false, bool isGhostState = false) : base(playerBrain, needsExitTime, isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _playerBrain.CharacterController.SetLookDirection(PhysicsBasedCharacterController.lookDirectionOptions.targetDirection);
        }
    }
}