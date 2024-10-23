using _BASE_.Scripts;
using _GAME_.Scripts.Gun;
namespace _GAME_.Scripts
{
    public class LootIdleState: PlayerState
    {
        public LootIdleState(PlayerBrain playerBrain, bool needsExitTime = false, bool isGhostState = false) : base(playerBrain, needsExitTime, isGhostState)
        {
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            _playerBrain.CharacterController.SetLookDirection(PhysicsBasedCharacterController.lookDirectionOptions.targetDirection);
            _playerBrain.Inventory.PlayerEquipment.EquipGun(InventoryItem.Stick);
        }
        
        
        public override void OnLogic()
        {
            base.OnLogic();
            
            if (_playerBrain.Inventory.PlayerEquipment.currentGun)
                _playerBrain.Inventory.PlayerEquipment.currentGun.AutoFire(_playerBrain.Targeting.currentTarget);
        }
        
        
    }
}