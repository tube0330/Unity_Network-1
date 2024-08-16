using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startHP = 100f;                //시작 체력
    public float HP { get; protected set; }     //현재 체력
    public bool isDead { get; protected set; }
    public event Action OnDeath;                //사망시 발동 할 이벤트 선언

    protected virtual void OnEnable()   //생명체가 활성화 될 때 상태 리셋, 가상메서드
    {
        isDead = false;
        HP = startHP;
    }

    public void OnDamage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        HP -= damage;

        if (HP <= 0 && !isDead)
            Die();
    }

    public virtual void RestoreHP(float upHP)
    {
        if (isDead) return;

        HP += upHP;
    }

    public virtual void Die()
    {
        if(OnDeath != null)
            OnDeath();

        isDead = true;
    }
}
