using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class DrunkEffect : MonoBehaviour
{

    // Use this for initialization
    MotionBlur mBlur;
    float timerDrunk = 5.0f;

    void Start()
    {
        mBlur = GameObject.Find("Main Camera").GetComponent<MotionBlur>();
        mBlur.enabled = false;
    }
    private void Update()
    {
        if (Input.GetButton("Jump")) 
        {
            mBlur.enabled = true;
        }
        if (mBlur.enabled == true)
        {
            timerDrunk -= Time.deltaTime;
        }
        if (timerDrunk <= 0)
        {
            timerDrunk = 5;
            mBlur.enabled = false;
        }
    }


}
