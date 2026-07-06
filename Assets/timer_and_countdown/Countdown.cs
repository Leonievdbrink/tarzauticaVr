using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Countdown : MonoBehaviour
{
    private float timer = 0;

    [Header("Timer opties")]
    [Tooltip("Hoe lang de timer moet lopen voordat hij klaar is")]
    public float countdownTime = 30f;
    [Tooltip("Of er alleen hele nummers getoond moeten worden in de timer")]
    public bool wholeNumbersOnly = false;

    private bool isRunning = false;

    [Header("Start opties")]
    [Tooltip("Of de timer vanaf het start van het spel automatisch start")]
    [SerializeField]
    private bool runOnStart = true;

    [Header("Herlaad opties")]
    [Tooltip("Of de scene herladen moet worden als de timer klaar is")]
    public bool reloadSceneOnEnd = true;

    [Tooltip("Hoe lang er gewacht moet worden voordat de scene herladen wordt")]
    public float waitTimeBeforeReload = 5f;

    [Header("Scene opties")]
    [Tooltip("De naam van de scene die geladen wordt als de timer klaar is. Leeg = huidige scene herladen.")]
    public string sceneToLoadOnEnd = "MainMenu";

    [Header("Text opties")]
    [Tooltip("Hier moet je het tekst gameobject in slepen.")]
    public TextMeshProUGUI text;

    [Header("Events")]
    [Tooltip("Wordt aangeroepen zodra de timer op 0 staat")]
    public UnityEvent onTimerEnd;


    private void Start()
    {
        if (!runOnStart)
            return;

        isRunning = true;
        timer = countdownTime;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        timer -= Time.deltaTime;
        if (text != null)
            text.text = wholeNumbersOnly ? timer.ToString("0") : timer.ToString("F2") ;

        if (timer <= 0)
        {
            timer = 0;
            isRunning = false;
            onTimerEnd?.Invoke();
            if (reloadSceneOnEnd)
                OnTimerEnded();
        }
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    // pauzeren
    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timer = countdownTime;
    }

    private void OnTimerEnded()
    {
        StartCoroutine(WaitAndLoadScene());
    }

    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSecondsRealtime(waitTimeBeforeReload);

        if (!string.IsNullOrEmpty(sceneToLoadOnEnd))
        {
            SceneManager.LoadScene(sceneToLoadOnEnd);
        }
        else
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
