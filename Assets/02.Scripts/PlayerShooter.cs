using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*IK: 왼손 오른손이 무기에 정확하게 부착되게 총알발사*/

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    public Transform gunPivot;  //총 배치 기준점
    public Transform leftHandMound; //총의 왼쪽 손잡이 위치
    public Transform rightHandMound;    //총의 오른쪽 손잡이 위치
    PlayerInput playerInput;
    Animator ani;

    readonly int hashReload = Animator.StringToHash("Reload");

    void OnEnable()
    {
        gun.gameObject.SetActive(true); //이 스크립트가 활성화 될 때 gun 스크립트 gameobject도 함께 활성화
    }

    void Start()
    {
        ani = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (playerInput.fire)
        {
            gun.Fire();
        }

        else if (playerInput.reload)
        {
            if (gun.Reload())
                ani.SetTrigger(hashReload);
        }

        UpdateUI();
    }

    void UpdateUI()
    {

    }

    void OnAnimatorIK(int layerIndex)
    {
        gunPivot.position = ani.GetIKHintPosition(AvatarIKHint.RightElbow); //총이 기준점 gunpivot을 3D모델의 오른쪽 팔꿈치 위치로 이동

        /*IK 사용해 왼손의 위치와 회전을 총의 왼쪽 손잡이에 맞춤*/
        ani.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0f/*쓸거면 1.0*/);
        ani.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
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
        gun.gameObject.SetActive(false);
    }
}
