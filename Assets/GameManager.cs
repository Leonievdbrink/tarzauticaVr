using System;
using System.Collections;
using UnityEngine;

public enum GameState
{
    Menu,
    Playing,
    Ended
}

/// <summary>
/// Centrale state machine voor Tarzautica.
/// Regelt de loop: Menu -> Spelen -> Einde (dood/ontsnapt/tijd op) -> terug naar Menu.
/// Zet deze op een leeg GameObject in je scene, bv "GameManager".
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Spawn Points")]
    [Tooltip("Lege Transform waar de speler staat tijdens het menu")]
    public Transform menuSpawnPoint;
    [Tooltip("Lege Transform waar de speler spawnt in de centrale ruimte bij start")]
    public Transform gameSpawnPoint;

    [Header("Scene Objects")]
    [Tooltip("Het GameObject (of meerdere onder 1 parent) met je menu: UI, startknop, etc.")]
    public GameObject menuRoot;
    [Tooltip("Het GameObject met je hele speelbare level: centrale ruimte + zijruimtes")]
    public GameObject gameRoot;

    [Header("XR Rig")]
    [Tooltip("De XR Origin (OVR Camera Rig / XR Origin) van de speler, NIET de camera zelf")]
    public Transform xrOrigin;

    [Header("Timer (optioneel)")]
    [Tooltip("Aanzetten als er een maximale speeltijd is waarbij de speler verliest")]
    public bool useTimeLimit = false;
    public float timeLimitSeconds = 300f;
    [Tooltip("Hoelang het eindscherm/score blijft staan voordat je teruggaat naar het menu")]
    public float returnToMenuDelay = 4f;

    public GameState CurrentState { get; private set; } = GameState.Menu;
    public float ElapsedTime { get; private set; }

    // Event hooks zodat UI/score-scherm/audio zich kunnen aanmelden zonder dat GameManager ze hoeft te kennen
    public event Action<GameState> OnStateChanged;
    public event Action<float> OnGameLost;   // dood of tijd op -> geeft speeltijd door
    public event Action<float> OnGameWon;    // ontsnapt -> geeft eindtijd (=score) door

    private Coroutine timerRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GoToMenu();
    }

    // ---------- MENU ----------
    public void GoToMenu()
    {
        StopTimer();
        CurrentState = GameState.Menu;

        if (menuRoot != null) menuRoot.SetActive(true);
        if (gameRoot != null) gameRoot.SetActive(false);

        TeleportPlayer(menuSpawnPoint);
        ResetLevelState();

        OnStateChanged?.Invoke(CurrentState);
    }

    // Roep dit aan vanuit je startknop (zie StartButton.cs)
    public void StartGame()
    {
        CurrentState = GameState.Playing;

        if (menuRoot != null) menuRoot.SetActive(false);
        if (gameRoot != null) gameRoot.SetActive(true);

        TeleportPlayer(gameSpawnPoint);

        ElapsedTime = 0f;
        timerRoutine = StartCoroutine(RunTimer());

        OnStateChanged?.Invoke(CurrentState);
    }

    // Roep dit aan als de speler doodgaat (val, hazard, etc.)
    public void PlayerDied()
    {
        if (CurrentState != GameState.Playing) return;
        EndGame(won: false);
    }

    /// <summary>Sluit de applicatie af vanuit het menu.</summary>
    public void QuitApplication()
    {
        Application.Quit();
    }

    // Roep dit aan vanuit de hoofduitgang als de speler ontsnapt (zie EscapeDoor.cs)
    public void PlayerEscaped()
    {
        if (CurrentState != GameState.Playing) return;
        EndGame(won: true);
    }

    private void EndGame(bool won)
    {
        CurrentState = GameState.Ended;
        StopTimer();

        if (won) OnGameWon?.Invoke(ElapsedTime);
        else OnGameLost?.Invoke(ElapsedTime);

        OnStateChanged?.Invoke(CurrentState);

        StartCoroutine(ReturnToMenuAfterDelay(returnToMenuDelay));
    }

    private IEnumerator ReturnToMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GoToMenu();
    }

    private IEnumerator RunTimer()
    {
        while (true)
        {
            ElapsedTime += Time.deltaTime;

            if (useTimeLimit && ElapsedTime >= timeLimitSeconds)
            {
                EndGame(won: false);
                yield break;
            }

            yield return null;
        }
    }

    private void StopTimer()
    {
        if (timerRoutine != null)
        {
            StopCoroutine(timerRoutine);
            timerRoutine = null;
        }
    }

    private void TeleportPlayer(Transform target)
    {
        if (xrOrigin == null || target == null) return;

        // CharacterController moet even uit, anders negeert Unity de positie-aanpassing.
        var cc = xrOrigin.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        xrOrigin.position = target.position;
        xrOrigin.rotation = target.rotation;

        if (cc != null) cc.enabled = true;
    }

    private void ResetLevelState()
    {
        // Reset alle puzzels/deuren/sleutels die IResettable implementeren.
        // Zo hoeft GameManager niet elk los systeem te kennen.
        if (gameRoot == null) return;

        var resettables = gameRoot.GetComponentsInChildren<IResettable>(true);
        foreach (var r in resettables)
            r.ResetState();
    }
}

/// <summary>
/// Implementeer dit op elk script dat moet terug-resetten als de loop opnieuw begint
/// (bv. een deur die weer dicht moet, een kist die weer gesloten moet, een sleutel die terug op zijn plek moet).
/// </summary>
public interface IResettable
{
    void ResetState();
}