using _GAME_.Scripts;

public class PlayerHealthSystem : Damageable
{
    public override Side GetSide()
    {
        return Side.Player;
    }
}