using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager u_instance;
    public Gun c_gun;
    Text ammoTxt;


    void Start()
    {
        u_instance = this;
        ammoTxt = GameObject.Find("HUD Canvas").transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        ammoTxt.text = $"{c_gun.curMagAmmo}/{c_gun.remainAmmo}";
    }
}
