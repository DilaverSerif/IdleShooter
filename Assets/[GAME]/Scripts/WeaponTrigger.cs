namespace _GAME_.Scripts
{
    public class WeaponTrigger : TriggerBase
    {
        protected override void PlayerTrigger(PlayerInventory playerInventory)
        {
            playerInventory.playerEquipment.EquipGun(weaponType);
        } 
    }
}