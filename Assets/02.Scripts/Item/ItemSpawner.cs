using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class ItemSpawner : MonoBehaviourPun
{
    public GameObject[] itemPrefabs;    //생성할 Item. 2개만 할거(탄약 증가, HP 회복)
    Transform playerTr;
    string playerTag = "Player";
    public float maxDist = 5f;  //플레이어 위치에서 아이템이 배치 될 최대 반경
    public float minCooltime = 2f;
    public float MaxCooltime = 7f;
    float spawnCooltime;    //생성 쿨타임
    float lastSpawnTime = 0f;    //마지막으로 아이템을 생성한 시간


    void Start()
    {
        playerTr = GameObject.FindWithTag(playerTag).GetComponent<Transform>();
        spawnCooltime = Random.Range(minCooltime, MaxCooltime); //2~7초 사이로 생성
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;   //호스트만 아이템 직접 생성 가능

        if (Time.time >= lastSpawnTime + spawnCooltime && playerTr != null)
        {
            lastSpawnTime = Time.time;
            spawnCooltime = Random.Range(minCooltime, MaxCooltime);
            Spawn();
        }

    }

    void Spawn()
    {
        Vector3 spawnPos = GetRandomPointOnNavMesh(playerTr.position, maxDist);
        spawnPos += Vector3.up * 0.5f; //아이템 위치를 Player보다 0.5f 올림
        GameObject ramdomItem = itemPrefabs[Random.Range(0, itemPrefabs.Length)];

        /* 자기자신만 생성되고 소멸
        GameObject createItem = Instantiate(ramdomItem, spawnPos, Quaternion.identity); //생성

        //Destroy(createItem, 5f);
        */

        GameObject item = PhotonNetwork.Instantiate(ramdomItem.name, spawnPos, Quaternion.identity);    //네트워크상에서 모든 클라이언트에 해당 아이템 생성
        StartCoroutine(DestroyAfter(item, 5f));
    }

    IEnumerator DestroyAfter(GameObject targetItem, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (targetItem != null)
            PhotonNetwork.Destroy(targetItem);
    }

    /*Navmesh 위에서 Random한 위치를 반환하는 메서드
     *center를 중심으로 거리 반경 안에서 랜덤한 위치를 찾음
     */
    Vector3 GetRandomPointOnNavMesh(Vector3 center, float distance)
    {
        Vector3 randomPos = Random.insideUnitSphere/*반지름이 1인 구의 내부에서 임의의 점을 반환*/ * distance + center;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, distance, NavMesh.AllAreas); //randomPos에서 가장 가까운 NavMesh 상의 유효한 지점을 찾는 데 사용

        return hit.position;
    }
}
