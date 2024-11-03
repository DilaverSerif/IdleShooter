using DG.Tweening;
using UnityEngine;
namespace _BASE_.Scripts
{
    public abstract class MainMenu : MonoBehaviour
    {
        public abstract Tween OpenMenu();
        public abstract Tween CloseMenu();
    }
}