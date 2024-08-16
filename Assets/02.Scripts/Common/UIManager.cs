using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    
    public Gun c_gun;
    Text ammoTxt;


    void Start()
    {
        ammoTxt = GameObject.Find("HUD Canvas").transform.GetChild(0).GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        ammoTxt.text = $"{c_gun.curMagAmmo}/{c_gun.remainAmmo}";
    }
}
