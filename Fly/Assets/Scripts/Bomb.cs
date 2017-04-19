using UnityEngine;
using System.Collections;
using System;

public class Bomb : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    public GameObject fire;
    [SerializeField]
    public int numFire = 10;
    [SerializeField]
    public float explodeOffSetRange = 1.5f;
   
    bool isDetonated = false;

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Bomb")
        {
            //Harm the player

            Bomb bomb = collision.gameObject.GetComponent<Bomb>();

            if (bomb != null)
            {

                //Instantiate(fire, transform.position, Quaternion.identity);
                bomb.Explode();

                Destroy(gameObject);

            }
        }
    }

    public void Explode()
    {
        //gameObject.GetComponentInChildren<FireSprayer>(PointEffector2D(enabled));
        for (int i = 0; i < numFire; i++)
        {
            Vector2 spawnOffSet = new Vector2(UnityEngine.Random.Range(-explodeOffSetRange, explodeOffSetRange), UnityEngine.Random.Range(-explodeOffSetRange, explodeOffSetRange));
            Instantiate(fire, (Vector2)transform.position + spawnOffSet, Quaternion.identity);
        }
        float explodeTime = 5;
        explodeTime -= Time.deltaTime;
        while (explodeTime > 0)
        {
            explodeTime--;
            if (explodeTime <= 0)
            {
                gameObject.SetActive(false);
                isDetonated = true;
            }
        }
    }
}
