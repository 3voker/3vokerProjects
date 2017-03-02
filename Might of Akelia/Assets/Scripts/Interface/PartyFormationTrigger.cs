using UnityEngine;
using System.Collections;

public class PartyFormationTrigger : MonoBehaviour {

	// Use this for initialization

	bool isOnTrigger = false; 
	void OnTriggerStay(GameObject other){
		yield return WaitForSeconds (WaitForSeconds: 1);
		if (other.gameObject.tag == "Player") {
			isOnTrigger = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		triggerCheck();
	}
	void triggerCheck(){


			}
}
