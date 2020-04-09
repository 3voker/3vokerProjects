using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class NPCInteraction : MonoBehaviour
{
    //Autumn additions
   // private PlayerController controller;
    public KeyCode kbInteractButton;
    public GamepadButtons gbInteractButton;

    [SerializeField]
    float maxDistanceToActivateObjects = 4;
    [SerializeField]
    LayerMask NPCInteractable;
    GameObject lookedAtActivatableObject;

    GameObject lookedAtCommandableObject;
   
    Text lookedAtActivatableObjectText;
    Text lookedAtCommandableObjectText;

    IIdentifiable IdentifiableNPC;
    IActivatable ActivatableNpC;
    IInteractable InteractableNPC; //Activates NPC

    GameObject lookedAtNPCObject;
    float activateObjectTimer;
    bool canActivate = true;

    void Awake()
    {
        //  controller = this.GetComponent<PlayerController>();   
}
    void Update()
    {
        RaycastHit raycastHit;       
       // lookedAtActivatableObject = lookedAtActivatableObjectText.gameObject;
    //    lookedAtCommandableObject = lookedAtCommandableObjectText.gameObject;

        if (Physics.Raycast(transform.position, transform.forward,
            out raycastHit, maxDistanceToActivateObjects, NPCInteractable))
        {
            //if our ray hits something, we go into this code block       
            ActivatableNpC = raycastHit.collider.gameObject.GetComponent<IActivatable>();
            if (ActivatableNpC == null)
            {
                throw new System.Exception(raycastHit.collider.gameObject.name +
                " MUST have a script that implements IActivatable script attached to it.");
            }
            // string objectName = raycastHit.collider.gameObject.name;
            //Debug.Log("Object Looked at " + objectName);
            if (Input.GetButton("Fire1"))
            {
                if (canActivate == true)
                {
                    if (gameObject.tag == "NPC")
                    {
                        //indicator showing you can talk to NPC                 
                        for (int i = 0; i < 1; i++)
                        {
                            ActivatableNpC.DoActivate();                        
                        }
                        canActivate = false;
                    }
                }             
            }
        }
        else
        {
            //indicator showing you can talk to NPC
            ActivatableNpC = null;
        }
            CanNPCActivate();       
            CheckedForLookedAtObjects();       
    }
    void HandleInput()
    {
        //if (Input.GetKeyDown(kbInteractButton) || (controller.gamepadInput && controller.myGamepad.GetButtonDown(gbInteractButton)))
        if (Input.GetButton("Fire1"))
        {
            if (ActivatableNpC != null)
            {
                ActivatableNpC.DoActivate();
            }
        }
    }
    private void CanNPCActivate()
    {
        if (canActivate == false)
        {
            activateObjectTimer -= Time.deltaTime;
            Debug.Log("Can Activate timer is " + activateObjectTimer + " .");
        }
        if (activateObjectTimer <= 0)
        {
            canActivate = true;
            activateObjectTimer = 2;
            Debug.Log("Can Activate items again.");
        }
    }
    private void CheckedForLookedAtObjects()
    {
        Vector3 endPoint = transform.position + maxDistanceToActivateObjects * transform.forward;
        Debug.DrawLine(transform.position, endPoint, Color.red);
    }  
}
