using UnityEngine;
using System.Collections;
using System;

public class QuestTriggers : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject homePoint;
    Collider homePointCollider;
    [SerializeField]
    GameObject dropOffPoint;
    Collider dropOffPointCollider;
    [SerializeField]
    GameObject pizzaSprite;
    [SerializeField]
    GameObject currentSprite;
    int speed = 5;
    int newLocation;
    int current;
    bool hasPizzaOrder = false;
    RectTransform greenArrow;
    RectTransform redArrow;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
   Transform[] locations; 
     
    void Start()
    {             
        spriteRenderer = pizzaSprite.GetComponent<SpriteRenderer>();
        homePointCollider = homePoint.GetComponent<Collider>();
        dropOffPointCollider = dropOffPoint.GetComponent<Collider>();
    }
    void Update()
    {
        CheckMission(current);
        CheckCollision(); 
    }

    private void CheckCollision()
    {
       if(dropOffPointCollider)
        {

        }
    }

    public bool Pizza
    {
        get
        {
            return hasPizzaOrder;
        }            
        set
        {
           hasPizzaOrder = value;
        }
    }


    public void CheckMission(int current)
    {
        if (newLocation == current && hasPizzaOrder)
        {
            CreateDropOff();
            EnrouteMission();
        }
        else if (newLocation == current && hasPizzaOrder == false)
        {
            EnrouteMission();          
            // Debug.Log("Return To Base//CheckMission Method");
        }
    }   
    private void CreateDropOff()
    {
        foreach (Transform location in locations)
        {
            if (newLocation != current && hasPizzaOrder)
            {
                //Activate green arrow when going to new Location
                //Arrow should rotate in the direction of the drop Off Point
                //Find compass youtube video for help
                GameObject currentDropOff;
                currentDropOff = Instantiate(dropOffPoint, location.position, dropOffPoint.transform.rotation) as GameObject;
                currentSprite.transform.position = Vector3.MoveTowards(transform.position, dropOffPoint.transform.position, speed * Time.deltaTime);
                break;
            }
            else if (newLocation != current && hasPizzaOrder == false)
            {
               // ReturnToBase();
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
        else if (hasPizzaOrder == false)
        {
            spriteRenderer.enabled = false;
            //  Debug.Log("!hasPizza //EnrouteMission");
        }
    }
    private void NextMission()
    {
        if (hasPizzaOrder)
        {
            Debug.Log("Has mission!");
            //Randomizes new location
            newLocation = UnityEngine.Random.Range(0, locations.Length);
            //Location selected   
            Debug.Log("New Location: " + newLocation);
            newLocation = current;
        }
    }
}

  

