using UnityEngine.UI;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlManager : MonoBehaviour
{
    private int userProgress;
    public static int currentLvl { get; private set; }

    public GameObject levelHandler;

    void Start()
    {
        currentLvl = 0;
        userProgress = PlayerPrefs.GetInt("PlayerProgress", 0);
        for (int i = 0; i <= userProgress; i++)
        {
            var button = levelHandler.transform.GetChild(i).GetComponent<Button>();
            button.interactable = true;
        }
    }
    public void LoadLevel(int i)
    {
        currentLvl = i;
        SceneManager.LoadSceneAsync("GameplayScene");
    } 
    public void Exit()
    {
        Application.Quit();
    }
}
