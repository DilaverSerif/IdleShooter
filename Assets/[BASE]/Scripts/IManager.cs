namespace _BASE_.Scripts
{
    public interface IManager
    {
        public void Enable();
        public void Disable();
        public int Priority { get; }
    }
}