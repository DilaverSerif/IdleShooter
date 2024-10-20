using _GAME_.Scripts.Gun;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public abstract class Bullet<T> : BulletBase
{
    protected T CurrentTarget;
}

public abstract class BulletBase : MonoBehaviour
{
    public InventoryItem bulletType;
    protected Rigidbody Rigidbody;
    protected Collider BaseCollider;
    public Vector2 bulletRangeMinMax;
    public Vector2 bulletLifeTimeMinMax;

    protected float BulletRange()
    {
        return Random.Range(bulletRangeMinMax.x, bulletRangeMinMax.y);
    }

    protected float BulletLifeTime()
    {
        return Random.Range(bulletLifeTimeMinMax.x, bulletLifeTimeMinMax.y);
    }

    public int damage;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        BaseCollider = GetComponent<Collider>();
    }
    
    public abstract void Fire(int extraDamage = 0);
}