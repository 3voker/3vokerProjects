using UnityEngine;
using System.Collections;

public class DropOffPointScript : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    GameObject homePoint;

    Collider homePointCollider;
   
    

    void Start()
    {
        homePointCollider = homePoint.GetComponent<Collider>();
        
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")

        {
          
            
            //   CustomerFeedBack();
            Debug.Log("Drop off point Reached!");
            homePoint.SetActive(true);
            homePointCollider.enabled = true;
            ReturnToBase();
        }
    }
    private void CustomerFeedBack()
    {   //Happy, Ok, Angry feed back. Rating from customer gets to boss feedback. 
        Debug.Log("Get Customer FeedBack!");
        //throw new NotImplementedException();
    }
    private void ReturnToBase()
    {
        //Activate red arrow when returning to base
        //Arrow should rotate in the direction of the homePoint
        //Find compass youtube video for help
        //  redArrow.TransformDirection
        //Reactivate homebase GameObject



    }
}
