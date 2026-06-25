using UnityEngine;

/// <summary>
/// Controls the main menu scene. Attach to any GameObject in the MainMenu scene.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        // Ensure the GameStateManager singleton exists when starting from the menu scene.
        if (GameStateManager.Instance == null)
        {
            GameObject managerObject = new GameObject("GameStateManager");
            managerObject.AddComponent<GameStateManager>();
        }
    }

    /// <summary>Called by the Start Game button. Loads the game scene.</summary>
    public void StartGame()
    {
        GameStateManager.Instance.StartGame();
    }

    /// <summary>Called by the Quit button. Exits the application.</summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
