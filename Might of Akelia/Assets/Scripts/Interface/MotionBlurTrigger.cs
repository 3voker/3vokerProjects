using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MotionBlurTrigger : MonoBehaviour
{

    // Use this for initialization
    MotionBlur mBlur;
    float timerDrunk = 5.0f;
    void Start () {
        mBlur = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MotionBlur>();
        
        mBlur.enabled = false;
	}
    private void Update()
    {
        if (mBlur.enabled == true)
        {
            timerDrunk -= Time.deltaTime;
        }
        if (timerDrunk < 0)
        {
            timerDrunk = 5;
            mBlur.enabled = false;
        }      
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            mBlur.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
              
        }
    }

}
