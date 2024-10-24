using _BASE_.Scripts;
using Cysharp.Threading.Tasks;

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
            
            if (_playerBrain.Inventory.PlayerEquipment.currentGun)
                _playerBrain.Inventory
                    .PlayerEquipment.currentGun
                    .AutoFire(_playerBrain.Targeting.currentTarget).Forget();
        }
    }
}