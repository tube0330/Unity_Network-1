using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class EnemySpawner : MonoBehaviourPun, IPunObservable
{
    List<Enemy> enemies = new List<Enemy>();
    public Enemy enemyPrefab;
    public Transform[] spawnPoints;
    Color strongEnemySkinColor = Color.red;  //강한 적 AI가 가질 피부색
    int wave;

    float damageMax = 30f;   //최대 공격력
    float damageMin = 20f;   //최소 공격력
    float MaxHP = 200f;
    float MinHP = 100f;
    float MaxSpeed = 3.0f;
    float MinSpeed = 1.0f;
    int enemyCnt = 0;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)    //송신
        {
            stream.SendNext(enemies.Count);
            stream.SendNext(wave);
        }

        else //수신
        {
            enemyCnt = (int)stream.ReceiveNext();
            wave = (int)stream.ReceiveNext();
        }
    }

    void Awake()
    {
        PhotonPeer.RegisterType(typeof(Color), 128, ColorSerialization.SerializeColor, ColorSerialization.DeserializeColor);    //좀비 색상이 직렬화되었다가,,,,?
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)   //호스트만 적을 생성하도록, 클라이언트들은 호스트가 생성한 적을 동기화해서 받아옴
        {
            if (GameManager.G_instance != null && GameManager.G_instance.isGameOver) return;

            if (enemies.Count <= 0) //적을 다 없앴다면 다음 spawn 실행
                SpawnWave();

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (PhotonNetwork.IsMasterClient)
            UIManager.u_Instance.UpdateWaveText(wave, enemies.Count);

        else
            UIManager.u_Instance.UpdateWaveText(wave, enemyCnt);   //클라이언트는 적리스트를 갱신할 수 없으므로 호스트가 보내준 enemyCnt 이용 
    }

    void SpawnWave()    //현재 웨이브에 맞춰 적 생성
    {
        wave++;     //현재 Wave * 1.5를 반올림해서 적 생성
        int spawnCount = Mathf.RoundToInt(wave * 1.5f);

        for (int i = 0; i < spawnCount; i++)
        {
            float enemyIntensity = Random.Range(0f, 1f);  //적의 강도(세기)를 0% ~ 100%사이에서 랜덤으로 결정
            CreateEnemy(enemyIntensity);
        }

    }

    void CreateEnemy(float intensity)   //적 생성하고 추적할 대상을 할당
    {
        /*intensity가 0.5로 설정되면
         *HP, damage, speed는 각각 Min과 Max 값의 중간 값을 가지게 됨
         */
        float HP = Mathf.Lerp(MinHP, MaxHP, intensity);
        float damage = Mathf.Lerp(damageMin, damageMax, intensity);
        float speed = Mathf.Lerp(MinSpeed, MaxSpeed, intensity);

        Color skinColor = Color.Lerp(Color.white, strongEnemySkinColor, intensity);
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Enemy enemy = Instantiate(enemyPrefab, point.position, point.rotation);
        GameObject createdEnemy = PhotonNetwork.Instantiate(enemyPrefab.name, point.position, point.rotation);

        Enemy c_enemy = createdEnemy.GetComponent<Enemy>();

        //enemy.SetUp(HP, damage, speed, skinColor);
        photonView.RPC("SetUp", RpcTarget.All, HP, damage, speed, skinColor);

        enemies.Add(c_enemy);

        c_enemy.OnDeath += () => enemies.Remove(c_enemy);  //적이 죽으면 enemies list에서 제거
        c_enemy.OnDeath += () => StartCoroutine(DestroyAfter(c_enemy.gameObject, 10f));
        c_enemy.OnDeath += () => GameManager.G_instance.AddScore(100);
    }

    IEnumerator DestroyAfter(GameObject targetEnemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (targetEnemy != null)
            PhotonNetwork.Destroy(targetEnemy);
    }
}