using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public Counter counter;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Target")
        {
            Debug.Log("trigger entered");
            counter.addPoints(10);
        }
    }
}
