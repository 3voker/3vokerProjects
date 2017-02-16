using UnityEngine;
using System.Collections;

public class DropOffPointScript : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    GameObject homePoint;

    [SerializeField]
    GameObject pizzaSprite;
    Collider homePointCollider;
    SpriteRenderer spriteRenderer;
    bool hasPizzaOrder = false;

    public Vector3[] positions;
    void Start()
    {
        //int randomNumber = UnityEngine.Random.Range(0, positions.Length);
        //transform.position = positions[4];
        homePointCollider = homePoint.GetComponent<Collider>();
        spriteRenderer = pizzaSprite.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" &&  spriteRenderer.enabled)
        {                   
    //   CustomerFeedBack();
            Debug.Log("Drop off point Reached!");
            homePoint.SetActive(true);
            homePointCollider.enabled = true;
            ReturnToBase();
            hasPizzaOrder = false;
            spriteRenderer.enabled = false;
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
