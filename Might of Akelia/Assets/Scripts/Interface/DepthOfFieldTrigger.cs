using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class DepthOfFieldTrigger : MonoBehaviour
{

    // Use this for initialization
    DepthOfField dField;
    float timerDrunk = 5.0f;
    void Start()
    {
        dField = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DepthOfField>();

        dField.enabled = false;
    }
    private void Update()
    {
        if (dField.enabled == true)
        {
            timerDrunk -= Time.deltaTime;
        }
        if (timerDrunk < 0)
        {
            timerDrunk = 5;
            dField.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dField.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }

}
