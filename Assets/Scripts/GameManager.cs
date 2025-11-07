using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverText;
    public Button restartButton;
    public Button playButton;
    public GameObject titleScreen;

    [Header("Pause Settings")]
    public GameObject pausePanel;
    private bool isPaused = false;

    [Header("Audio")]
    private AudioSource bgMusic; // Reference to Main Camera AudioSource

    private int score;
    public bool isGameActive;

    void Start()
    {
        // Get the AudioSource from the Main Camera
        bgMusic = Camera.main.GetComponent<AudioSource>();
        if (bgMusic != null)
        {
            bgMusic.Stop(); // Stop any music that may play on scene load
        }

        // Game should start at the title screen only
        isGameActive = false;
        Time.timeScale = 1f; // Reset time in case game was paused before

        // Initial UI setup
        titleScreen.SetActive(true);
        playButton.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGameActive)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void StartGame()
    {
        // Called when the Play button is pressed in-game
        isGameActive = true;
        score = 0;

        // Update UI
        titleScreen.SetActive(false);
        playButton.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
        gameOverText.SetActive(false);
        pausePanel.SetActive(false);

        UpdateScore(0);
        Debug.Log("Game Started");

        // Play background music
        if (bgMusic != null && !bgMusic.isPlaying)
        {
            bgMusic.Play();
        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (!isGameActive) return;

        isGameActive = false;
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame()
    {
    if (!isGameActive) return;

    isPaused = true;
    Time.timeScale = 0f;

    // Show the pause panel
    pausePanel.SetActive(true);

    // Ensure the Exit/Quit button (child of pause panel) is also visible
    Transform exitButton = pausePanel.transform.Find("ExitButton"); // Replace with the exact name of your button
    if (exitButton != null)
    {
        exitButton.gameObject.SetActive(true);
    }

    Debug.Log("Game Paused");
    }

    public void ResumeGame()
    {
    isPaused = false;
    Time.timeScale = 1f;

    // Hide the pause panel
    pausePanel.SetActive(false);

    // Hide Exit/Quit button as well
    Transform exitButton = pausePanel.transform.Find("ExitButton");
    if (exitButton != null)
    {
        exitButton.gameObject.SetActive(false);
    }

    Debug.Log("Game Resumed");
    }


    public void QuitToTitle()
 {
    // Reset time (important in case paused)
    Time.timeScale = 1f;
    isGameActive = false;
    isPaused = false;

    // Hide in-game UI
    pausePanel.SetActive(false);
    restartButton.gameObject.SetActive(false);
    gameOverText.gameObject.SetActive(false);
    scoreText.gameObject.SetActive(false);

    // Show title screen and Play button again
    titleScreen.SetActive(true);
    playButton.gameObject.SetActive(true);

    // Stop background music (optional)
    if (bgMusic != null && bgMusic.isPlaying)
    {
        bgMusic.Stop();
    }

    // Destroy all active obstacles
    GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
    foreach (GameObject obstacle in obstacles)
    {
        Destroy(obstacle);
    }

    Debug.Log("Returned to Title Screen — obstacles cleared, title and play button visible.");
 }

}
