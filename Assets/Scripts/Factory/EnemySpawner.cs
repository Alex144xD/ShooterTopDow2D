using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Factory host (GameObject que tiene una factory que implemente IFactory<GameObject>)")]
    [SerializeField] private GameObject factoryHost;

    private IFactory<GameObject> factory;

    [Header("Spawn")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField, Min(0.01f)] private float spawnInterval = 5f;

    private float timer;

    private void Awake()
    {
        if (!factoryHost)
        {
            Debug.LogError("[EnemySpawner] Asigna 'factoryHost'.");
            return;
        }

        factory = factoryHost.GetComponent<IFactory<GameObject>>();
        if (factory == null)
        {
            Debug.LogError("[EnemySpawner] 'factoryHost' no tiene un componente que implemente IFactory<GameObject>.");
        }
    }

    private void Update()
    {
        if (factory == null) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("[EnemySpawner] No hay spawnPoints configurados.");
            return;
        }

        Transform p = spawnPoints[Random.Range(0, spawnPoints.Length)];
        factory.CreateAt(p.position, p.rotation); 
    }
}