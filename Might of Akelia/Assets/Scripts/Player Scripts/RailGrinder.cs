using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RailGrinding : MonoBehaviour
{

    /// <summary>
    /// Activates when player makes contact with a rail 
    /// Instantiates a pathfollowing 
    /// </summary>
    [SerializeField]
    float speed = 3f;
    [SerializeField]
    float speedRotate = 7f;

    [SerializeField]
    float maxRotationBeforeWipeOut = 10f;

    [SerializeField]
    Transform pathParent;

    //Node to reach next
    Transform targetPoint;
    Vector3 distance;
    Quaternion newRotate;
    int index;

    Quaternion playerRotationalInput;

    [SerializeField]
    GameObject objectToFollow;
    //Bool to check if player is grinding rail. 
    bool isGrinding;

    bool isPlayerInAlignment;
    void Start()
    {
        index = 0;
        targetPoint = pathParent.GetChild(index);
    }
    void OnCollision(Collision other) //Might be collider other
    {
        if (other.gameObject.tag == "Rail")
        {
            //Create Object to follow on specific part of rail touched
            Instantiate(objectToFollow, transform.position, playerRotationalInput);
            //Depending on player force when making contact with rail determines initial speed 
            //Object to follow will move the same speed as player 
            RailGrindSpeed();
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrinding = isPlayerInAlignment;
        if (isGrinding)
        {

        }

        //Rotational input of player determines the rotational value to be gauged by Alignment method
        playerRotationalInput.x = Input.GetAxis("leftJoystickHorizontal");

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

       Alignment(newRotate);
    }

    //Return the rotation of of player rotational input - railGrinderRotation and return the value
    bool Alignment(Quaternion railGrinderRotation)
    {
        if (playerRotationalInput.x - railGrinderRotation.x > Mathf.Abs(maxRotationBeforeWipeOut))
        {
            return isPlayerInAlignment = true;
        }
        else 
        {
            return isPlayerInAlignment = false;
        }
    }
    void RailGrindSpeed()
    {

    }
}
