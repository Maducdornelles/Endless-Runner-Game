using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    public GameObject spikePrefab;
    public Transform groundTransform;            // arraste o Ground aqui
    public float spawnDistanceFromCamera = 1.5f;
    public float minSpawnTime = 1.2f;
    public float maxSpawnTime = 2.2f;

    Camera cam;
    float nextSpawnIn;
    float spikeHalfHeight = 0f;

    void Start()
    {
        cam = Camera.main;
        if (spikePrefab != null)
        {
            SpriteRenderer sr = spikePrefab.GetComponent<SpriteRenderer>();
            if (sr != null) spikeHalfHeight = sr.bounds.extents.y;
            Debug.Log("[SPAWNER] spikeHalfHeight = " + spikeHalfHeight);
        }
        ScheduleNext();
    }

    void Update()
    {
        nextSpawnIn -= Time.deltaTime;
        if (nextSpawnIn <= 0f)
        {
            float z = -cam.transform.position.z;
            Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1f, 0.5f, z));
            float spawnX = rightEdge.x + spawnDistanceFromCamera;

            float spawnY;
            if (groundTransform != null)
                spawnY = groundTransform.position.y + spikeHalfHeight; // base do spike alinhada com o ground
            else
                spawnY = transform.position.y;

            Debug.Log($"[SPAWNER] Instanciando Spike em X={spawnX:F2} Y={spawnY:F2}");
            Instantiate(spikePrefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity);
            ScheduleNext();
        }
    }

    void ScheduleNext()
    {
        nextSpawnIn = Random.Range(minSpawnTime, maxSpawnTime);
    }
}
