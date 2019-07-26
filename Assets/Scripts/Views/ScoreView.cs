using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    private GameObject scoreHandler;
    public void UpdateScore(int pointsValue)
    {
        if (scoreHandler!=null)
        {
            scoreHandler.GetComponent<UnityEngine.UI.Text>().text = pointsValue.ToString();
        }
        else
        {
            scoreHandler = GameObject.Find("AmountOfScore");
        }
    }
}
