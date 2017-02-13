using UnityEngine;
using System.Collections;
using System;

public class MoveTowardScript : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Transform startPosGO;
    [SerializeField]
    Transform endPosGO;
    [SerializeField]
    float speed;
    void Update()
    {
        moveTowardsThat();
      
    }

    private void moveTowardsThat()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPosGO.position, speed * Time.deltaTime);
    }
}
