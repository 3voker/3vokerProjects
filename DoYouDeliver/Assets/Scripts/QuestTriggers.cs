using UnityEngine;
using System.Collections;
using System;

public class QuestTriggers : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject homePoint;

    [SerializeField]
    GameObject pizzaSprite;
    Collider homePointCollider;
    [SerializeField]
    GameObject dropOffPoint;
    Collider dropOffPointCollider;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    GameObject currentSprite;
    int speed = 5;
    int orders = 0;
    bool hasPizzaOrder = false;
    RectTransform greenArrow;
    RectTransform redArrow;
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
   Transform[] locations;
    [SerializeField]
    GameObject cellphonePanel;

   
     
    void Start()
    {
        spriteRenderer = pizzaSprite.GetComponent<SpriteRenderer>();
        homePointCollider = homePoint.GetComponent<Collider>();
        dropOffPointCollider = dropOffPoint.GetComponent<Collider>();
        spriteRenderer.enabled = false;
        CheckMission();
    }
    private void CheckMission()
    {
        if (spriteRenderer.enabled)
        {          
            EnrouteMission();
        }
           
    }   
    private void CreateDropOff()
    {
        foreach (Transform location in locations)
        {
          
        }
    }
    private void EnrouteMission()
    {
        if (spriteRenderer.enabled)
        {
            spriteRenderer.enabled = true;
            hasPizzaOrder = true;
            orders++;
            //    Debug.Log("hasPizzaOrder //EnrouteMission");
        }
        else if (!spriteRenderer.enabled)
        {
            spriteRenderer.enabled = false;
            hasPizzaOrder = false;
            //  Debug.Log("!hasPizza //EnrouteMission");
        }
    }
   
}

  

