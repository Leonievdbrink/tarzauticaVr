using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    private float timer = 0;
    public Counter counter;
    public GameObject orb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            timer = 0;
           GameObject temp = Instantiate(orb,this.transform.position,Quaternion.identity);
           temp.GetComponent<HitChecker>().counter = counter;

        }
    }
}
