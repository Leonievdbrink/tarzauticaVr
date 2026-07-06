using UnityEngine;

public class TimeTrialManager : MonoBehaviour
{
    [Header("Referenties")]
    public Countdown countdown;
    public DoorController door;

    // Deze koppel je aan de OnClick() van je button
    public void StartChallenge()
    {
        countdown.ResetTimer();
        countdown.StartTimer();
        door.Open();
    }
}