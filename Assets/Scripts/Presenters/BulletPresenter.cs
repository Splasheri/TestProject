using UniRx;
using UnityEngine;

public class BulletPresenter : MonoBehaviour, ISpaceDestroyable
{
    public Vector3 startPosition { get; set; }
    private BulletModel bModel;
    private BulletView bView;
    private CompositeDisposable disposables;

    void Start()
    {
        bModel = new BulletModel(startPosition);
        bView = CreateBulletView(startPosition);
        disposables = new CompositeDisposable();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                bModel.UpdatePosition(1 * Time.deltaTime);
            })
            .AddTo(disposables);
        bModel.position
            .ObserveEveryValueChanged(position => position.Value)
            .Subscribe(posValue =>
            {
                bView.UpdatePosition(posValue);
            })
            .AddTo(disposables);
        bView.isCollided
            .ObserveEveryValueChanged(position => position.Value)
            .Subscribe(posValue =>
            {
                bModel.SendDeathNotice();
            })
            .AddTo(disposables);
        MessageBroker.Default
            .Receive<DeathNotice>()
            .Where(msg => msg.sender == bModel && msg.objectType == SpaceObjectType.bullet)
            .Subscribe(_ => {
                SelfDestruct();
            })
            .AddTo(disposables);
        MessageBroker.Default
            .Receive<GameOverNotice>()
            .Subscribe(msg =>
            {
                disposables.Dispose();
            })
            .AddTo(this);
    }

    private BulletView CreateBulletView(Vector3 position)
    {
        var asteroid = new GameObject("Bullet");
        asteroid.transform.position = position;
        asteroid.layer = 10;
        asteroid.transform.localScale = new Vector3(4, 4, 1);
        asteroid.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Bullet");
        asteroid.AddComponent<BoxCollider2D>().isTrigger = false;
        return asteroid.AddComponent<BulletView>();
    }
    public void SendDeathNotice()
    {
        MessageBroker.Default
            .Publish<DeathNotice>(DeathNotice.Create(
                this,
                SpaceObjectType.bulletPresenter
            ));
    }
    public void SubscribeOnCollection(BulletPullPresenter pull)
    {
        pull.allBulletsPresenter
            .ObserveRemove()
            .Where(asteroid => asteroid.Value == this)
            .Subscribe(_ =>
            {
                Object.Destroy(this.gameObject);
            })
            .AddTo(this);
    }
    public void SelfDestruct()
    {
        bView.SelfDestruct();
        disposables.Dispose();
        SendDeathNotice();
    }
}
