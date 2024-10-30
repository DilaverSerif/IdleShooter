using _GAME_.Scripts.Gun;
using DG.Tweening;

public class CircleAlertArea : AlertArea
{
    public override Tween OpenAnim(SpawnAlertAreaData spawnAlertAreaData)
    {
        return transform.DOScale(1, .1f);
    }
    public override Tween CloseAnim()
    {
        return transform.DOScale(0, .1f);
    }
}
