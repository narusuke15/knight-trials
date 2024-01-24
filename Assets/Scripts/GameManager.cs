using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public TMPro.TextMeshProUGUI HpText;
    public TMPro.TextMeshProUGUI TimerText;
    public GameObject GameOverPrefabs;
    public GameObject MissionClearedPrefabs;

    public static bool GameStarted = true;

    protected bool isDead = false;

    public void TimerLoop()
    {

    }

    public float gameTime = 0;

    void Update()
    {
        if (GameStarted)
        {
            gameTime += Time.deltaTime;
        }
        DisplayTimer(gameTime);
    }

    public void SetHealth(int amount)
    {
        if (amount < 0)
        {
            amount = 0;
        }
        HpText.text = "HP : " + amount + "/10";
        if (amount <= 0 && !isDead)
        {
            isDead = true;
            Instantiate(GameOverPrefabs);
        }
    }

    void DisplayTimer(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
