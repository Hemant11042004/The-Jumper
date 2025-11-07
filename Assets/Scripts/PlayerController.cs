using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    private GameManager gameManager;

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpForce = 10;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    private int obstaclesCleared = 0;
    private bool wasInAir = false;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = FindFirstObjectByType<GameManager>();

        Physics.gravity *= gravityModifier;
    }

    void Update()
    {
        if (gameManager == null || !gameManager.isGameActive || gameOver) return;

        // Jumping logic
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            wasInAir = true; // Player is jumping
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
    }
    
    public void ObstacleCleared()
{
    if (wasInAir && !gameOver)
    {
        obstaclesCleared++;
    }
}



    private void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isOnGround = true;

        // Only count score if the player was in the air (meaning they jumped)
        if (wasInAir && !gameOver)
        {
            // Reward based on how many obstacles cleared during that jump
            if (obstaclesCleared > 0)
            {
                int points = obstaclesCleared == 1 ? 5 : 10;
                gameManager.UpdateScore(points);
                Debug.Log($"Landed safely — cleared {obstaclesCleared} obstacle(s), +{points} points!");
            }

            // Reset after landing
            obstaclesCleared = 0;
            wasInAir = false;
            dirtParticle.Play();
        }
    }
    else if (collision.gameObject.CompareTag("Obstacle"))
    {
        Debug.Log("Game Over!");
        gameOver = true;
        playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 1);
        explosionParticle.Play();
        dirtParticle.Stop();
        playerAudio.PlayOneShot(crashSound, 1.0f);

        if (gameManager != null)
        {
            gameManager.GameOver();
        }
    }
}

}
