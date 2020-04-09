using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class ActivateLookedAtObjects : MonoBehaviour
{

    //This script is intended to be put on the Camera 
    //This script is modified and currently on the ThirdPersonCharacter

    [SerializeField]
    GameObject raycastEnhancers;

    [SerializeField]
    float maxDistanceToActivateObjects = 30;

    [SerializeField]
    LayerMask layerActivatableObjectsAreOn;

    [SerializeField]
    Text lookedAtObjectName;
    GameObject lookedAtObjectNameGameObject;
    [SerializeField]
    Text userInputText;
    GameObject userInputTextGameObject;

    [SerializeField]
    LayerMask layerEnemiesObjectsAreOn;

    bool canActivate = true;
    float activateObjectTimer = 2;
    InventoryManager inventoryManager;

    // public Animator anim;
    //AudioSource audioSource; 
    // Use this for initialization	8
    IIdentifiable objectLookedAt;
   
    void Start()
    {
        lookedAtObjectNameGameObject = GameObject.Find("LookedAtObjectText");
        userInputTextGameObject = GameObject.Find("User Action Text");
        SetIdentifiables(lookedAtObjectNameGameObject, userInputTextGameObject);       
    }
    private void SetIdentifiables(GameObject displayName, GameObject userInput)
    {
        if(lookedAtObjectName == null)
        {
            lookedAtObjectName = displayName.GetComponent<Text>();
        }
        if(userInputText == null)
        {
            userInputText = userInputTextGameObject.GetComponent<Text>();
        }
        try
        {
            lookedAtObjectNameGameObject = GameObject.Find("lookedAtobjectText");
            userInputTextGameObject = GameObject.Find("User Action Text");
        }
        catch (System.Exception)
        {
            throw new System.Exception("Scene requires game object named:"
                + "lookedAtObject/UserInputText");
        }
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit,
           maxDistanceToActivateObjects, layerActivatableObjectsAreOn))
        {
            //if our ray hits something, we go into this code block
            objectLookedAt = raycastHit.collider.gameObject.GetComponent<IIdentifiable>();
            
            if (objectLookedAt == null)
            {
                throw new System.Exception(raycastHit.collider.gameObject.name +
                " MUST have a script that implements IActivatable script attached to it.");
            }
            if (gameObject.tag == "Enemy")
            {
                
                if (Input.GetButton("Fire1"))
                {
                    Debug.Log("Smack that hoe");
                }
            }
            // string objectName = raycastHit.collider.gameObject.name;
            //Debug.Log("Object Looked at " + objectName);
            if (Input.GetButton("Fire1"))
            {
                if (canActivate == true)
                {
                    if (gameObject.tag == "Items")
                    {
                        raycastHit.collider.gameObject.SetActive(false);
                        for (int i = 0; i < 1; i++)
                        {
                            //objectLookedAt.DoActivate();
                            //userInput.DoCommand();
                        }
                        canActivate = false;
                    }
                }
               // userInput.DoCommand();
            }
        }
        else
        {
            objectLookedAt = null;
           
        }
        //CanActivate();
        UpdateLookedAtObjectText();
        UpdateUserInputText();
        CheckedForLookedAtObjects();
    }
    private void CanActivate()
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
    private void UpdateLookedAtObjectText()
    {
        if (objectLookedAt != null)
        {
            lookedAtObjectName.gameObject.SetActive(true);
            lookedAtObjectName.text = objectLookedAt.DisplayName;
        }
        else if (objectLookedAt == null)
        {
            lookedAtObjectName.text = "";
            lookedAtObjectName.gameObject.GetComponent<Text>().enabled = true;
        }
    }
    private void UpdateUserInputText()
    {
        if (objectLookedAt != null)
        {
            userInputText.gameObject.SetActive(true);
            userInputText.text = objectLookedAt.DisplayInput;
        }
        else if (objectLookedAt == null)
        {
            userInputText.text = "";
            userInputText.gameObject.GetComponent<Text>().enabled = true;
        }
    }
}
