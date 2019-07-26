using System.IO;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private SpaceshipPresenter ssPresenter;
    private ScorePresenter scPresenter;
    private BulletPullPresenter bulletPull;
    private AsteroidPullPresenter asteroidPull;
    private AsteroidType asteroidType;
    private CompositeDisposable disposables;
    private int playerProgress;
    private int lvl;
    private const int pointsToWin = 10;

    public GameObject resultWindow;
    public ReactiveProperty<int> points { get; private set; }

    void Start()
    {
        InitializeGameManager();
    }

    private void InitializeGameManager()
    {
        lvl = LvlManager.currentLvl;
        playerProgress = PlayerPrefs.GetInt("PlayerProgress", 0);
        switch (lvl)
        {
            case 0:
                asteroidType = AsteroidType.big;
                break;
            case 1:
                asteroidType = AsteroidType.medium;
                break;
            case 2:
                asteroidType = AsteroidType.small;
                break;
            default:
                asteroidType = AsteroidType.small;
                break;
        }
        points = new ReactiveProperty<int>(0);
        ssPresenter = new GameObject("SpaceshipPresenter").AddComponent<SpaceshipPresenter>();
        scPresenter = new GameObject("ScorePresenter").AddComponent<ScorePresenter>();
        scPresenter.SubscribeToManager(this);
        bulletPull = new GameObject("BulletPull").AddComponent<BulletPullPresenter>();
        asteroidPull = new GameObject("AsteroidPull").AddComponent<AsteroidPullPresenter>();
        asteroidPull.asteroidType = this.asteroidType;
        MessageBroker.Default
            .Receive<ScoringNotice>()
            .Subscribe(_ => {
                points.Value += (int)asteroidType;
                if (points.Value >= pointsToWin)
                {
                    GameOver(true);
                }
            })
            .AddTo(this);
        StartGame();
    }
    private void StartGame()
    {
        disposables = new CompositeDisposable();
        resultWindow.SetActive(false);
        points.Value = 0;
        Observable.Timer(System.TimeSpan.FromSeconds(3.2f - lvl))
            .RepeatSafe()
            .Subscribe(_ =>
            {
                asteroidPull.AddAsteroidPresenter();
            })
            .AddTo(disposables);
        MessageBroker.Default
            .Receive<DeathNotice>()
            .Where(msg => msg.objectType == SpaceObjectType.spaceship)
            .Subscribe(msg =>
            {
                if (points.Value < pointsToWin)
                {
                    GameOver(false);
                }
            })
            .AddTo(disposables);
    }
    private void GameOver(bool win)
    {
        MessageBroker.Default.Publish<GameOverNotice>(GameOverNotice.Create());
        resultWindow.SetActive(true);
        disposables.Dispose();
        Button menuButton;
        menuButton = resultWindow.transform.GetChild(1).GetChild(0).GetComponent<Button>();
        menuButton.onClick.AsObservable().Subscribe(_ => { BackToMenu(); });
        if (win)
        {
            if (lvl < 2) { playerProgress = lvl+1; }
            resultWindow.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "You won";            
        }
        else
        {
            resultWindow.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "You lose";
        }
        Save();
    }
    private void BackToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
    private void Save()
    {
        PlayerPrefs.SetInt("PlayerProgress", playerProgress);
    }
}
