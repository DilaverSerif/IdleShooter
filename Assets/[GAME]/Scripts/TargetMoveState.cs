using _BASE_.Scripts;
using Cysharp.Threading.Tasks;

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
            _playerBrain.Inventory.playerEquipment.currentGun?.StopFire();
            _playerBrain.CharacterController.SetLookDirection(PhysicsBasedCharacterController.lookDirectionOptions.targetDirection);
            
                        
            if (_playerBrain.Inventory.playerEquipment.currentGun)
                _playerBrain.Inventory
                    .playerEquipment.currentGun
                    .AutoFire(_playerBrain.Targeting.currentTarget).Forget();
        }
        
        
        public override void OnExit()
        {
            base.OnExit();
            if (_playerBrain.Inventory.playerEquipment.currentGun)
                _playerBrain.Inventory
                    .playerEquipment.currentGun
                    .StopFire();
        }
    }
}