using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    float speedRotate = 7f;
    [SerializeField]
    Transform pathParent;

    //Node to reach next
    Transform targetPoint;
    Vector3 distance;
    Quaternion newRotate;
    int index; 

    



    void OnDrawGizmos()
    {
        Vector3 from;
        Vector3 to;
        for (int a=0; a<pathParent.childCount; a++)
        {
            from = pathParent.GetChild(a).position;
            to = pathParent.GetChild((a + 1) % pathParent.childCount).position;
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(from, to);
        }
    }
    void Start()
    {
        index = 0;
        targetPoint = pathParent.GetChild(index);
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        rotate();
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            index++;
            index %= pathParent.childCount;
            targetPoint = pathParent.GetChild(index);
        }
    }
    void rotate()
    {
        distance = transform.position - targetPoint.position; 
        newRotate = Quaternion.LookRotation(distance, transform.forward); //newRotate = Quaternion.LookRotation(distance, transform.forward);
        newRotate.x = 0;
        newRotate.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotate, speedRotate * Time.deltaTime);
    }

}
