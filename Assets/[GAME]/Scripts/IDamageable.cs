namespace _GAME_.Scripts
{
    public interface IDamageable
    {
        public Side GetSide();
        public void TakeDamage(float damage);
    }
}


public enum Side
{
    Player = 0,
    Enemy = 1,
    Mine = 2
}