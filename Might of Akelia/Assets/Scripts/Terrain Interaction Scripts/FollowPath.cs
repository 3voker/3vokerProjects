using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour
{
    #region Enums
    public enum MovementType  //Type of Movement
    {
        MoveTowards,
        LerpTowards
    }
    #endregion //Enums

    #region Public Variables
    public MovementType Type = MovementType.MoveTowards; // Movement type used
    /// <summary>
    /// Reference to Movement Path Used
    /// </summary>
    public MovementPath MyPath;

    /// <summary>
    /// Reference to controlVelocityRailBalancer;
    /// </summary>
    public ControlVelocity controlVelocityRailBalancer;

    public GameObject railBalancer;

    public GameObject railCar;

    /// <summary>
    /// Reference to GrindableObject Script
    /// </summary>   
    public GrindableObject grindableObject;

    /// <summary>
    ///  currentPathSpeed object is moving
    /// </summary>
    public float currentPathSpeed = 1;

    /// <summary>
    ///  minPathSpeed object is moving
    /// </summary>
    public float minPathSpeed;

    /// <summary>
    ///  maxPathSpeed object is moving
    /// </summary>
    public float maxPathSpeed;

    /// <summary>
    /// // How close does it have to be to the point to be considered at point
    /// </summary>
    public float MaxDistanceToGoal = .1f;

    float timeSinceStarted;
    /// <summary>
    /// The time taken to move from the start to finish positions
    /// </summary>
    public float timeTakenDuringLerp = 1f;


    #region Quaternion Variables
    /// <summary>
    /// Slow duration by dividing it
    /// </summary>
    public float slowPercentage;

    /// <summary>
    /// Rotational Value to slow the rail balancer for it's rotations
    /// </summary>
    public float slowRotation;

    /// <summary>
    /// Amplifies rail Grinders rotation 
    /// </summary>
    public float railGrinderZRotationAmplifier;
    /// <summary>
    /// The time taken to move from the start to finish rotations
    /// </summary>
    public float timeTakenDuringSlerp = 1f;
    #endregion

    /// <summary>
    /// How much extra distance will the endPoint need to reach
    /// </summary>
    /// 
    public float endPointBuffer;
    /// <summary>
    /// How far the object should move when 'space' is pressed
    /// </summary>
    public float distanceToMove = 10;

    public bool canMoveTowards;

    public bool canLerpTowards;

    #endregion //Public Variables

    #region Private Variables
    /// <summary>
    /// Used to reference points returned from MyPath.GetNextPathPoint
    /// </summary>
    private IEnumerator<Transform> pointInPath;

    /// <summary>
    /// How far the object should move when 'space' is pressed
    /// </summary>
    //Whether we are currently interpolating or not
    [SerializeField]
    public bool isLerping;

    /// <summary>
    /// How far the object should move when 'space' is pressed
    /// </summary>
    //Whether we are currently spherically interpolating or not

    /// <summary>
    /// How far the object should move when 'space' is pressed
    /// </summary>
    [SerializeField]
    private bool isSlerping;

    private bool yeet;
    /// <summary>
    /// The start positions for the linear interpolation
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// The finish positions for the linear interpolation
    /// </summary>
    private Vector3 endPosition;

    /// <summary>
    /// Used as reference from point A to Point B
    /// </summary>
    private Vector3 relativePosition;

    /// <summary>
    /// The start position for the spherical interpolation
    /// </summary>
    private Quaternion startRotation;
    /// <summary>
    /// The finish position for the spherical interpolation
    /// </summary>
    private Quaternion endRotation;

    /// <summary>
    /// Start rotation of rail Balancer
    /// </summary>
    private Quaternion balancerStartRotation;

    /// <summary>
    /// End rotation of rail Balancer
    /// </summary>
    private Quaternion balancerEndRotation;
    /// <summary>
    /// The time when we started the linear interpolation
    /// </summary>
    private float timeStartedLerping;

    /// <summary>
    /// Reference to railBalancer's RigidBody
    /// </summary>
    public Rigidbody railBalancerRigidBody;

    /// <summary>
    /// Reference to player's RigidBody
    /// </summary>
    public Rigidbody playerRigidBody;

    /// <summary>
    /// Reference to railBalancer's RigidBody
    /// </summary>
    public GameObject playerGameObject;
    /// <summary>
    /// TReference to railBalancer's Velocity
    /// </summary>
    public Vector3 currentVelocityRailBalancer;

    /// <summary>
    /// Z Velocity for following the rail 
    /// </summary>
    private float zVelocity = 0.0F;

    /// <summary>
    /// Distance from specific point rail balancer should focus on
    /// </summary>
    public float smoothDampAngleDistance = 5F;

    /// <summary>
    /// Amplifies input of players input to add rotation value
    /// </summary>
    public float rotationalPlayerInputBuffer = 30;

    /// <summary>
    /// Returns angle of two quaternions 
    /// </summary>
    public float angle;
  
    ///// <summary>
    ///// Returns true or false based on RotationalAlignmentCheck method
    ///// </summary>
    //public bool isInAlignment;
    #endregion //Private Variables
    // (Unity Named Methods)

    #region Slerp/Lerp/SmoothDamp Variables
    public float currentDuration;

    public float totalDistance;

    public float percentageComplete;

    public float step;

    float smoothTime = 0.3f;
    #endregion
    #region Main Methods
    public void Start()
    {
        timeStartedLerping = Time.time;
        //Make sure there is a path assigned
        //railBalancerRigidBody = this.gameObject.GetComponent<Rigidbody>();
        controlVelocityRailBalancer = this.gameObject.GetComponent<ControlVelocity>();

        grindableObject = this.gameObject.GetComponentInParent<GrindableObject>();


        if (MyPath == null)
        {
            Debug.LogError("Movement Path cannot be null, I must have a path to follow.", gameObject);
            return;
        }

        //Sets up a reference to an instance of the coroutine GetNextPathPoint
        pointInPath = MyPath.GetNextPathPoint();

        //if(pointInPath == null)
        //{
        //    pointInPath = MyPath.pointC.transform as IEnumerator<Transform>;
        //}
        //   Debug.Log("Start Method pointInPath: " + pointInPath.Current);
        //Get the next point in the path to move to (Gets the Default 1st value)
        pointInPath.MoveNext();
        // Debug.Log("Start Method pointInPath: " + pointInPath.Current);

        //Make sure there is a point to move to
        if (pointInPath.Current == null)
        {
            Debug.LogError("Start Method A path must have points in it to follow, gameObject: " + gameObject);
            return; //Exit Start() if there is no point to move to
        }
    }
    /// <summary>
    /// Called to begin the linear interpolation
    /// </summary>
    void StartLerping()
    {
        isLerping = true;


        //We set the start position to the current position, and the finish to 10 spaces in the 'forward' direction
        //   startPosition = transform.position;

        endPosition = pointInPath.Current.position;

        startRotation = transform.rotation;

        endRotation = pointInPath.Current.rotation;
    }

    //Update is called by Unity every frame
    public void Update()
    {
        //if (!isInAlignment)
        //{
        //    playerJumpOffRail();
        //}
        if (MyPath.PathType == MovementPath.PathTypes.generated || MyPath.PathType == MovementPath.PathTypes.linear)
        {
            MyPath.pointC.transform.position = railCar.transform.position;
            MyPath.pointC.transform.forward = railCar.transform.forward;
        }
        if (MyPath.PathType == MovementPath.PathTypes.loop)
        {
            MyPath.pointC.transform.position = railCar.transform.position;
            MyPath.pointC.transform.forward = railCar.transform.forward;
        }

        //Validate there is a path with a point in it
        if (pointInPath == null || pointInPath.Current == null)
        {
            if (MyPath != null)
            {
                Debug.LogFormat("Update Method pointInPath is null, {0}", pointInPath.Current);
              pointInPath = MyPath.GetNextPathPoint();
               pointInPath.MoveNext();
            }

            return; //Exit if no path is found
        }
        if (MyPath != null)
        {
            //Sets up a reference to an instance of the coroutine GetNextPathPoint
            //  pointInPath = MyPath.GetNextPathPoint();
            //    pointInPath.MoveNext();

            //Debug.Log("Next point in path: " + pointInPath.Current.ToString());
            if (Type == MovementType.MoveTowards) //If you are using MoveTowards movement type
            {
                MoveTowardsTheRailBalancer();
            }
            else if (Type == MovementType.LerpTowards) //If you are using LerpTowards movement type
            {
                if (this.gameObject != null)
                {
                    StartLerping();
                }
            }

            //Check to see if you are close enough to the next point to start moving to the following one
            //Using Pythagorean Theorem
            //per unity suaring a number is faster than the square root of a number
            //Using .sqrMagnitude 
            //var distanceSquared = (this.transform.position - pointInPath.Current.position).sqrMagnitude;
            var distanceSquared = (railCar.transform.position - pointInPath.Current.position).sqrMagnitude;

            //    Debug.Log("DistanceSquared is: " + distanceSquared);

           
            //Distance is based on the RAILBALANCER NOT RAILCAR OR PLAYER
            if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal) //If you are close enough
            {
                if (MyPath.PathType == MovementPath.PathTypes.generated || MyPath.PathType == MovementPath.PathTypes.linear)
                {
                    if (MyPath.reachedEndofPathSequence)
                    {
                        playerJumpOffRail();
                    }
                }
                //Set the position of this object to the position of our starting point
                transform.position = pointInPath.Current.position; //Distance Squared maybe?

                //Get the next point in the path to move to (Gets the Default 1st value)
                pointInPath.MoveNext();

                //Debug.Log("Moving to next point!" + pointInPath.Current.ToString());
                endPosition = pointInPath.Current.position;
            }
            //The version below uses Vector3.Distance same as Vector3.Magnitude which includes(square root)
            if (MyPath.skrt == true)
            {
                distanceSquared = Vector3.Distance(transform.position, pointInPath.Current.position);

                float quarterDistance = distanceSquared / 4;

                if (distanceSquared <= quarterDistance) //If you are close enough
                {
                    yeet = true;
                }
            }

        }
        //else grindableObject = null;        
    }
    private void FixedUpdate()
    {
        //We want percentage = 0.0 when Time.time = _timeStartedLerping
        //and percentage = 1.0 when Time.time = _timeStartedLerping + timeTakenDuringLerp
        //In other words, we want to know what percentage of "timeTakenDuringLerp" the value
        //"Time.time - _timeStartedLerping" is.
        currentDuration = Time.time - timeStartedLerping;

        totalDistance = Vector3.Distance(startPosition, endPosition);

        percentageComplete = currentDuration / totalDistance;

        step = currentPathSpeed * Time.deltaTime;

        smoothTime = 0.3f;

        if (this.gameObject.activeSelf)
        {
            controlVelocityRailBalancer.modeSwitching = ControlVelocity.ModeSwitching.VelocityChange;

            //    pointInPath = MyPath.GetNextPathPoint() as IEnumerator<Transform>;

          //  controlVelocityRailBalancer.RailGrindingVelocity(currentVelocityRailBalancer, railCar.transform.position); // pointInPath.Current.position

            this.transform.forward = pointInPath.Current.position;

            if (isLerping)
            {
                startPosition = transform.position;

                endPosition = new Vector3(pointInPath.Current.position.x,
                pointInPath.Current.position.y, pointInPath.Current.position.z);
                //Debug.Log("Transform Position: " + transform.position.ToString());
                //Debug.Log("Start Position: " + startPosition.ToString());
                //Debug.Log("End Position: " + endPosition.ToString());
                //If GO with tag RailPlatform

                maxPathSpeed = controlVelocityRailBalancer.maxRailSpeed;

                Vector3 lookDirection = (this.transform.localPosition - pointInPath.Current.localPosition);

           
                //this.transform.forward = lookDirection;

                //railBalancerRigidBody.MovePosition(endPosition); //railBalancerRigidBody.MovePosition(transform.position + playerRigidBody.velocity);

                this.transform.position = Vector3.SmoothDamp(startPosition, endPosition, ref currentVelocityRailBalancer,
                             smoothTime, maxPathSpeed, percentageComplete / (slowPercentage + smoothTime));

                // transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete / (slowPercentage + smoothTime));

                // Debug.Log("whatNumber is Nan: " + this.transform.position.ToString());
                if (percentageComplete >= .50f)
                {
                    isLerping = false;
                    isSlerping = true; //Should remove, not necessary.
                }
            }

            Quaternion newRotation = Quaternion.FromToRotation(startPosition, endPosition);

            if (isSlerping)
            {
                //Set the Quaternion rotation from the GameObject's position to the next GameObject's position


                //Rotate the GameObject towards the second GameObject


                //Sets rotation to spherically interpolate towards each corresponding end Rotation.
                //That end rotation will be the point in path minus this gameObjects transform.
                relativePosition = (pointInPath.Current.localPosition - transform.localPosition); // (pointInPath.Current.position - transform.position);

                //Start rotation is this game objects transform local rotation
                startRotation = transform.rotation;

                //End rotation is the relative position of this gameObject and the point in path
                //referencing this transforms up vector in world space 
                endRotation = Quaternion.LookRotation(relativePosition, pointInPath.Current.transform.up);

                float zAngle = Mathf.SmoothDampAngle(startRotation.eulerAngles.z, endRotation.eulerAngles.z, ref zVelocity, maxPathSpeed, percentageComplete / slowPercentage);

                if (yeet)
                {
                    zAngle = Mathf.SmoothDampAngle(startRotation.eulerAngles.z, endRotation.eulerAngles.z + MyPath.DotResult * railGrinderZRotationAmplifier, ref zVelocity, maxPathSpeed, percentageComplete / slowPercentage);
                }
                Vector3 position = endPosition;

                position += Quaternion.Euler(0, 0, zAngle) * new Vector3(0, 0, -smoothDampAngleDistance);

                transform.rotation = Quaternion.Slerp(startRotation, endRotation, percentageComplete / (slowPercentage + slowRotation));

            }

            if (totalDistance == 0)
            {
                totalDistance = 0;
                currentDuration = timeStartedLerping;
                //this.gameObject.SetActive(false);
            }

        }
        controlVelocityRailBalancer.RailGrindingVelocity(railBalancerRigidBody.velocity, endPosition);
    }

    ///// <summary>
    ///// Method called from the player controller to rotate the Rail Cart X and Z rotational axis. 
    ///// Y rotational axis is determined by the railBalancer's position
    ///// </summary>
    //public Quaternion SetRailCarPath(float playerYInput, float railBalancerYAxis, float playerXInput)
    //{
    //    //Assign player y input to the railCar X rotational axis 
    //    playerYInput = Input.GetAxis("leftJoystickVertical");
    //    if (invertY) { playerYInput = -Input.GetAxis("leftJoystickVertical"); }

    //    //Assign railBalancer Y axis to the railBalancer's Y rotation
    //    railBalancerYAxis = railBalancer.transform.eulerAngles.y;

    //    //Assign player x input to the railCar Z rotational axis 
    //    playerXInput = Input.GetAxis("leftJoystickHorizontal");
    //    if (invertX) { playerXInput = -Input.GetAxis("leftJoystickHorizontal"); }


    //    //If controlVelocity script on RailBalancer is autoBalancing 
    //    //Reduce player rotational control of railCart
    //    //Also ease up the railCheckAlignmentCheck requirements to stay on Rail
    //    if (!controlVelocityRailBalancer.isAutoBalancing)
    //    {
    //        //To do soon 
    //        //Probably halve or muffle player input to keep player on Path

    //    }

    //    Quaternion rotationalValue = Quaternion.Euler(playerYInput * rotationalPlayerInputBuffer, railBalancerYAxis, playerXInput * rotationalPlayerInputBuffer);

    //    float currentDuration = Time.time - timeStartedLerping;

    //    float totalDistance = Vector3.Distance(startPosition, endPosition);

    //    float percentageComplete = currentDuration / totalDistance;

    //    float step = currentPathSpeed * Time.deltaTime;

    //    float smoothTime = 0.3f;

    //    //Assign start position as rail car's current position
    //    Vector3 railCarStartPosition = railCar.transform.position;

    //    //end position is railbalancers velocity, endPosition
    //    Vector3 railCarEndPosition = railBalancer.gameObject.transform.position;

    //    //railCar.transform.rotation = Quaternion.Slerp(railCar.transform.rotation, rotationalValue, Time.deltaTime * smoothTime);
    //    isInAlignment = RotationalAlignmentCheck(railCar.transform.rotation, railBalancer.transform.rotation);

    //    return rotationalValue;
    //}

    //private bool RotationalAlignmentCheck(Quaternion railCartRotation, Quaternion railBalancerRotation)
    //{
    //    float rotationalRange = 15f;

    //    float maxRotationalRange = 45f;

    //    if (angle == 0)
    //    {
    //        return true;
    //    }
    //    angle = Quaternion.Angle(railCartRotation, railBalancerRotation);
    //    //If rail car rotation is outside of specific float range of rail balancer rotation slow down toward min speed
    //    if (angle >= rotationalRange)
    //    {
    //        return true;
    //    }
    //    //If rail car rotation is inside of specific float range of rail balancer rotation speed up toward max speed
    //    if (angle <= rotationalRange)
    //    {
    //        return true;
    //    }
    //    //If rail car rotation is outside of specific float range of rail balancer rotation fall off rail
    //    if (angle > maxRotationalRange)
    //    {
    //        return false;
    //    }

    //    return false;
    //}

    //Don't think i'm using this...
    private void MoveTowardsTheRailBalancer()
    {
        float step = currentPathSpeed * Time.deltaTime;
        //If GO with tag RailPlatform
        if (this.gameObject.tag == "RailPlatform")
        {
            Debug.Log("Move Towards Method Point in path: " + pointInPath.Current);
            Vector3 lookDirection = (this.transform.position - pointInPath.Current.position);
            //Set new direction to movetowards as the 
            Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, lookDirection * -1, step, 0f); //Vector3 newDirection = Vector3.RotateTowards(grindableObject.railBalancerPrefab.transform.forward, lookDirection, step, 0f);

            this.transform.position = Vector3.MoveTowards(grindableObject.railBalancerPrefab.transform.position,
                              pointInPath.Current.position,
                              step);
        }

    }
    #endregion //Main Methods

    //(Custom Named Methods)
    #region Utility Methods 

    public void playerJumpOffRail()
    {
        grindableObject.DropOffRail(playerGameObject, MyPath);
    }

    public void AssignParent(GameObject parent)
    {
        parent = MyPath.gameObject;
        this.transform.parent = MyPath.transform;
    }
    #endregion //Utility Methods

    //Coroutines run parallel to other fucntions
    #region Coroutines

    #endregion //Coroutines
}
