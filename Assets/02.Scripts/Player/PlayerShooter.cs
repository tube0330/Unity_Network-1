using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

/*IK: 왼손 오른손이 무기에 정확하게 부착되게 총알발사*/

public class PlayerShooter : MonoBehaviourPun
{
    public Gun c_gun;
    public Transform gunPivot;          //총 배치 기준점
    public Transform leftHandMound;     //총의 왼쪽 손잡이 위치
    public Transform rightHandMound;    //총의 오른쪽 손잡이 위치

    PlayerInput c_playerInput;
    Animator ani;

    readonly int hashReload = Animator.StringToHash("Reload");

    void OnEnable()
    {
        c_gun.gameObject.SetActive(true); //이 스크립트가 활성화 될 때 gun 스크립트 gameobject도 함께 활성화
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        c_playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        if (c_playerInput.fire)
        {
            c_gun.Fire();
        }

        else if (c_playerInput.reload)
        {
            if (c_gun.Reload())
                ani.SetTrigger(hashReload);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (c_gun != null && UIManager.u_Instance != null)
            UIManager.u_Instance.UpdateAmmoText(c_gun.curMagAmmo, c_gun.remainAmmo);
    }

    void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = ani.GetIKHintPosition(AvatarIKHint.RightElbow); //gunpivot을 3D모델의 오른쪽 팔꿈치 위치로 이동

        /*IK 사용해 왼손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤*/
        ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f/*안쓸거면 0*/);
        ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
        ani.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMound.position);
        ani.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMound.rotation);

        /*IK 사용해 오른손의 위치와 회전을 총의 오른쪽 손잡이에 맞춤*/
        ani.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
        ani.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
        ani.SetIKPosition(AvatarIKGoal.RightHand, rightHandMound.position);
        ani.SetIKRotation(AvatarIKGoal.RightHand, rightHandMound.rotation);
    }

    void OnDisable()
    {
        c_gun.gameObject.SetActive(false);
    }
}
