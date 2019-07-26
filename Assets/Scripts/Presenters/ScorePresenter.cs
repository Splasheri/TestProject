using UniRx;
using UnityEngine;

public class ScorePresenter : MonoBehaviour
{
    private ScoreView scView;
    public ScorePresenter()
    {
        scView = new ScoreView();
    }
    public void SubscribeToManager(GameManager gm)
    {
        gm.points.ObserveEveryValueChanged(points => points.Value)
            .Subscribe(pointsValue =>
            {
                scView.UpdateScore(pointsValue);
            })
            .AddTo(this);
    }
}
