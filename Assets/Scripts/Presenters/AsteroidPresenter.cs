using UniRx;
using UnityEngine;

public class AsteroidPresenter : MonoBehaviour, ISpaceDestroyable
{
    public AsteroidType type { get; set; }
    public Vector3 position { get; set; }
    private AsteroidModel asModel;
    private AsteroidView asView;
    private CompositeDisposable disposables;

    private void Start()
    {
        asModel = new AsteroidModel(type, position);
        asView = CreateAsteroidView(type, position);
        disposables = new CompositeDisposable();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                asModel.UpdatePosition(-1*Time.deltaTime);
            })
            .AddTo(disposables);
        asModel.position
            .ObserveEveryValueChanged(position => position.Value)
            .Subscribe(posValue =>
            {
                asView.UpdatePosition(posValue);
            })
            .AddTo(disposables);
        asView.isCollided
            .ObserveEveryValueChanged(position => position.Value)
            .Subscribe(posValue =>
            {
                asModel.LoseHp();
                asView.CloseFlag();
            })
            .AddTo(disposables);
        MessageBroker.Default
            .Receive<DeathNotice>()
            .Where(msg => msg.sender == asModel && msg.objectType==SpaceObjectType.asteroid)
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

    private AsteroidView CreateAsteroidView(AsteroidType type, Vector3 position)
    {
        var asteroid = new GameObject(type.ToString()+"Asteroid");
        asteroid.transform.position = position + new Vector3(0,1,0);
        asteroid.layer = 9;
        asteroid.transform.localScale = new Vector3(4,4,1);
        asteroid.AddComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Meteors/meteorBrown_"+type.ToString()+Random.Range(1,3).ToString());
        asteroid.AddComponent<BoxCollider2D>().isTrigger = true;
        asteroid.AddComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        return asteroid.AddComponent<AsteroidView>();
    }

    public void SendDeathNotice()
    {
        MessageBroker.Default
            .Publish<DeathNotice>(DeathNotice.Create(
                this,
                SpaceObjectType.asteroidPresenter
            ));
    }
    public void SubscribeOnCollection(AsteroidPullPresenter pull, AsteroidPresenter thisObject)
    {
        pull.allAsteroidPresenter
            .ObserveRemove()
            .Where(asteroid => asteroid.Value == this)
            .Subscribe(_ => {
                Object.Destroy(this.gameObject);
            })
            .AddTo(thisObject);
    }
    public void SelfDestruct()
    {
        asView.SelfDestruct();
        disposables.Dispose();
        SendDeathNotice();
    }
}
