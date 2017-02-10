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

    public int newLocation;
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

        CheckMission();
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
            if(this.gameObject == homePoint)
            {
                BossFeedBack();
                NextMission();
            }
            else if (this.gameObject == dropOffPoint)
            {
                CustomerFeedBack();
                ReturnToBase();
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
        throw new NotImplementedException();
    }

    private void CustomerFeedBack()
    {
        throw new NotImplementedException();
    }

    private void NextMission()
    {
        //Randomizes new location
        newLocation = UnityEngine.Random.Range(0, locations.Length); 
        //Location selected   
        locations[newLocation] = new Vector3 current;
        
        Debug.Log("New Location: " + newLocation);   
    }
    private void CheckMission(Vector3 current)
    {
        if(newLocation == current)
        {
            NextMission();
            Instantiate(dropOffPoint, current, Quaternion.identity);
            
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
