namespace _GAME_.Scripts.HexagonGame
{
    public class HexagonHealth : Damageable
    {
        //Hexagon scripti burayı yönetiyor
        public override Side GetSide()
        {
            return Side.Mine;
        }
    }
}