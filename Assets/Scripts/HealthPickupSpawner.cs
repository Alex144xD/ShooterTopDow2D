using UnityEngine;

public class HealthPickupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthPickupPrefab;
    [SerializeField] private float spawnInterval = 10f;
    [SerializeField] private float pickupLifetime = 15f;

    private float timer;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnPickup();
            timer = 0f;
        }
    }

    private void SpawnPickup()
    {
        Vector2 spawnPosition = GetRandomPositionInsideCameraView();

        GameObject pickup = Instantiate(healthPickupPrefab, spawnPosition, Quaternion.identity);
        Destroy(pickup, pickupLifetime); // Se destruye automáticamente después de un tiempo
    }

    private Vector2 GetRandomPositionInsideCameraView()
    {
        float cameraHeight = 2f * mainCam.orthographicSize;
        float cameraWidth = cameraHeight * mainCam.aspect;

        Vector3 camPos = mainCam.transform.position;

        float x = Random.Range(camPos.x - cameraWidth / 2f, camPos.x + cameraWidth / 2f);
        float y = Random.Range(camPos.y - cameraHeight / 2f, camPos.y + cameraHeight / 2f);

        return new Vector2(x, y);
    }
}