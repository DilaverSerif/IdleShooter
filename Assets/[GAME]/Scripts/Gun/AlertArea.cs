using DG.Tweening;
using UnityEngine;
namespace _GAME_.Scripts.Gun
{
    public enum AlertAreaType
    {
        Circle,
        Box,
        Cone,
    }
    
    public abstract class AlertArea : MonoBehaviour
    {
        public AlertAreaType alertAreaType;
        public abstract Tween OpenAnim(SpawnAlertAreaData spawnAlertAreaData);
        public abstract Tween CloseAnim();
    }
}