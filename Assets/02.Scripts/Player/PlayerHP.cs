using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHP : LivingEntity
{
    public Slider HPslider;             //체력 표시 할 슬라이더
    public AudioClip hitClip;           //피격 소리
    public AudioClip itemPickupClip;    //아이템 줍는 소리
    public AudioClip deathClip;         //사망 시 소리
    AudioSource source;
    Animator ani;
    PlayerMovement c_movement;
    PlayerShooter c_shooter;

    readonly int hashDie = Animator.StringToHash("Die");

    void Awake()
    {
        source = GetComponent<AudioSource>();
        ani = GetComponent<Animator>();
        c_movement = GetComponent<PlayerMovement>();
        c_shooter = GetComponent<PlayerShooter>();
    }

    protected override void OnEnable()  //물려받은것도 쓸건데 override해서 다르게도 쓸 예정
    {
        base.OnEnable();    //LivingEntity.OnEnable()호출
        HPslider.gameObject.SetActive(true);
        HPslider.maxValue = startHP;
        HPslider.value = HP;
        c_movement.enabled = true;
        c_shooter.enabled = true;
    }

    [PunRPC]
    public override void RestoreHP(float addHP)
    {
        base.RestoreHP(addHP);    //LivingEntity.RestoreHP()호출

        HPslider.value = HP;
    }

    [PunRPC]
    public override void OnDamage(float damage, Vector3 hitPos, Vector3 hitDirection)
    {
        if (!isDead)
            source.PlayOneShot(hitClip);

        base.OnDamage(damage, hitPos, hitDirection);    //LivingEntity.OnDamage()호출

        HPslider.value = HP;
    }

    public override void Die()
    {
        base.Die();    //LivingEntity.Die()호출

        HPslider.gameObject.SetActive(false);
        source.PlayOneShot(deathClip, 1.0f);
        ani.SetTrigger(hashDie);
        c_movement.enabled = false;
        c_shooter.enabled = false;

        Invoke("Respawn", 5f);
    }

    public void Respawn()   //플레이어 사망 후 5초 후에 부활
    {
        if (photonView.IsMine)
        {
            Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;  //원점에서 반경 5유닛 내부의 랜덤 위치 지정
            randomSpawnPos.y = 0f;
            transform.position = randomSpawnPos;
        }
        gameObject.SetActive(false);    //OnDisable() 호출하기위해
        //GameManager.G_instance.ReStartGame();
        UIManager.u_Instance.RestartGame();
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) //아이템과 충돌한 경우 해당 아이템을 사용하도록 처리하는 함수
    {
        if (!isDead)
        {
            IItem item = other.GetComponent<IItem>();

            if (item != null)
            {
                /* 호스트만 아이템 사용 가능
                 * 호스트에서 아이템 사용 후 사용된 효과를 모든 클라이언트에 동기화시킴
                 */
                 
                if (PhotonNetwork.IsMasterClient)
                    item.Use(gameObject);
            }
            source.PlayOneShot(itemPickupClip, 1.0f);
        }
    }
}