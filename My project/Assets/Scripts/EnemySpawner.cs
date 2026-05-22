using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private float spawnCooldown = 3f;

    [Header("Spawn Area")]
    [SerializeField] private Vector3 spawnBoxSize = new Vector3(10f, 1f, 10f);
    [SerializeField] private LayerMask groundLayer;

    private int currentEnemyCount = 0;
    private float spawnTimer = 0f;

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnCooldown)
        {
            if (currentEnemyCount < maxEnemies)
            {
                TrySpawnEnemy();
            }
            spawnTimer = 0f;
        }
    }

    private void TrySpawnEnemy()
    {
        Vector3 randomLocalPoint = new Vector3(
            Random.Range(-spawnBoxSize.x / 2f, spawnBoxSize.x / 2f),
            Random.Range(-spawnBoxSize.y / 2f, spawnBoxSize.y / 2f),
            Random.Range(-spawnBoxSize.z / 2f, spawnBoxSize.z / 2f)
        );

        Vector3 randomWorldPoint = transform.position + randomLocalPoint;

        if (NavMesh.SamplePosition(randomWorldPoint, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            GameObject spawnedEnemy = Instantiate(enemyPrefab, hit.position, Quaternion.identity);
            currentEnemyCount++;

            if(spawnedEnemy.TryGetComponent(out EnemyAI enemyAI))
            {
                enemyAI.AssignSpawner(this);
            }
        }
    }

    public void DecrementEnemyCount()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position, spawnBoxSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnBoxSize);
    }
}