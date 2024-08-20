using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Coin : MonoBehaviourPun, IItem
{
    public int score = 10;

    public void Use(GameObject target)
    {
        GameManager.G_instance.AddScore(score);

        //Destroy(gameObject);
        PhotonNetwork.Destroy(gameObject);
    }
}
