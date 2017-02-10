using UnityEngine;
using System.Collections;
public class BulletScript : MonoBehaviour
{
    //Drag in the Bullet Emitter from the Component Inspector.
    //public 
    [SerializeField]
    GameObject bulletEmitter;

    //Drag in the Bullet Prefab from the Component Inspector.
    // public 
    [SerializeField]
    GameObject bullet;


    //Enter the Speed of the Bullet from the Component Inspector.
    // public 
    [SerializeField]
    float bulletForwardForce;

    [SerializeField]
    float bulletCurve;
  
    // Use this for initialization
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //The Bullet instantiation happens here.
            GameObject temporaryBulletHandler;
            temporaryBulletHandler = Instantiate(bullet, bulletEmitter.transform.position, bulletEmitter.transform.rotation) as GameObject;

            //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
            //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
            temporaryBulletHandler.transform.Rotate(Vector3.left * 90);

            //Retrieve the Rigidbody component from the instantiated Bullet and control it.
            Rigidbody Temporary_RigidBody;
            Temporary_RigidBody = temporaryBulletHandler.GetComponent<Rigidbody>();

            //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
            Temporary_RigidBody.AddForce(transform.forward * bulletForwardForce);

            //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
            Destroy(temporaryBulletHandler, 3f);
        }
    }
   
}