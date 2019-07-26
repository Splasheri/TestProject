using UniRx;
using UnityEngine;

public class SpaceshipPresenter : MonoBehaviour
{
    private SpaceshipModel ssModel;
    private SpaceshipView ssView;
    private CompositeDisposable disposables;

    private void Start()
    {
        ssModel = new SpaceshipModel(new Vector3(0,-9,-1));
        ssView = GameObject.Find("PlayerSpaceship").AddComponent<SpaceshipView>();
        disposables = new CompositeDisposable();

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                ssModel.UpdatePosition(Input.GetAxis("Vertical") * Time.deltaTime, Input.GetAxis("Horizontal") * Time.deltaTime);
            })
            .AddTo(disposables);

        ssModel.hp
            .ObserveEveryValueChanged(hp => hp.Value)
            .Where(hp => hp < 3)
            .Subscribe(hpValue =>
            {
                ssView.UpdateHP(hpValue);
                ssView.CloseFlag();
            })
            .AddTo(disposables);

        ssModel.position
            .ObserveEveryValueChanged(position => position.Value)
            .Subscribe(posValue =>
            {
                ssView.UpdatePosition(posValue);
            })
            .AddTo(disposables);

        ssView.isCollided
            .ObserveEveryValueChanged(isCollided => isCollided.Value)
            .Where(isCollidedValue => isCollidedValue == true)
            .Subscribe(isCollidedValue =>
            {
                ssModel.LoseHp();
            })
            .AddTo(disposables);

        MessageBroker.Default
            .Receive<DeathNotice>()
            .Where(msg => msg.objectType == SpaceObjectType.spaceship)
            .Subscribe(_ => {
                ssView.UpdateHP(0);
                SelfDestruct();
            })
            .AddTo(this);

        MessageBroker.Default
            .Receive<GameOverNotice>()
            .Subscribe(msg =>
            {
                disposables.Dispose();
            })
            .AddTo(this);
    }
    public void SelfDestruct()
    {
        ssView.SelfDestruct();
        disposables.Dispose();
        Object.Destroy(this.gameObject);
    }
}
