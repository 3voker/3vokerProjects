using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}
public class PartyFormationPlayerDrag : MonoBehaviour {

    // Use this for initialization
        //[SerializeField]
        //Boundary boundary;

       
        [SerializeField]
        float rotSpeed = 20;

        //[SerializeField]
        //float movSpeed = 20;
        void Start()
    {
        
    }
    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * rotSpeed * Mathf.Deg2Rad;
        float rotY = Input.GetAxis("Mouse Y") * rotSpeed * Mathf.Deg2Rad;

        transform.Rotate(Vector3.up, -rotX);
        transform.Rotate(Vector3.right, rotY);       
    }
//void Movement(){
//        Rigidbody rigidbody;
//        rigidbody = GetComponent<Rigidbody>();
//        float inputX = Input.GetAxis("Horizontal") * movSpeed * Time.deltaTime;
//            float inputZ = Input.GetAxis("Vertical") * movSpeed * Time.deltaTime;

//              Vector3 movement = new Vector3(inputX, 0.0f, inputZ);
//              rigidbody.velocity = movement * movSpeed;
//              rigidbody.position = new Vector3
//        (
//            Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
//            0.0f,
//            Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
//        );
//    }


    }

    // Update is called once per frame

