using UnityEngine;
using System.Collections;
using System;

public class PartyFormationTrigger : MonoBehaviour {

    // Use this for initialization

    bool isOnTrigger = false;
    


    IEnumerator OnTriggerStay(Collider other)
    {
       
        yield return new WaitForSeconds(1);

        if (other.gameObject.tag == "Player") 
			isOnTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		triggerCheck();
	}
	void triggerCheck(){
        if (isOnTrigger)
        {
            
        }

			}
}
