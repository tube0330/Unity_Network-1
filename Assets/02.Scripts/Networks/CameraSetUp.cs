using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Cinemachine;

public class CameraSetUp : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)  //Photon Network상의 PhotonView가 자기자신의 것이라면 == 자신이 localplayer일 경우
        {
            CinemachineVirtualCamera followcam = FindObjectOfType<CinemachineVirtualCamera>();

            //가상카메라의 추적대상을 자신의 transform으로 설정
            followcam.Follow = transform;
            followcam.LookAt = transform;
        }
    }
}
