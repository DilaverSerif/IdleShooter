using UnityEngine;

namespace _GAME_.Scripts
{
    public class WeaponPluginTrigger: TriggerBase
    {
        protected override void PlayerTrigger(PlayerInventory playerInventory)
        {
            Debug.Log("WeaponPluginTrigger");
            playerInventory.playerEquipment.currentGun.AddPlugin(weaponType);
        }
    }
}