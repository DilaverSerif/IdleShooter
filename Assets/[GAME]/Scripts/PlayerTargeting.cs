namespace _GAME_.Scripts
{
    public class PlayerTargeting : Targeting<EnemyHealth>
    {
        private void Update()
        {
            FindTarget();
        }
    }
}