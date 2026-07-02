using UnityEngine;

/// <summary>
/// Attach to the XR Origin or Player GameObject.
/// Call Die() from any game system (trigger zones, hazards, etc.) to return to the main menu.
/// </summary>
public class PlayerDeathHandler : MonoBehaviour
{
    [Header("Instellingen")]
    [Tooltip("Vertraging in seconden voordat het menu geladen wordt na de dood.")]
    [SerializeField]
    private float delayBeforeMenu = 2f;

    private bool isDead = false;

    /// <summary>Triggers player death and returns to the main menu after a delay.</summary>
    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Invoke(nameof(LoadMenu), delayBeforeMenu);
    }

    private void LoadMenu()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.ReturnToMenu();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
    }
}
