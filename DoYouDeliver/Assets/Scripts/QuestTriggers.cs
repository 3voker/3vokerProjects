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

    int newLocation;
    int current;
    bool hasPizzaOrder = false;

    RectTransform greenArrow;
    RectTransform redArrow;


    //[SerializeField]
    //GameObject key;

    [SerializeField]
    Camera mainCamera;

    //[SerializeField]
    //GameObject[] locations;
    [SerializeField]
    Vector3[] locations;

    //[SerializeField]
    //Camera cutSceneCamera;

    //[SerializeField]
    //private int numOfEnemies;
    //[SerializeField]
    //Animator anim;

    //AudioSource audioSource;



    void Start()
    {
        //anim = GetComponent<Animation>();
        //key.SetActive(true);
        //mainCamera.enabled = true;
        //cutSceneCamera.enabled = false;
        
        NextMission();
    }
    void Update()
    {

        CheckMission(current);
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Done playing"))
        //{
        //    key.SetActive(false);
        //    mainCamera.enabled = true;
        //    Destroy(cutSceneCamera);
        //    Destroy(gameObject);
           
        //   // key.SetActive(false);
        //}
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            //If player touches Homepoint. 
            //Gets new pizza 
            //Is assigned next Mission
            //Homepoint closes(Deactivates)
            if(this.gameObject == homePoint)
            {          
                hasPizzaOrder = true;
                BossFeedBack();
                NextMission();
                homePoint.SetActive(false);
            }
            //Else If player touches Random Drop Off Point
            //Loses Pizza 
            //Customer compliments or complains
            //Homepoint reopens doors
            //Return To Homepoint
            //Destroy this specific drop Off Point
            else if (this.gameObject == dropOffPoint)
            {
                hasPizzaOrder = false;
                CustomerFeedBack();
                homePoint.SetActive(true);
                ReturnToBase();
                Destroy(dropOffPoint);
            }

            //mainCamera.enabled = false;
            //cutSceneCamera.enabled = true;
            //anim.SetBool("isTriggered", true);
            //anim["cutsceneAnimation"].wrapMode = WrapMode.Once;
            // anim.Play("cutsceneAnimation");
            //if (anim.clip("cutsceneAnimation"))
            //transform(0, 0, 0);
        }
        //Destroy(other.gameObject);
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
    }

    private void CustomerFeedBack()
    {
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
            foreach (Vector3 location in locations)
            {
                if(newLocation != current && hasPizzaOrder)
                {
                    //Activate green arrow when going to new Location
                    //Arrow should rotate in the direction of the drop Off Point
                    //Find compass youtube video for help
                    Instantiate(dropOffPoint, location, Quaternion.identity);
                    break;
                }
                else if(newLocation != current && hasPizzaOrder == false)
                {
                    ReturnToBase();
                }
            }
            NextMission();                     
        }
     else if(newLocation == current && hasPizzaOrder == false)
        {
            ReturnToBase();
        }
    }
    /*  EnemySpawn()
 {
     for (int i = 0; i< 3; i++)
             { 		
           Instantiate(enemyPrefab, new Vector3(i* 2.0f, 0, player + 10), Quaternion.identity);
            // Instantiate(EnemyPrefab)
            }

             if (numOfEnemies == 0)
         {

         } 

         } */
}
