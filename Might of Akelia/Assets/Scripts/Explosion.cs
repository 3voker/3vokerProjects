using UnityEngine;
using System.Collections;
using System;

public class Explosion : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    GameObject explosionEmitter;

    //Drag in the Explosion Prefab from the Component Inspector.
    // public 
    [SerializeField]
    GameObject explosionPrefab;

    //Enter the Speed of the Explosion from the Component Inspector.
    // public 
    [SerializeField]
    float explosionExpansion;


    // Update is called once per frame

    void Explode()
    {
        {

            //The Explosion instantiation happens here.
            GameObject temporaryExplosionHandler;
            temporaryExplosionHandler = Instantiate(explosionPrefab, explosionEmitter.transform.position, explosionEmitter.transform.rotation) as GameObject;





            //Sometimes explosion may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
            //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
            temporaryExplosionHandler.transform.localScale += new Vector3(10F, 10, 10);

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = temporaryExplosionHandler.GetComponent<Rigidbody>();

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
            Temporary_RigidBody.AddForce(transform.forward * explosionExpansion);

            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(temporaryExplosionHandler, .25f);
        }
    }

}
    
