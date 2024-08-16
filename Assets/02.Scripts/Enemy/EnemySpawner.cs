using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();
    public Enemy enemyPrefab;
    public Transform[] spawnPoints;
    public Color strongEnemySkinColor = Color.red;  //강한 적 AI가 가질 피부색
    int wave;

    public float damageMax = 30f;   //최대 공격력
    public float damageMin = 20f;   //최소 공격력
    public float MaxHP = 200f;
    public float MinHP = 100f;
    public float MaxSpeed = 3.0f;
    public float MinSpeed = 1.0f;
    
    void Update()
    {
        if (GameManager.G_instance != null && GameManager.G_instance.isGameOver) return;

        if (enemies.Count <= 0) //적을 다 없앴다면 다음 spawn 실행
        {
            SpawnWave();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        UIManager.u_Instance.UpdateWaveText(wave, enemies.Count);
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

        Enemy enemy = Instantiate(enemyPrefab, point.position, point.rotation);
        enemy.SetUp(HP, damage, speed, skinColor);

        enemies.Add(enemy);
        enemy.OnDeath += () => enemies.Remove(enemy);  //적이 죽으면 enemies list에서 제거
        enemy.OnDeath += () => Destroy(enemy.gameObject, 10f);
        enemy.OnDeath += () => GameManager.G_instance.AddScore(100);
    }

}