using UnityEngine;
using System.Collections;

public class destroyOther : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject explosionPrefab;

    [SerializeField]
    GameObject explosionEmitter;

    AudioSource audioSource;

    Animation animation;



    void Update()
    {
        audioSource = GetComponent<AudioSource>();
        animation = explosionPrefab.GetComponent<Animation>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
                audioSource.Play();
                GameObject temporaryExplosionHandler;
                temporaryExplosionHandler = Instantiate(explosionPrefab, other.transform.position, explosionEmitter.transform.rotation) as GameObject;
                temporaryExplosionHandler.transform.localScale += new Vector3(1, 1, 1);
                animation.Play("explosion");
                Rigidbody Temporary_RigidBody;
                Temporary_RigidBody = temporaryExplosionHandler.GetComponent<Rigidbody>();

            if (!animation.isPlaying)
            {
                Destroy(temporaryExplosionHandler, .75f);
                Destroy(other.gameObject, .60f);
            }  
        }
        //if (other.gameObject.tag == "Box")
        //{
        //    animator.SetBool("isHit", true);
        //    animation.Play("");
        //    Destroy(other.gameObject);
        //}
    }
}
