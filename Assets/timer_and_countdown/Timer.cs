using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    [Header("Start opties")]
    [Tooltip("Of de timer vanaf het start van het spel automatisch start")]
    [SerializeField]
    private bool runOnStart = true;

    private bool isRunning = false;

    [Header("Timer opties")]
    [Tooltip("Of er alleen hele nummers getoond moeten worden in de timer")]
    public bool wholeNumbersOnly = true;
    private float timer = 0;
    [Header("Text opties")]
    [Tooltip("Hier moet je het tekst gameobject in slepen.")]
    public TextMeshProUGUI text;


    private void Start()
    {
        if (!runOnStart)
            return;

        isRunning = true;
        timer = 0;
    }

    private void Update()
    {
        if (!isRunning)
            return;

        timer += Time.deltaTime;
        if(text != null)
            text.text = wholeNumbersOnly ? timer.ToString("0") : timer.ToString("F2");
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        timer = 0;
    }
}
