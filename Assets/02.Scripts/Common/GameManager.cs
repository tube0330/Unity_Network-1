using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager g_instance;
    public static GameManager G_instance
    {
        get
        {
            if (g_instance == null)
                g_instance = FindObjectOfType<GameManager>();

            return g_instance;
        }
    }

    public bool isGameOver
    {
        get; private set;
    }

    int score = 0;  //현재 게임 점수

    void Awake()
    {
        /* if (G_instance == null)
            G_instance = this;

        else if (G_instance != this)
            Destroy(this.gameObject); */

        if (g_instance != null)
            Destroy(gameObject);
    }

    void Start()
    {
        FindObjectOfType<PlayerHP>().OnDeath += EndGame;
    }

    public void AddScore(int newScore)
    {
        score += newScore;
        UIManager.u_Instance.UpdateScoreText(score);
    }

    public void EndGame()
    {
        isGameOver = true;
        UIManager.u_Instance.SetActiveGameOverUI(isGameOver);
    }
}
