using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public int count = 0;

    public TextMeshProUGUI text;

    private void Awake()
    {
        text = this.GetComponent<TextMeshProUGUI>();
    }

    // voeg 1 punt toe.
    public void addPoint()
    {
        count++;
    }

    // voeg aantal punten toe.
    public void addPoints(int number)
    {
        
        count += number;
    }

    // verwijder aantal punten.
    public void removePoints(int number)
    {
        if (number >= count)
        {
            count -= number;
        }
    }

    // het updaten van de tekst.
    private void Update()
    {
        text.text = count.ToString();
    }

    // resetten van de punten.
    public void ResetPunten()
    {
        count = 0;
    }
}