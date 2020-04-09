using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class FishEyeTrigger : MonoBehaviour
{

    // Use this for initialization
    Fisheye fEye;
    float timerfEye = 5.0f;

    void Start()
    {
        fEye = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Fisheye>();
        fEye.enabled = false;

    }
    void Update()
    {
        if (fEye.enabled == true)
        {
            timerfEye -= Time.deltaTime;
        }
        if (timerfEye < 0)
        {
            timerfEye = 5;
            fEye.enabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            fEye.enabled = true;
        }
    }


}




