using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State { READY, EMPTY, RELOADING }
    public State gunstate { get; private set; }

    public Transform firePos;
    public ParticleSystem muzzleFlash;      //총구 화염 효과
    public ParticleSystem shellEjectEffect; //탄피 효과
    LineRenderer lineRenderer;
    AudioSource source;
    AudioClip shotClip;
    AudioClip reloadClip;

    public float damage = 25f;
    float fireRange = 50f;          //사정거리
    public int remainAmmo = 100;    //남은 탄약
    public int magCapacity = 25;    //탄창 용량 25
    public int curMagAmmo;          //현재 탄창에 있는 탄약
    public float timeBetweenShot = 0.1f;   //발사 쿨타임 0.1초
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
        curMagAmmo = magCapacity;  //25
        gunstate = State.READY;
        lastFireTime = 0f;
    }

    public void Fire() //발사 시도
    {
        if (gunstate == State.READY && Time.time >= lastFireTime + timeBetweenShot)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    void Shot() //실제 발사 처리
    {
        RaycastHit hit; //레이캐스트가 충돌한 정보를 담고 있는 구조체 선언
        //Ray ray = new Ray(firePos.position, firePos.forward);
        Vector3 hitPos = Vector3.zero;
        if (Physics.Raycast(firePos.position, firePos.forward/*ray*/, out hit, fireRange))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();  //Raycast가 충돌한 물체의 Collider에서 IDamageable 컴포넌트 찾음
            
            if (target != null) //충돌한 물체가 IDamageable 컴포넌트가 있다면
                target.OnDamage(damage, hit.point, hit.normal); //OnDamage() 함수 호출해 상대방에게 Damage를 줌

            hitPos = hit.point; //Ray가 충돌한 지점 저장
        }

        else
        {
            hitPos = firePos.position + (firePos.forward * fireRange); //Ray가 충돌하지 않았다면, 최대거리(fireRange)를 충돌 위치로 설정
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
        if (gunstate == State.RELOADING || remainAmmo <= 0 || curMagAmmo >= magCapacity)
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

        int ammoToFill = magCapacity - curMagAmmo;  //탄창에 채울 탄알 계산

        if (remainAmmo < ammoToFill)    //탄창에 채워야 할 탄알이 남은 탄알보다 많다면 채워야 할 탄알수를 남은 탄알수에 맞추어서 줄임
            ammoToFill = remainAmmo;

        curMagAmmo += ammoToFill;
        remainAmmo -= ammoToFill;
        gunstate = State.READY;
    }
}
