using UnityEngine;
using System.Collections;
using System;

public class HomePointScript : MonoBehaviour
{

    // Use this for initialization
 
    [SerializeField]
    GameObject dropOffPoint;
    [SerializeField]
    GameObject pizzaSprite;
    SpriteRenderer spriteRenderer;
    Collider dropOffPointCollider;
    bool hasPizzaOrder;
    int newLocation;
    int current;
    [SerializeField]
    Transform[] locations;
    [SerializeField]
    GameObject[] whatToSpawnPrefab;
    [SerializeField]
    GameObject[] whatToSpawnClone;

    void Start()
    {       
        dropOffPointCollider = dropOffPoint.GetComponent<Collider>();
        spriteRenderer = pizzaSprite.GetComponent<SpriteRenderer>();
        QuestTriggers questTriggers = GetComponent<QuestTriggers>();
        CheckforPizza();
        SpawnDropOffs();
    }

    private void CheckforPizza()
    {
        if (!spriteRenderer.enabled)
        {
            hasPizzaOrder = false;
            //StartCoroutine(BossFeedBack());
        }
        //else
            //StartCoroutine(BossFeedBack());
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && hasPizzaOrder == false)

        {          
            //BossFeedBack();
            Debug.Log("HomePoint Reached!");          
            dropOffPoint.SetActive(true);
            dropOffPointCollider.enabled = true;
            hasPizzaOrder = true;
            spriteRenderer.enabled = true;
        }
    }
    //private IEnumerator BossFeedBack()
    //{
    //    if (!spriteRenderer.enabled) {
    //        return YieldInstruction
    //      //  Debug.Log("Come pick up the fresh order!");
    //        //yield return new WaitForSeconds(30);
    //    }
    //}
    void Update()
    {
        CheckforPizza();
    }
    private void NextMission()
    {

        Debug.Log("Has mission!");
        //Randomizes new location
        newLocation = UnityEngine.Random.Range(0, locations.Length);
        //Location selected   
        Debug.Log("New Location: " + newLocation);
        newLocation = current;
        
    }
    private void SpawnDropOffs()
    {
        whatToSpawnClone[0] = Instantiate(whatToSpawnPrefab[0], locations[0].transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
    }

}