using UnityEngine;
using System.Collections;
using System;

public class HomingAttack : MonoBehaviour, IActivatable
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float speed;

    new Collider collider;
  
    Rigidbody rigidBody;

    [SerializeField]
    float torque;

    float turn; 
    public string DisplayText
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    public void DoActivate()
    {
        if (this.transform.position.y <= 2 && Input.GetButtonDown("Space"))
        {
            rigidBody.AddForce(transform.forward * speed);
            rigidBody.AddTorque(transform.up * torque * turn);
            collider.enabled = true;
        }

        else
        {
            collider.enabled = false;
        }
    }

    // Use this for initialization
    void OnColliderEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            //if ()  
        }
    }




    void Start () {
        collider = GetComponent<Collider>();
        collider.enabled = false;
        rigidBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
