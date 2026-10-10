using System;
using TMPro;
using UnityEngine;

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

    private void Start()
    {
        currentState = GameState.MainMenu;
        Time.timeScale = 0f;
    }

    private void Update()
    {
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

    private void GameOver()
    {
        // Fade in the main menu canvas and show the game over message
        mainMenuCanvas.SetActive(true);
        currentState = GameState.GameOver;
    }

    // Called on start button click
    public void StartGame()
    {
        // Logic to start the game
        mainMenuCanvas.SetActive(false);
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        gameStateText.text = "Pause";
        startButtonText.text = "Reprendre";
    }

    // Called on restart button click
    public void RestartGame()
    {
        StartGame();
        // Additional logic to reset the game state if needed
        // reset player pos
        // reset enemies
        // reset chad score
    }
}
