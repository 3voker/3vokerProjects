using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrindableObject : MonoBehaviour, IGrindable, IInteractable
{
    /// <summary>
    /// This script can go on any object that can be grinded on by a player or an agent.
    /// It implements IGrindable, and can thus be interacted with.
    /// The object is placed at a position relative to the player or the player camera by
    /// an offset in the X Y and Z axes.
    /// The object can be dropped by pressing the "Grind" button.
    /// As of right now, this functions such that if a player is holding an object and attempts
    /// to grind on something else, they decouple from the grinded object first.
    /// </summary>
    /*Special note:
     When testing these values out, an x limit of -1.1 to 1.1 and a y limit of 
     -0.6 to 0.6 worked for me as the edges of the screen. I don't know if this scales
     with increased screen size.*/
    [Tooltip("Sets the X offset relative to the agent when the object is picked up.")]
    [SerializeField]
    [Range(-1.1f, 1.1f)]
    private float xOffset = 0f;

    [Tooltip("Sets the Y offset relative to the agent when the object is picked up.")]
    [SerializeField]
    [Range(-0.6f, 0.6f)]
    private float yOffset = -0.6f;

    [Tooltip("Sets the Z offset relative to the agent when the object is picked up.")]
    [SerializeField]
    [Range(-3f, 3f)]
    private float zOffset = 1f;

    [Tooltip("The lerp speed of the object when it is picked up.")]
    [SerializeField]
    [Range(0f, 1f)]
    private float lerpSpeed = 0.1f;

    float step;

    [Tooltip("Sets the object as a child of the Player instead of the Camera.")]
    [SerializeField]
    private bool childToPlayer = false;

    // The offset position is determined by xOffset, yOffset and zOffset
    // It is relative to the player.
    private Vector3 offsetPosition;

    //Path follower script on railBalancer
    FollowPath followRailBalancerPath;

    RailCarScript railCarScript;

    

    [SerializeField]
    GameObject MovementPathGameObject;
    //Movement Path script on Grindable Object
    [SerializeField]
    MovementPath movementPath;

    [SerializeField]
    private MovementPathSpawner movementPathSpawnPath;

    ControlVelocity controlVelocity;

    private IEnumerator<Transform> pointInPath;

    private Vector3 railLocation;
    // The rigidbody currently attached to the Agent GameObject (if any).
    private Rigidbody grinderRigidBody;
    // When a player drops an rail, we want to make sure they don't interact
    // with it again on the same frame.
    private bool wasDroppedThisFrame = false;
    // Similarly, we want to make sure that the object isn't dropped on the same
    // frame it was picked up.
    private bool wasPickedUpThisFrame = false;

    public bool wasRevealedThisFrame = false;

    public bool isRevealed = false;

    public float interactCoolDown = 1;

    public bool isLaunched = false;

    // Is the object currently being grinded?
    protected bool isGrinded;

    // A reference to the agent grinding the rail(object) (if any).
    [SerializeField]
    protected GameObject railGrinder;

    //Prefab of rail Balancer
    public GameObject railBalancerPrefab;

    [SerializeField]
    public GameObject RailPathGameObject;

    private GameObject railBalancer;

    //23280664290005
    //THIS IS SUPPOSE TO BE A REFERENCE TO THE POINT PLAYER STARTS FROM
    [SerializeField]
    public GameObject startPointReference;

    private NavMeshAgent navMeshAgent;

    /// <summary>
    /// Reference to MeshGeneration script on this gameObject 
    /// </summary>
    private MeshGeneration meshGenerator;

    public BoxCollider railCollider;

    public GameObject railCar;

    public GameObject emptyGameObject;

    Vector3 scale;

    private PlayerController playerController;

    protected virtual void Start()
    {
        offsetPosition = new Vector3(xOffset, yOffset, zOffset);

        railCollider = this.gameObject.GetComponent<BoxCollider>();
        railCar.SetActive(false);
        grinderRigidBody = null;
        isGrinded = false;
        railGrinder = null;
        movementPath = null;
        railCarScript = railCar.GetComponent<RailCarScript>();
        meshGenerator = GetComponentInChildren<MeshGeneration>();
    }

    bool DetectHit(Ray ray)
    {
        return railCollider.bounds.IntersectRay(ray);
    }

    protected virtual void Update()
    {
        // Reset the variable so the the object can be interacted with again.
        wasDroppedThisFrame = false;
        wasRevealedThisFrame = RevealInteract(railGrinder);

        // Only drop the object if it is being held and was not picked up this frame.
        if (isGrinded && !wasPickedUpThisFrame && Input.GetButtonDown("Jump"))
        {         
            Debug.Log("GrindableObjectScript Drop Rail Next");
            //Drop the object and make a note that it was dropped this frame.
            DropOffRail(railGrinder, movementPath);

            wasDroppedThisFrame = true;

            //Make railGrinder null so that RevealInteract doesn't stay active
            railGrinder = null;
        }       
        ////If Cool Down timer is greater than 0
        //if (wasRevealedThisFrame)
        //{
        //    meshGenerator = this.GetComponentInChildren<MeshGeneration>();

        //    // meshGenerator.Createmesh(this.transform.position);
        //    meshGenerator.IncreaseVolume();
        //}

        //if (!wasRevealedThisFrame)
        //{
        //    meshGenerator = this.GetComponentInChildren<MeshGeneration>();
        //    //meshGenerator.RemoveMesh();
        //    meshGenerator.DecreaseVolume();
        //}
  
        // Reset the variable so that the object can be dropped again.
        wasPickedUpThisFrame = false;
    }

    protected virtual void FixedUpdate()
    {
        // Move the object to a position relative to the holder if it is currently
        // being held.
        // Also makes sure that the object stays in its place in case of wall
        // collision or other physics events.
        if (isGrinded && railGrinder != null)
        {
            
            //scale = railCar.transform.localScale;
            //railCar.transform.localScale = scale;
            //emptyGameObject.transform.localScale = scale;
            //emptyGameObject.transform.parent = this.transform;
            //railCar.transform.parent = emptyGameObject.transform;
        }
    }
    /// <summary>
    /// Pick the object up when it is interacted with.
    /// </summary>
    /// <param name="agentInteracting">The agent interacting with the object</param>
    public virtual void Grind(GameObject agentInteracting, MovementPath movePath)
    {
        // Only grind the object if it is not currently being grinded and if it was not dropped this frame.
        if (!isGrinded && !wasDroppedThisFrame)
        {
            movePath = MovementPathGameObject.GetComponent<MovementPath>();

            playerController = agentInteracting.GetComponent<PlayerController>();

            //Agent intracting hop on the movement path
            HopOnRail(agentInteracting, movePath);

            //Debug.Log("GrindableObjectScript Hop on Rail Next");
            //Assign Nav mesh agent for interaction
            navMeshAgent = agentInteracting.GetComponent<NavMeshAgent>();

            meshGenerator.canBeTriggered = true;
            //Enable nav mesh for agent interacting
            agentInteracting.GetComponent<NavMeshAgent>().enabled = true;

            #region path Assignment
            //Assign followPath script to agent interacting
            followRailBalancerPath = railBalancer.GetComponent<FollowPath>();
      
            //Assign movement path as the movePath
            movementPath = movePath;

            if(movePath.PathType == MovementPath.PathTypes.generated)
            {
                movePath.movingTo = 2;
            }
            
            if (followRailBalancerPath == null)
            {
                followRailBalancerPath = railBalancer.GetComponent<FollowPath>();
                followRailBalancerPath.MyPath = movePath;
            }

            followRailBalancerPath.grindableObject = this;

            #endregion

            wasPickedUpThisFrame = true;
        }
    }
    protected virtual void HopOnRail(GameObject agentInteracting, MovementPath movePath)
    {
        //Assigns railGrinder(player) as the agent Interacting with the rail
        railGrinder = agentInteracting;

        //Set railCar GameObject active
        railCar.SetActive(true);

        //Assign grinder(player) rigidbody as the railGrinder RigidBody
        Rigidbody grinderRigidBody = railGrinder.GetComponent<Rigidbody>();

        ThirdPersonPlayerCharacter tPPC = agentInteracting.GetComponent<ThirdPersonPlayerCharacter>();

       // playerController.railCar = railCar;

        if (railBalancer == null)
        {
            railBalancer = Instantiate(railBalancerPrefab, railGrinder.transform.position, railGrinder.transform.rotation) as GameObject;

            //Makes railBalancer parent the whole rail. 
            railBalancer.transform.parent = RailPathGameObject.transform;

            //Assign this path to railbalancer followpath script
            followRailBalancerPath = railBalancer.GetComponent<FollowPath>();

            //Assign this path rigidbody script variable to grinderRigidBody
            followRailBalancerPath.playerRigidBody = grinderRigidBody;

            followRailBalancerPath.playerGameObject = railGrinder;
            //Assign this path railCar to this railCar
            followRailBalancerPath.railCar = railCar;

            tPPC.railCar = railCar;
            //Assign this path ControlVelocity script variable to railBalancer ControlVelocity
            followRailBalancerPath.controlVelocityRailBalancer = railBalancer.GetComponent<ControlVelocity>();

            //Assign followPath on railBalancer as this railBalancer instantiated
            followRailBalancerPath.railBalancer = railBalancer;

            //Assign the controlVelocity script 
            //On the rail balancer's follow script 
            //To this railBalancer instantiated 
            followRailBalancerPath.controlVelocityRailBalancer = railBalancer.GetComponent<ControlVelocity>();

            //followRailBalancerPath.grindableObject = this.gameObject.GetComponent<GrindableObject>();      

            followRailBalancerPath.MyPath = movePath;


            //followRailBalancerPath.railCar = railCar;
        }
        CenterOnRail(agentInteracting, movePath, railGrinder.transform.position);
        BalanceOnRail();

        movePath.CanSpawn = true;
      
        //Move rail car to position of player 
        //Below players feet
        //Aligned with RailBalancer 

        if (!railBalancer.gameObject.activeSelf)
        {
            //Turn on rail balancer for reference point 
            railBalancer.gameObject.SetActive(true);

        }
        railCar.transform.forward = railBalancer.transform.forward;
        // Turn off the rotation and gravity for the object.
        TurnOnNavMeshAgent();

        // Ignore collision with the agent holding this object.
      //  IgnoreCollisionWithPlayer(true);

        // The object is currently being held.
        isGrinded = true;
    }

    //Center player on the Center most part of the rail's collider
    //This is to make it so by default player starts in the middle and not unbalanced 
    //Prevent player from falling off the rail upon first contact

    private void CenterOnRail(GameObject agentInteracting, MovementPath movePath, Vector3 contactPoint)
    {
        agentInteracting = railGrinder;
       // movementPath = movePath;
       // contactPoint = movementPath.point
       // railCar.transform.position = railGrinder.transform.localPosition;
        //Initializes the closest Z location of the Rail 
        Vector3 railContactPoint = railCollider.ClosestPoint(railGrinder.transform.position); //railLocation - railGrinder.transform.position
        //Initializes the center of the rail's location adding an offset so that player always is on top of the rail.

        
        //(May be a problem for loop to loops in the future...) 
        Vector3 railCenter = new Vector3(railGrinder.transform.position.x, railGrinder.transform.position.y + yOffset, railGrinder.transform.position.z);

        railBalancer.transform.position = railCenter;
        //movementPath.pointC.transform.position = railCenter;
        //movementPath.PointCLocation(railCenter);
        ///Rail Balancer SHOULD NOT follow railGrinder(Player) 
        ///Rail Balancer leads!
       
        float centerDot = Vector3.Dot(railGrinder.transform.forward, (startPointReference.transform.position - railGrinder.transform.forward).normalized);
        if (centerDot < 0f) { /* Debug.Log("Dot facing toward A: " + centerDot);*/ movePath.facingTowardsB = true; }
        else if (centerDot > 0f) { /*Debug.Log("Dot facing toward B: " + centerDot);*/ movePath.facingTowardsA = true; }

        agentInteracting.transform.position = railCenter + offsetPosition;    
    }

    private void BalanceOnRail()
    {
        float yRotation = railBalancer.transform.eulerAngles.y;

        //railCar.transform.Rotate(railCar.transform.eulerAngles.x, yRotation, railCar.transform.eulerAngles.z);

        //Align railCar to position of railBalancer
        railCar.transform.position = railBalancer.transform.position;
    }

    public virtual void DropOffRail(GameObject agentInteracting, MovementPath movePath)
    {
        movePath = MovementPathGameObject.GetComponent<MovementPath>();
        //Turn off rail balancer reference point

        railGrinder.transform.SetPositionAndRotation(grinderRigidBody.velocity, Quaternion.identity);

        Debug.Log("RailbalancerPath Control Velocity: " + followRailBalancerPath.controlVelocityRailBalancer.transferredVelocity);
        railBalancer.SetActive(false);

        railCar.SetActive(false);

      
       // railGrinder.transform.Translate(0, railGrinder.transform.position.y + 1, railGrinder.transform.position.z + 1);
        // Turn on rotation and gravity for the object.
        TurnOffNavMeshAgent();
        // Stop ignoring collision with the agent holding the object.
      //  IgnoreCollisionWithPlayer(false);

        if(movePath.PathType == MovementPath.PathTypes.generated)
        {
            movePath.canReset = true;
        }
        movementPath = null;
        // The object is no longer being held.
        isGrinded = false;
    }

    //Turn off NavMeshAgent so player canMove and Jump around
    private void TurnOffNavMeshAgent()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }
    }

    //Turn on NavMeshAgent so player can't Move and Jump around and only move in a linear direction based on Velocity
    private void TurnOnNavMeshAgent()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }
    }
    private void LateUpdate()
    {         
           railCar.transform.position = railBalancer.transform.position;

        //Player control tilt of railCar 
        railCar.transform.localRotation = railCarScript.SetRailCarPath(railCar.transform.localRotation.x, railBalancer.transform.eulerAngles.y, railCar.transform.localRotation.z);

            railGrinder.transform.position = railCar.transform.position + offsetPosition;
     
           railGrinder.transform.localRotation = railCar.transform.localRotation;
    }
    /// <summary>
    /// Ignores collision between the object and the player currently holding it.
    /// </summary>
    /// <param name="trueOrFalse">Should collision be ignored or not?</param>
    //private void IgnoreCollisionWithPlayer(bool trueOrFalse)
    //{
    //    // Make sure someone is holding it.
    //    if (railGrinder != null)
    //    {
    //        Collider parentCollider = this.gameObject.GetComponent<Collider>(); //railGrinder.GetComponent<Collider>();
    //        if (parentCollider == null)
    //        {
    //            // In case there is no collider on the agent holding this, look for one in its parent.
    //            parentCollider = this.gameObject.GetComponentInParent<Collider>(); //railGrinder.GetComponentInParent<Collider>();
    //        }
    //        // If a collider is found, ignore collision
    //        if (parentCollider != null)
    //        {
    //            Physics.IgnoreCollision(parentCollider, this.GetComponent<Collider>(), trueOrFalse);
    //        }
    //    }
    //}



    public void Interact(GameObject agent)
    {
        Grind(agent, movementPath);
    }

    public bool RevealInteract(GameObject agent)
    {
        //Interactable is revealed 
         isRevealed = true;
        //If interactable is revealed and agent is present
        if (isRevealed && agent != null && interactCoolDown >= 0)
        {      
            return isRevealed = true;
        }
        //If agent is not present and interactable is revealed
        else if (isRevealed && agent == null)
        {
            //If interaction cool down greater than or equal to 0
            if (interactCoolDown >= 0)
            {
                //Start cool down for interaction
                interactCoolDown -= Time.deltaTime;
                return isRevealed = true;
            }
            return isRevealed = false;
        }
        else
        {
            interactCoolDown = 0.5f;
            
            return isRevealed = false;
        }
    }


    public bool RevealEndOfRail(BoxCollider rail)
    {
        if(rail == null)
        {
            return true;
        }
        else 
        return false;
    }
}
