using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager u_instance;
    public static UIManager u_Instance
    {
        get
        {
            if (u_instance == null)
                u_instance = FindObjectOfType<UIManager>();

            return u_instance;
        }
    }

    public Text ammoTxt;
    public Text waveTxt;
    public Text scoreTxt;
    public GameObject gameOverUI;


    void Start()
    {
        ammoTxt = GameObject.Find("HUD Canvas").transform.GetChild(0).GetChild(0).GetComponent<Text>();
        scoreTxt = GameObject.Find("HUD Canvas").transform.GetChild(1).GetComponent<Text>();
        waveTxt = GameObject.Find("HUD Canvas").transform.GetChild(2).GetComponent<Text>();
    }

    public void UpdateAmmoText(int curMagAmmo, int remainAmmo)
    {
        ammoTxt.text = curMagAmmo + "/" + remainAmmo;
    }

    public void UpdateScoreText(int addScore)
    {
        scoreTxt.text = "Score: " + addScore;
    }

    public void UpdateWaveText(int waves, int cnt)
    {
        waveTxt.text = "Wave " + waves + "\n EnemyLeft: " + cnt;
    }

    public void SetActiveGameOverUI(bool active)
    {
            gameOverUI.SetActive(active);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Main Scene(본인) 로드
    }
}
