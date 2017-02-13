using UnityEngine;
using System.Collections;

public class Lerp : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject randomObject;
    //[SerializeField]
    //Vector3 startPos;
    //[SerializeField]
    //Vector3 endPos;
    [SerializeField]
    Transform startPosGO;

    [SerializeField]
    Transform endPosGO;
    float startTime;
    float totalDistanceToDestination;

	void Start () {
        startTime = Time.time;
        totalDistanceToDestination = Vector3.Distance(startPosGO.position, endPosGO.position);
	}
	
	// Update is called once per frame
	void Update () {
        float currentDuration = Time.time - startTime;
        float journeyFraction = currentDuration / totalDistanceToDestination;
        transform.position = Vector3.Lerp(startPosGO.position, endPosGO.position, journeyFraction);
	}
}
