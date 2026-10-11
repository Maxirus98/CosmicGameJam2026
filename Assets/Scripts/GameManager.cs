using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Represents the current state of the game.
    /// </summary>
    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
    }

    /// <summary>
    /// Event that is triggered when the game state changes. Subscribers can listen to this event to respond to state changes.
    /// </summary>
    public static event Action<GameState> GameStateHandler;

    [SerializeField]
    [Tooltip("The canvas for the main menu")]
    private GameObject mainMenuCanvas;

    [SerializeField]
    [Tooltip("The text component of the start button")]
    private TextMeshProUGUI startButtonText;
    [SerializeField]
    [Tooltip("Text to show if paused")]
    private TextMeshProUGUI gameStateText;
    private GameState currentState;

    [SerializeField]
    [Tooltip("The canvas for the story")]
    private GameObject storyCanvas;

    [SerializeField]
    [Tooltip("The canvas for the end")]
    private GameObject endCanvas;


    private void Start()
    {
        currentState = GameState.MainMenu;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (currentState == GameState.GameOver) return;
        if (currentState != GameState.MainMenu && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

    }

    // Toggle between playing and paused states
    private void TogglePause()
    {

        if (currentState == GameState.Playing)
        {
            currentState = GameState.Paused;
            mainMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (currentState == GameState.Paused)
        {
            currentState = GameState.Playing;
            mainMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
        }

        // Notify subscribers about the state change
        GameStateHandler?.Invoke(currentState);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        // Fade in the main menu canvas and show the game over message
        endCanvas.SetActive(true);
        mainMenuCanvas.SetActive(true);
        currentState = GameState.GameOver;
        startButtonText.text = "Restartmaxxing";

        // Weird way to get the button. Don't judge me.
        startButtonText.transform.parent.GetComponent<Button>().onClick.AddListener(RestartScene);
    }

    public void RestartScene()
    {
        // 2. Get the index of the currently active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 3. Load it again
        SceneManager.LoadScene(currentSceneIndex);
    }

    // Called on start button click
    public void StartGame()
    {
        // Logic to start the game
        mainMenuCanvas.SetActive(false);
        currentState = GameState.Playing;
        startButtonText.text = "Resumemaxxing";

        storyCanvas.SetActive(true);
    }


    public void Quitter()
    {
        Application.Quit();
    }
}
