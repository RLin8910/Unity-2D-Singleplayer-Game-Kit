using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Class to handle game state. </summary>
public class GameManager : MonoBehaviour
{
    // static variables
    /// <summary> The one instance of the GameManager in the scene. </summary>
    /// <remarks> Uses a Singleton structure so other scripts can use GameManager.instance to
    /// access the instance. </remarks>
    public static GameManager instance { get; private set; }
    // data types
    /// <summary> Data type for storing game state. </summary>
    private enum GameState
    {
        Playing,
        Ended
    }
    // Inspector-editable variables
    /// <summary> Game timer. When this timer ends, the game ends. </summary>
    [SerializeField]
    private Timer timer;
    [Header("UI Elements")]
    /// <summary> Text to display the winner's name. </summary>
    [SerializeField]
    private Text winnerText;
    /// <summary> CanvasMenus to use when showing win screen. </summary>
    [SerializeField]
    private CanvasMenus canvasMenus;
    /// <summary> Name of win screen gameObject in canvas menu. </summary>
    [SerializeField]
    private string winScreen;
    // private variables
    /// <summary> The current game state. </summary>
    private GameState gameState = GameState.Ended;
    void Start()
    {
        instance = this;
        timer.AddFinishAction(OnTimerEnded);
    }
    void OnDestroy()
    {
        timer.RemoveFinishAction(OnTimerEnded);
    }
    /// <summary> Function called when the timer ends. </summary>
    void OnTimerEnded()
    {
        EndGame(false);
    }
    /// <summary> End the game. </summary>
    /// <param name="winner"> Whether the player won. </param>
    public void EndGame(bool winner)
    {
        if(gameState == GameState.Ended) return;

        winnerText.text = winner ? "You Win!" : "You Lose!";
        timer.Pause();
        canvasMenus.OpenMenu(winScreen);
        // disable player movement
        Player.CanMove = false;
    }
    /// <summary> Start/restart the game. </summary>
    public void StartGame()
    {
        gameState = GameState.Playing;
        timer.ResetTime();
        Player.ResetAllPositions();
        Player.CanMove = true;
    }
}
