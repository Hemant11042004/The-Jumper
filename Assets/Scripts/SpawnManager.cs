using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;
    private Vector3 spawnPos = new Vector3(25, 0, 0);
    private float startDelay = 2f;
    private float repeatRate = 2f;
    private PlayerController playerControllerScript;
    private GameManager gameManager;

    [Range(0f, 1f)]
    public float doubleSpawnChance = 0.2f;
    public float doubleSpawnOffset = 2f;

    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = FindFirstObjectByType<GameManager>();

        // Wait until game starts before spawning
        StartCoroutine(WaitForGameStart());
    }

    private System.Collections.IEnumerator WaitForGameStart()
    {
        while (gameManager == null || !gameManager.isGameActive)
        {
            yield return null;
        }

        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);
    }

    void SpawnObstacle()
    {
        if (gameManager == null || !gameManager.isGameActive || playerControllerScript.gameOver) return;

        float randomValue = Random.value;

        if (randomValue < doubleSpawnChance)
        {
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
            Vector3 secondSpawnPos = spawnPos + new Vector3(doubleSpawnOffset, 0, 0);
            Instantiate(obstaclePrefab, secondSpawnPos, obstaclePrefab.transform.rotation);
        }
        else
        {
            Instantiate(obstaclePrefab, spawnPos, obstaclePrefab.transform.rotation);
        }
    }
}
