using UnityEngine;
using System.Collections;

public class CoinSprayer : MonoBehaviour
{

    // Use this for initialization
    public int numCoins = 10;
    public GameObject coinPrefab;
    public float offSetRange = 1.5f;
    void Start()
    {

        SpawnCoins();
    }

    // Update is called once per frame
    void SpawnCoins()
    {
        for (int i = 0; 1 < numCoins; i++)
        {
            Vector2 spawnOffSet = new Vector2(Random.Range(-offSetRange, offSetRange), Random.Range(-offSetRange, offSetRange));
            Instantiate(coinPrefab, (Vector2)transform.position + spawnOffSet, Quaternion.identity);
        }
    }
}
	

