using UnityEngine;

public class EnemyFactory : MonoBehaviour, IFactory<GameObject>
{
    [SerializeField] private GameObject enemyPrefab;

    public GameObject Create()
    {
        if (!enemyPrefab)
        {
            Debug.LogError("[EnemyFactory] Falta asignar enemyPrefab en el Inspector.");
            return null;
        }
        return Instantiate(enemyPrefab); 
    }

    public GameObject CreateAt(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!enemyPrefab)
        {
            Debug.LogError("[EnemyFactory] Falta asignar enemyPrefab en el Inspector.");
            return null;
        }
        return Instantiate(enemyPrefab, position, rotation, parent);
    }
}