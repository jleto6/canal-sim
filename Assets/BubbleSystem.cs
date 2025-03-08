using UnityEngine;

public class BubbleSystem : MonoBehaviour
{
    public GameObject bubblePrefab;   // Prefab for the bubble object
    public Transform spawnPoint;     // Point where bubbles spawn
    public Transform endPoint;       // Point where bubbles should be destroyed
    public float spawnInterval = 1f; // Time between bubble spawns
    public float bubbleSpeed = 2f;   // Speed of bubble movement
    public Vector3 randomOffsetRange = new Vector3(0.1f, 0f, 0.1f); // Random offset for spawning

    private void Start()
    {
        // Start spawning bubbles at regular intervals
        InvokeRepeating("SpawnBubble", 0f, spawnInterval);
    }

    private void SpawnBubble()
    {
        // Create a new bubble at the spawn point with a slight random offset
        Vector3 spawnPosition = spawnPoint.position + new Vector3(
            Random.Range(-randomOffsetRange.x, randomOffsetRange.x),
            Random.Range(-randomOffsetRange.y, randomOffsetRange.y),
            Random.Range(-randomOffsetRange.z, randomOffsetRange.z)
        );

        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // Attach this script's movement function to the bubble
        bubble.AddComponent<BubbleMover>().Initialize(bubbleSpeed, endPoint);
    }
}

public class BubbleMover : MonoBehaviour
{
    private float speed;
    private Transform target;

    public void Initialize(float bubbleSpeed, Transform end)
    {
        speed = bubbleSpeed;
        target = end;
    }

    private void Update()
    {
        // Move the bubble toward the end point
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Destroy the bubble if it reaches or passes the endpoint
        if (target != null && transform.position.y >= target.position.y)
        {
            Destroy(gameObject);
        }
    }
}
