using _BASE_.Scripts;

namespace _GAME_.Scripts
{
    public class TargetIdleState : PlayerState
    {
        public TargetIdleState(PlayerBrain playerBrain, bool needsExitTime = false, bool isGhostState = false) : base(playerBrain, needsExitTime, isGhostState)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _playerBrain.CharacterController.SetLookDirection(PhysicsBasedCharacterController.lookDirectionOptions.targetDirection);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            if (_playerBrain.Inventory.PlayerEquipment.CurrentGun)
            {
                _playerBrain.Inventory.PlayerEquipment.CurrentGun.Fire();
            }
        }
    }
}