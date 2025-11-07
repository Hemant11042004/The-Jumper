using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 20f;
    private PlayerController playerControllerScript;
    private GameManager gameManager;
    private float leftBound = -15f;
    private bool hasPassedPlayer = false; // prevents double-counting

    void Start()
    {
        // Find PlayerController and GameManager
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        // Only move if the game is active and not over
        if (gameManager != null && gameManager.isGameActive && !playerControllerScript.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);

            // Detect if obstacle just passed the player
            if (!hasPassedPlayer && transform.position.x < playerControllerScript.transform.position.x)
            {
                hasPassedPlayer = true; // make sure it only counts once
                playerControllerScript.ObstacleCleared();
                Debug.Log("Obstacle cleared!");
            }

            // Destroy off-screen obstacles
            if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
            {
                Destroy(gameObject);
            }
        }
    }

    
}
