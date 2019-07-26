using UniRx;
using UnityEngine;

public class AsteroidPullPresenter : MonoBehaviour
{
    public AsteroidType asteroidType { get; set; }
    public ReactiveCollection<AsteroidPresenter> allAsteroidPresenter { get; private set; }

    private void Start()
    {
        allAsteroidPresenter = new ReactiveCollection<AsteroidPresenter>();
        MessageBroker.Default
            .Receive<DeathNotice>()
            .Where(msg => msg.objectType == SpaceObjectType.asteroidPresenter)
            .Subscribe(msg =>
            {
                if (allAsteroidPresenter.Contains(msg.sender as AsteroidPresenter))
                {
                    allAsteroidPresenter.Remove(msg.sender as AsteroidPresenter);
                }
            })
            .AddTo(this);
    }

    public void AddAsteroidPresenter()
    {
        var asteroid = new GameObject("AsteroidPresenter").AddComponent<AsteroidPresenter>();
        asteroid.type = asteroidType;
        asteroid.position = GenerateAsteroidPosition();
        asteroid.SubscribeOnCollection(this, asteroid);
        allAsteroidPresenter.Add(asteroid);
    }

    private Vector3 GenerateAsteroidPosition()
    {
        Vector3 newPosition;
        bool isCorrect = false;
        do
        {
            newPosition = new Vector3(Random.Range(-11.0f + (float)asteroidType / 2.0f, 11.0f - (float)asteroidType / 2.0f), 9 ,-1);
            isCorrect = true;
            foreach (var asteroid in allAsteroidPresenter)
            {
                if ((asteroid.position - newPosition).magnitude < (float)asteroidType)
                {
                    isCorrect = false;
                    break;
                }
            }
        } while (isCorrect==false);
        return newPosition;
    }
}
