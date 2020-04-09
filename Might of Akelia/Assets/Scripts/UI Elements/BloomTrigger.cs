using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class BloomTrigger : MonoBehaviour
{

    // Use this for initialization
    Bloom mBloom;
    float timerDrunk = 5.0f;
    void Start()
    {
        mBloom = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Bloom>();

        mBloom.enabled = false;
    }
    private void Update()
    {
        if (mBloom.enabled == true)
        {
            timerDrunk -= Time.deltaTime;
        }
        if (timerDrunk < 0)
        {
            timerDrunk = 5;
            mBloom.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mBloom.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }

}
