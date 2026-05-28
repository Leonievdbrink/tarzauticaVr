using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab; // The prefab of the item to spawn

    public bool instantSpawn = false;

    public float timeToSpawn = 2f;

    public bool autoSpawn = true;


    public void SpawnItem()
    {
       GameObject gameObj = Instantiate(itemPrefab, this.transform.position, Quaternion.identity);
        //gameObj.transform.position = Vector3.zero;
        Rigidbody rb = gameObj.GetComponent<Rigidbody>();
        //rb.AddForce(rb.transform.up * 5f, ForceMode.Impulse);
        gameObj.transform.SetParent(null);
    }

    private void Start()
    {
        if (!autoSpawn) return;
        if (instantSpawn)
        {
            SpawnItem();
        }else
        {
        StartCoroutine(Spawn());
        }
    }


    IEnumerator Spawn()
    {
        yield return new WaitForSecondsRealtime(timeToSpawn);
        SpawnItem();
    }
}
