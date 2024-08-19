using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*입력과 움직임을 분리해 스크립트 생성*/

public class PlayerInput : MonoBehaviourPun
{
    public string moveAxisName = "Vertical";        //W, S: 전진 후진
    public string rotateAxisName = "Horizontal";    //A, D: 회전
    public string fireButtonName = "Fire1";     //L_Ctrl, mouse0
    public string reloadButtonName = "Reload";  //edit - progectSettings - Input Manager - Axes - Fire3 Duplicate - Name : Reload

    #region 키 관련 property
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    #endregion

    void Start()
    {

    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (photonView.IsMine && GameManager.G_instance != null && GameManager.G_instance.isGameOver)
        {
            move = 0f;
            rotate = 0f;
            fire = false;
            reload = false;

            return;
        }

        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
    }
}
