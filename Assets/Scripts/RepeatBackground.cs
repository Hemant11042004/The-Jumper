using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;
    private GameManager gameManager;

    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x / 2;
        gameManager = FindFirstObjectByType<GameManager>(); 
    }

    void Update()
    {
        // Only move the background if the game is active
        if (gameManager != null && gameManager.isGameActive)
        {
            if (transform.position.x < startPos.x - repeatWidth)
            {
                transform.position = startPos;
            }
        }
    }
}
