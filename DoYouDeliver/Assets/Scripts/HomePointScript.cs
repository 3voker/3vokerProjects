using UnityEngine;
using System.Collections;

public class HomePointScript : MonoBehaviour
{

    // Use this for initialization
 
    [SerializeField]
    GameObject dropOffPoint;
   
    Collider dropOffPointCollider;
    bool hasPizzaOrder;
   

    void Start()
    {       
        dropOffPointCollider = dropOffPoint.GetComponent<Collider>();



        QuestTriggers questTriggers = GetComponent<QuestTriggers>();
    }

    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")

        {          
            BossFeedBack();
            Debug.Log("HomePoint Reached!");          
            dropOffPoint.SetActive(true);
            dropOffPointCollider.enabled = true;
            hasPizzaOrder = true;
            
        }
    }
    private void BossFeedBack()
    {
        // throw new NotImplementedException();
       
        
    }
    
}