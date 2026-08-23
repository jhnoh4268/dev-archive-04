using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Vector3 centerPoint = Vector3.zero;
    public float spawnRadius = 10.0f;
    public GameObject monsterPrefab;
    public GameObject player;
    private float spawnInterval = 3.0f;

    public float time;

    private void Start()
    {
        spawnInterval = MANAGER.DB.levelDesign.monsterSpawnRate;
        MANAGER.Session.onBossTime += SpawnBossMonster;
    }

    private void OnDestroy()
    {
        MANAGER.Session.onBossTime -= SpawnBossMonster;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if(time > spawnInterval)
        {
            time = 0.0f;
            SpawnMonsterAtEdge();
        }
    }

    void SpawnBossMonster()
    {
        SpawnMonsterAtEdge("Skeleton_Boss");
    }

    void SpawnMonsterAtEdge(string id = null)
    {
        Vector3 spawnPoint = GetRandomPointOnCircleEdge(player.transform.position, spawnRadius);

        var monster = MANAGER.poolManager.PoolingObj("Monster").Get((value)=>
        {
            value.transform.position = spawnPoint;
            value.GetComponent<Monster>().Initalize(player.transform, string.IsNullOrEmpty(id) ? "Skeleton_01" : id);
        });

    }

    Vector3 GetRandomPointOnCircleEdge(Vector3 center, float radius)
    {
        float angle = Random.Range(0.0f, Mathf.PI * 2f);

        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        return new Vector3(center.x + x, 0, center.z + z);
    }
}
