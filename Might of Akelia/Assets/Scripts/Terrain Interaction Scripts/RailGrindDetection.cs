using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Rail Grind Detection to detect when rail begins and 
/// ends and exact reference point to player touches
/// This goes on the RailBalancer Game Object
/// </summary>
public class RailGrindDetection : MonoBehaviour {

    /// <summary>
    /// Reference to Movement Path Used
    /// </summary>
    public MovementPath MyPath;

    /// <summary>
    /// Reference to railBalancer's RigidBody
    /// </summary>
    public GameObject playerGameObject;

    /// <summary>
    /// Reference to GrindableObject Script
    /// </summary>   
    public GrindableObject grindableObject;

    /// <summary>
    /// Reference to gameObject collided Script
    /// </summary>   
    GameObject railPointofReference;
    /// <summary>
    /// Distance of raycast to acitvate/deactivate rail grinding
    /// </summary>
    [SerializeField]
    float distanceToRail = 3;

    /// <summary>
    /// Layer grinableObjects are on 
    /// </summary>
    [SerializeField]
    LayerMask layerGrindableObjectsAreOn;


    private IGrindable railObjectLookedAt;

    /// <summary>
    /// Bool for other scripts to reference if Rail Balancer encounters new reference points
    /// New references can be Start Points, End Points, cracked rails, etc...
    /// </summary>
    public bool offRail;

    void Start () {
        grindableObject = this.gameObject.GetComponentInParent<GrindableObject>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit raycastHit;

        //THIS IS WHERE YOU ARE DRE
        //May move this to separate script on rail car
        //Raycasts to check for end point of rail to make player jump off and deactivate grinder/rail car
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit,
           distanceToRail, layerGrindableObjectsAreOn))
        {
            //if our ray hits something, we go into this code block           
            if (grindableObject == null)
            {
                grindableObject = raycastHit.collider.gameObject.GetComponentInParent<GrindableObject>();

                railObjectLookedAt = grindableObject.GetComponent<IGrindable>();

                railPointofReference = raycastHit.collider.gameObject;
            }

            if (railObjectLookedAt == null)
            {
                throw new System.Exception(raycastHit.collider.gameObject.name +
                " MUST have a script that implements IGrindable script attached to it.");
            }
            CheckedForLookedAtObjects();
            UpdateRailBalancerLookedAtObject();
        }
        else grindableObject = null;      
    }

    private void CheckedForLookedAtObjects()
    {
        Vector3 endPoint = transform.position + distanceToRail * transform.forward;
        Debug.DrawLine(transform.position, endPoint, Color.red);
    }

    private void UpdateRailBalancerLookedAtObject()
    {     
        if (railObjectLookedAt != null)
        {
            if (railPointofReference.tag == "Point A")
            {
                Debug.Log("Rail Balancer reached Point A");
                grindableObject.DropOffRail(playerGameObject, MyPath);

                if(MyPath.PathType == MovementPath.PathTypes.generated)
                {
                    MyPath.isGenerated = true;
                }

                offRail = true;
            }
            if (railPointofReference.tag == "Point B")
            {
                Debug.Log("Rail Balancer reached Point B");
                grindableObject.DropOffRail(playerGameObject, MyPath);
                if (MyPath.PathType == MovementPath.PathTypes.generated)
                {
                    MyPath.isGenerated = true;
                }
                offRail = true;
            }
        }
    }
}
