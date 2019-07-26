using UniRx;
using UnityEngine;

public class BulletPullPresenter : MonoBehaviour
{
    public ReactiveCollection<BulletPresenter> allBulletsPresenter { get; private set; }

    private void Start()
    {
        allBulletsPresenter = new ReactiveCollection<BulletPresenter>();
        MessageBroker.Default
            .Receive<DeathNotice>()
            .Where(msg => msg.objectType == SpaceObjectType.bulletPresenter)
            .Subscribe(msg =>
            {
                if (allBulletsPresenter.Contains(msg.sender as BulletPresenter))
                {
                    allBulletsPresenter.Remove(msg.sender as BulletPresenter);
                }
            })
            .AddTo(this);
        MessageBroker.Default
            .Receive<ShootNotice>()
            .Subscribe(msg =>
            {
                AddBulletPresenter(msg.startPosition);
            })
            .AddTo(this);
    }

    public void AddBulletPresenter(Vector3 startPosition)
    {
        var bullet = new GameObject("BulletPresenter").AddComponent<BulletPresenter>();
        bullet.startPosition = startPosition;
        bullet.SubscribeOnCollection(this);
        allBulletsPresenter.Add(bullet);
    }
}
