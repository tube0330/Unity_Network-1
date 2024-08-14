using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State { READY, EMPTY, RELOADING }
    public State gunstate { get; private set; }

    public Transform firePos;
    public ParticleSystem muzzleFlash;  //총구 화염 효과
    public ParticleSystem shellEjectEffect; //탄피 효과
    LineRenderer lineRenderer;
    AudioSource source;
    AudioClip shotClip;
    AudioClip reloadClip;

    public float damage = 25f;
    float fireDistance = 50f;       //사정거리
    public int remainAmmo = 100;    //남은 탄약
    public int magCapacity = 25;    //탄창 용량
    public int curmagAmmo;          //현재 탄창에 남아 있는 탄알
    public float timeBetweenShot = 0.1f;   //탄알 발사 간격
    public float reloadTime = 1.0f; //재장전 소요 시간
    float lastFireTime;             //총을 마지막으로 발사한 시점

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        source = GetComponent<AudioSource>();

        lineRenderer.positionCount = 2; //사용할 점을 두 개로 변경
        lineRenderer.enabled = false;
    }

    void OnEnable()
    {
        curmagAmmo = magCapacity;  //25
        gunstate = State.READY;
        lastFireTime = 0f;
    }

    void Fire() //발사 시도
    {
        if (gunstate == State.READY && Time.time >= lastFireTime + timeBetweenShot)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    void Shot() //실제 발사 처리
    {
        RaycastHit hit;
        //Ray ray = new Ray(firePos.position, firePos.forward);
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(firePos.position, firePos.forward/*ray*/, out hit, fireDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();  //충돌한 상대방으로부터 IDamageable 오브젝트 가져오기 시도
            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal); //상대방의 OnDamage() 함수 호출해 상대방에게 Damage를 줌
            }

            hitPos = hit.point; //Ray가 충돌한 위치 저장
        }

        else
        {
            hitPos = firePos.position + firePos.forward * fireDistance; //Ray가 충돌하지 않았다면, 50만큼의 거리를 충돌 위치로 설정
            //lineRenderer.SetPosition(1, ray.GetPoint(fireDistance));
        }
    }

    IEnumerator ShotEffect(Vector3 hitPos)
    {
        lineRenderer.enabled = true;
        muzzleFlash.Play();
        shellEjectEffect.Play();
        source.PlayOneShot(shotClip);
        lineRenderer.SetPosition(0, firePos.position);  //선의 시작점을 총구의 위치로 잡음
        lineRenderer.SetPosition(1, hitPos);            //선의 끝점은 입력으로 들어온 충돌 위치로 설정
        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;
    }

    public bool Reload()    //재장전 시도
    {
        if (gunstate == State.RELOADING || remainAmmo <= 0 || curmagAmmo >= magCapacity)
        {   //재장전 | 남은 탄약이 없음 | 탄창에 탄알이 가득 참
            return false;
        }
        else
        {
            StartCoroutine(ReloadRoutine());
            return true;
        }
    }

    IEnumerator ReloadRoutine() //실제 재장전 처리를 진행
    {
        gunstate = State.RELOADING;
        source.PlayOneShot(reloadClip);
        yield return new WaitForSeconds(reloadTime);
        
        int ammoToFill = magCapacity - curmagAmmo;  //탄창에 채울 탄알 계산

        if (remainAmmo < ammoToFill)    //탄창에 채워야 할 탄알이 남은 탄알보다 많다면 채워야 할 탄알수를 남은 탄알수에 맞추어서 줄임
            ammoToFill = remainAmmo;

        curmagAmmo += ammoToFill;
        remainAmmo -= ammoToFill;
        gunstate = State.READY;
    }
}
