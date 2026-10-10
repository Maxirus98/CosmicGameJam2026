using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private float spawnRateMin = 0.2f;

    [SerializeField]
    private float spawnRateMax = 1f;
    private float spawnRate;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            SpawnEnemy();
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
            timer = spawnRate;
        }
    }

    void SpawnEnemy()
    {
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        Vector3 camPos = cam.transform.position;
        Vector2 spawnPos = Vector2.zero;

        // Random side of the camera
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // Left
                spawnPos = new Vector2(camPos.x - camWidth - 1f, Random.Range(camPos.y - camHeight, camPos.y + camHeight));
                break;
            case 1: // Right
                spawnPos = new Vector2(camPos.x + camWidth + 1f, Random.Range(camPos.y - camHeight, camPos.y + camHeight));
                break;
            case 2: // Top
                spawnPos = new Vector2(Random.Range(camPos.x - camWidth, camPos.x + camWidth), camPos.y + camHeight + 1f);
                break;
            case 3: // Bottom
                spawnPos = new Vector2(Random.Range(camPos.x - camWidth, camPos.x + camWidth), camPos.y - camHeight - 1f);
                break;
        }

        Instantiate(enemy, new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity);
    }
}
