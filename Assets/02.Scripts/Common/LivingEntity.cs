using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startHP = 100f;                //시작 체력
    public float HP { get; protected set; }     //현재 체력
    public bool isDead { get; protected set; }
    public event Action OnDeath;                //사망시 발동 할 이벤트 선언

    [PunRPC]    //호스트(방장) => 모든 클라이언트 간에 사망 상태를 순서대로 동기화하는 메서드
    public void ApplyUpdateHP(float newHP, bool newDead)
    {
        HP = newHP;
        isDead = newDead;
    }

    protected virtual void OnEnable()   //생명체가 활성화 될 때 상태 리셋, 가상메서드
    {
        isDead = false;
        HP = startHP;
    }

    [PunRPC]    //호스트에서 단독 실행되고 호스트를 통해 다른 클라이언트에서 일괄 실행
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            HP -= damage;
            photonView.RPC("ApplyUpdateHP", RpcTarget.Others, HP, isDead);
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal);
        }

        if (HP <= 0 && !isDead)
            Die();
    }

    [PunRPC]
    public virtual void RestoreHP(float addHP)
    {
        if (isDead) return;

        if (PhotonNetwork.IsMasterClient)    //호스트인경우
        {
            HP += addHP;
            photonView.RPC("ApplyUpdateHP", RpcTarget.Others, HP, isDead);  //서버에서 클라이언트로 동기화
            photonView.RPC("RestoreHP", RpcTarget.Others, addHP);   //다른 클라이언트도 RestoreHP를 실행하도록 함
        }
    }

    public virtual void Die()
    {
        if (OnDeath != null)
            OnDeath();

        isDead = true;
    }
}