using _GAME_.Scripts;
using TMPro;
using UnityEngine;

public class Hexagon : Damageable
{
    public Transform hexagon;
    public WeaponTrigger weaponTrigger;
    public TextMeshPro healthText;
    private Collider _collider;

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider>();
    }

    public override Side GetSide()
    {
        return Side.Enemy;
    }

    private void Start()
    {
        weaponTrigger.OpenTrigger(false);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        var localScale = hexagon.localScale;
        localScale.y = CurrentHealth;
        healthText.text = CurrentHealth.ToString("F2");
        hexagon.localScale = localScale;
    }
    
    protected override void Die()
    {
        _collider.enabled = false;
        lifeState = LifeState.Dead;
        weaponTrigger.OpenTrigger();
    }
}
