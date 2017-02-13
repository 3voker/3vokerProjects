using UnityEngine;
using System.Collections;
using System;

public class QuestTriggers : MonoBehaviour {

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject homePoint;

    [SerializeField]
    GameObject dropOffPoint;

    [SerializeField]
    GameObject pizzaSprite;

    [SerializeField]
    GameObject currentSprite;

    int newLocation;
    int current;
    bool hasPizzaOrder = false;

    RectTransform greenArrow;
    RectTransform redArrow;
    SpriteRenderer spriteRenderer;

    //[SerializeField]
    //GameObject key;
    //[SerializeField]
    //Camera cutSceneCamera;
    //[SerializeField]
    //private int numOfEnemies;
    //[SerializeField]
    //Animator anim;
    //AudioSource audioSource;
    //[SerializeField]
    //GameObject[] locations;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Vector3[] locations;

    Collider homePointCollider;

    Collider dropOffPointCollider;

    void Start()
    {       
        homePointCollider = homePoint.GetComponent<Collider>();
        dropOffPointCollider = dropOffPoint.GetComponent<Collider>();
        spriteRenderer = pizzaSprite.GetComponent<SpriteRenderer>();
        CheckMission(0);
    }
    void Update()
    {

        CheckMission(current);
   
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            foreach (Collider collider in this.gameObject.GetComponents<Collider>())
            {
                collider.isTrigger = true;
                if (collider is SphereCollider)
                {
                    SphereCollider homePointCollider = (SphereCollider)collider;
                    //Do some stuff with the box collider
                    hasPizzaOrder = true;
                    BossFeedBack();
                    Debug.Log("HomePoint Reached!");
                    NextMission();
                    homePoint.SetActive(false);
                }
                else if (collider is BoxCollider)
                {
                    BoxCollider dropOffPointCollider = (BoxCollider)collider;
                    //Do some stuff with the cyrcle collider                                      
                    hasPizzaOrder = false;
                    CustomerFeedBack();
                    Debug.Log("Drop off point Reached!");
                    homePoint.SetActive(true);
                    ReturnToBase();
                    Destroy(dropOffPoint);                       
                }
            }
     
        }       
    }

    private void BossFeedBack()
    {
        throw new NotImplementedException();
    }

    private void ReturnToBase()
    {
        //Activate red arrow when returning to base
        //Arrow should rotate in the direction of the homePoint
        //Find compass youtube video for help
        //  redArrow.TransformDirection
        //Reactivate homebase GameObject
         


    }

    private void CustomerFeedBack()
    {   //Happy, Ok, Angry feed back. Rating from customer gets to boss feedback. 
        
        throw new NotImplementedException();
    }

    private void NextMission()
    {
        if (hasPizzaOrder)
        {
            //Randomizes new location
            newLocation = UnityEngine.Random.Range(0, locations.Length);
            //Location selected   
            Debug.Log("New Location: " + newLocation);
            newLocation = current;
        }
    }
    private void CheckMission(int current)
    {
        if(newLocation == current && hasPizzaOrder)
        {
            CreateDropOff();
            EnrouteMission();                     
        }
     else if(newLocation == current && hasPizzaOrder == false)
        {
            EnrouteMission();
            ReturnToBase();
           // Debug.Log("Return To Base//CheckMission Method");
        }
    }

    private void CreateDropOff()
    {
        foreach (Vector3 location in locations)
        {
            if (newLocation != current && hasPizzaOrder)
            {
                //Activate green arrow when going to new Location
                //Arrow should rotate in the direction of the drop Off Point
                //Find compass youtube video for help
                
                GameObject currentDropOff;
currentDropOff = Instantiate(dropOffPoint, location, dropOffPoint.transform.rotation) as GameObject;
                break;
                currentSprite.transform.position
            }
            else if (newLocation != current && hasPizzaOrder == false)
            {
                ReturnToBase();
            }
        }       
    }

    private void EnrouteMission()
    {
        if (hasPizzaOrder)
        {
            spriteRenderer.enabled = true;
        //    Debug.Log("hasPizzaOrder //EnrouteMission");
            
        }
        else if(hasPizzaOrder == false)
        {
            spriteRenderer.enabled = false;
          //  Debug.Log("!hasPizza //EnrouteMission");
        }
    }
}
