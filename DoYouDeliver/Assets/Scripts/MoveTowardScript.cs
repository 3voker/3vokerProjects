using UnityEngine;
using System.Collections;

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
        transform.position = Vector3.MoveTowards(transform.position, endPosGO.position, speed * Time.deltaTime);
    }

}
