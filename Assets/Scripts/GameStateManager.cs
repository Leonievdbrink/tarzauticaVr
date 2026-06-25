using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton that survives scene loads and manages transitions between the menu and game scenes.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    private const string MenuSceneName = "MainMenu";
    private const string GameSceneName = "TessaScene";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>Loads the main game scene.</summary>
    public void StartGame()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    /// <summary>Returns to the main menu scene.</summary>
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
}
