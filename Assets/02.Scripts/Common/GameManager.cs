using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
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

    public GameObject playerPrefab;

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

        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
        randomSpawnPos.y = 0f;
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)    //로컬 움직임 송신
            stream.SendNext(score);
        else
        {
            score = (int)stream.ReceiveNext();
            UIManager.u_Instance.UpdateScoreText(score);
        }
    }
}
