using UnityEngine;
using System.Collections;

public class DestroyedWall : MonoBehaviour {

    // Use this for initialization.

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
}
