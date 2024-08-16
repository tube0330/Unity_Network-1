using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity   //player처럼 HP깎이고 죽고 회복할 수 있으니까 LivingEntity 상속
{
    public LayerMask whatIsTarget;         //추적 대상 레이어
    public LivingEntity targetEntity;   //추적 대상
    public ParticleSystem hitEff;
    public AudioClip deathClip;
    public AudioClip hitClip;
    AudioSource source;
    Animator ani;
    Renderer e_renderer;
    Rigidbody rb;
    NavMeshAgent pathfinder;    //경로 계산 AI agent
    
    public float damage = 20f;
    public float timeBetweenAttack = 0.5f;  //공격 쿨타임
    float lastAttackTime = 0f;

    readonly int hashHasTarget = Animator.StringToHash("hasTarget");
    readonly int hashDie = Animator.StringToHash("Die");

    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.isDead)    //추적 대상이 존재하고 대상이 사망하지 않았다면
                return true;

            return false;
        }
    }

    private void Awake()
    {
        pathfinder = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animator>();
        e_renderer = GetComponentInChildren<Renderer>();
        source = GetComponent<AudioSource>();
        hitEff = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(float setHP, float setDamage, float setSpeed, Color skinColor)
    {
        startHP = setHP;
        HP = setHP;
        damage = setDamage;
        pathfinder.speed = setSpeed;
        e_renderer.material.color = skinColor;
    }

    void Start()
    {
        //StartCoroutine(UpdatePath());
        InvokeRepeating("UpdatePath", 0.01f, 0.25f);    //0.01f초 후에 UpdatePath 메서드를 0.25초 간격으로 반복 호출
    }

    void Update()
    {
        ani.SetBool(hashHasTarget, hasTarget);
    }

    void UpdatePath()   //IEnumerator UpdatePath()
    {
        if (!isDead)
        {
            if (hasTarget)   //추적 대상이 있다면
            {
                pathfinder.isStopped = false;
                pathfinder.SetDestination(targetEntity.transform.position);
            }

            else
            {
                pathfinder.isStopped = true;
                Collider[] cols = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                for (int i = 0; i < cols.Length; i++)
                {
                    LivingEntity livingEntity = cols[i].GetComponent<LivingEntity>();

                    if (livingEntity != null && !livingEntity.isDead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }

            //yield return new WaitForSeconds(0.25f);
        }
    }

    public override void OnDamage(float damage, Vector3 hitPos, Vector3 hitNormal)
    {
        if (!isDead)
        {
            hitEff.transform.position = hitPos;
            hitEff.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEff.Play();
            source.PlayOneShot(hitClip);
        }

        base.OnDamage(damage, hitPos, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        Collider[] e_cols = GetComponents<Collider>();

        for (int i = 0; i < e_cols.Length; i++)
        {
            e_cols[i].enabled = false;  //다른 AI의 방해를 받지 않도록 모든 콜라이더 비활성화
            rb.isKinematic = true;
        }

        pathfinder.isStopped = true;
        pathfinder.enabled = false;
        source.PlayOneShot(deathClip);
        ani.SetTrigger(hashDie);
    }

    void OnTriggerStay(Collider other)
    {
        if (!isDead && Time.time >= lastAttackTime + timeBetweenAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>();

            if (attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time;
                Vector3 hitPoint = other.ClosestPoint(attackTarget.transform.position); //'other' Collider의 가장 가까운 지점을 'hitPoint'로 저장
                Vector3 hitNormal = transform.position - other.transform.position;

                attackTarget.OnDamage(damage, hitPoint, hitNormal); //'attackTarget'에 'damage'를 주는 OnDamage 메서드를 호출
            }
        }
    }
}
