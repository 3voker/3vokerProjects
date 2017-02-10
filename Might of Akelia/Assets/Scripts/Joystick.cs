using UnityEngine;
using System.Collections;
using System;

public class Joystick : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    bool isButton;
    [SerializeField]
    bool leftJoystick;

    [SerializeField]
    string buttonName;

    [SerializeField]
    bool leftTrigger;

    [SerializeField]
    bool rightTrigger;


    [SerializeField]
    bool isDpad;

    Vector3 startPos;
    Transform thisTransform;
    MeshRenderer meshRenderer;

    //Be sure to implement multi-dimensional input system for button input pressed, tapped, held. 
    //Directional inputs should be excluded...slightly. Double dash inputs blue flames can be applied to rail system. 
    //Input[,] inputs = new Input[14, 3] {{ } };
    //numbers = new int[3, 2] {{9, 99}, {3, 33}, {5, 55}};

    void Start () {
        thisTransform = transform;
        startPos = thisTransform.position;
        meshRenderer = this.transform.GetComponent<MeshRenderer>();     
	}
	
	// Update is called once per frame
	void Update () {

        playerMovement();
       
	}

    private void playerMovement()
    {
        if (isButton)
        {
            meshRenderer.enabled = Input.GetButton(buttonName);
        }
        else if (leftTrigger)
        {
            //Triggers operate differently than GetButton, require numerical float value 
            if ((Input.GetAxis("leftTrigger") > 0))
            {
                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }
        else if (rightTrigger)
        {
            //Triggers operate differently than GetButton, require numerical float value 
            if ((Input.GetAxis("rightTrigger") > 0))
            {

                meshRenderer.enabled = true;
            }
            else
            {
                meshRenderer.enabled = false;
            }
        }

        else if (isDpad)
        {
            //meshRenderer.enabled = Input.GetButton(buttonName);
            Vector3 inputDirection = Vector3.zero;
            inputDirection.x = Input.GetAxis("dPadHorizontal");
            inputDirection.y = Input.GetAxis("dPadVertical");
            this.transform.position = startPos - inputDirection;
        }
        else
        {
            if (leftJoystick)
            {
                Vector3 inputDirection = Vector3.zero;
                inputDirection.x = Input.GetAxis("leftJoystickHorizontal");
                inputDirection.y = Input.GetAxis("leftJoystickVertical");
                this.transform.position = startPos - inputDirection;
            }
            else if (!leftJoystick)
            {
                Vector3 inputDirection = Vector3.zero;
                inputDirection.x = Input.GetAxis("rightJoystickHorizontal");
                inputDirection.y = Input.GetAxis("rightJoystickVertical");
                this.transform.position = startPos - inputDirection;
            }
        }
    }
}
