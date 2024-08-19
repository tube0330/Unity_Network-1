using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
    public int score = 10;

    public void Use(GameObject target)
    {
        GameManager.G_instance.AddScore(score);
        Destroy(gameObject);
    }
}
