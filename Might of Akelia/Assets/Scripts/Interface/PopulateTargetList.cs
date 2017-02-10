using UnityEngine;
using System.Collections;
using System;

public class PopulateTargetList : MonoBehaviour {

    // Use this for initialization

    // Update is called once per frame
    [SerializeField]
    Transform[] nearbyTargets = new Transform[10];

    SpriteRenderer spriteRenderer;
    Color color;

    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter(GameObject other)
    {
        foreach (Transform nearbyTarget in nearbyTargets)
        {
            //case switch? or enumeration for later refactoring
            if (other.transform.tag == "Enemy")
            {
                EnemyTarget();
                addtoArray(other.transform);
                Debug.Log("Enemy Target: " + other.transform.name);
                //Red RGBA is (1, 0, 0, 1)

            }
         
            else if (other.transform.tag == "Hazard")
            {
                HazardTarget();
                addtoArray(other.transform);
                Debug.Log("Hazard Target: " + other.transform.name);
                //Yellow. RGBA is (1, 0.92, 0.016, 1)

            }
            else if (other.transform.tag == "Clue")
            {
                NPCTarget();
                addtoArray(other.transform);
                Debug.Log("Clue Target: " + other.transform.name);
                //Magenta. RGBA is (1, 0, 1, 1). Though I think purple would be better...

            }
            else if (other.transform.tag == "NPC")
            {
                NPCTarget();
                addtoArray(other.transform);
                Debug.Log("NPC Target: " + other.transform.name);
                //Solid blue. RGBA is (0, 0, 1, 1).

            }
            else if (other.transform.tag == "Confirm")
            {
                ConfirmTarget();
                addtoArray(other.transform);
                Debug.Log("Confirm Target: " + other.transform.name);
                //Solid green. RGBA is (0, 1, 0, 1).

            }

            //Create a subtarget condition so that when arraylist is full or player 
            //is doing an action that only effects one target activate subTarget cursors.
            //Cursors over subtargets will be gray and slightly opaque. 
            //else if (nearbyTargets[].Length <= 10)
            //{
            //    SubTarget();
            //    addtoArray(other.transform);
            //    Debug.Log("Sub Target: " + other.transform.name);
            //    //Debug.Log(hit.collider.gameObject.name);

            //}
        }
    }

    private void EnemyTarget()
    {
        throw new NotImplementedException();
    }

    private void ConfirmTarget()
    {
        throw new NotImplementedException();
    }

    private void SubTarget()
    {
        throw new NotImplementedException();
    }

    private void HazardTarget()
    {
        throw new NotImplementedException();
    }

    private void NPCTarget()
    {
        throw new NotImplementedException();
    }

    void Update () {
	
	}
    void addtoArray(Transform obj)
       {
    nearbyTargets.Add(obj);
    }
}
