using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// This heavily controls the movement path in which pathSequence moves upon. 
/// Can Create new paths
/// Destroy old paths
/// Move paths towards sequential paths directions
/// Rotate paths to better align with sequential paths
/// </summary>
public class MovementPath : MonoBehaviour
{
    #region Enums

    public enum PathIndexes { zero, ichi, ni, san, chi, go, roku, sichi, hachi, qu, ju}
    public enum PathTypes //Types of movement paths
    {
        linear,
        loop, generated
    }
    #endregion //Enums

    #region Public Variables
    /// <summary>
    /// Bool that checks if point in path reached end of pathSequence 
    /// </summary>
    public bool reachedEndofPathSequence; 

    public PathTypes PathType; //Indicates type of path (Linear or Looping)

    public PathIndexes PathIndex;

    [SerializeField]
    public MovementPathSpawner movementPathSpawner;

    [SerializeField]
    MovementPathNode[] movementPathNode;

    public int movementDirection = 1; //1 clockwise/forward || -1 counter clockwise/backwards

    public int movingTo = 0; //used to identify point in PathSequence we are moving to
    /// <summary>
    /// List of all points in the path 
    /// </summary>
    public List<Transform> PathSequence; 
    /// <summary>
    /// Nodes that are planned to be skipped
    /// </summary>
    private List<Transform> SkippedNodes;
    /// <summary>
    /// Nodes that are currently active
    /// </summary>
    [SerializeField]
    private List<GameObject> ActiveNodes;

    public int pathSequenceListCapacity = 10;

    public GameObject generatedPointPrefab;


    public Collider pathCollider;
    /// <summary>
    /// Reference to a generated point Prefab gameobject 
    /// </summary>
    private GameObject movementPathPointSpawnerGameObject;

    [SerializeField]
    private GameObject endofPath;

    [SerializeField]
    private GameObject startofPath;

    [SerializeField]
    private GameObject parentGameObject;

    private GrindableObject grindableObject;

    private Vector3 currentPosition;

    private Vector3 nextPosition;

    private Vector3 lastPosition;

    private Vector3 relativePosition;

    private Vector3 pointUp;

    /// <summary>
    /// Current rotation of current path. 
    /// </summary>
    Vector3 vectorResult;

    Vector3 startingPointForward;
    /// <summary>
    /// Current rotation of current path. 
    /// </summary>
    private Quaternion currentOrientation;
    private Quaternion nextOrientation;
    private Quaternion lastOrientation;

    private Quaternion lookAtNextObjectInPath;

    private Quaternion currentRotation;


    public Vector3 pointCPosition;

    private static ObjectPooler objectPooler;
    /// <summary>
    /// Adds low rotational curve to the next rotation
    /// </summary>
    public float softTurnBuffer;

    /// <summary>
    //// Adds high rotational curve to the next rotation
    /// </summary>
    public float hardTurnBuffer;

    public float buffer;

    public float turnTimer;

    public float totalDistance;
    /// <summary>
    /// Buffer to pointC before nodes are declared inactive
    /// </summary>
    private float safeDistanceBuffer = 5;
    #endregion //Public Variables


    public float timeStartedSlerping;
    #region Private Variables
    private int maxPathSequenceValue = Int32.MinValue;
    private int maxIndex = -1;
    public float DotResult;

    [SerializeField] private float tiltMax = 75f; // The maximum value of the x axis rotation of the pivot.
    [SerializeField] private float tiltMin = 45f; // The minimum value of the x axis rotation of the pivot.
    private float tiltAngle; // The pivot's x axis rotation.

    /// <summary>
    /// Toggles if turn is greater than 0 degrees Z axis 
    /// </summary>
    private bool softTurn;
    /// <summary>
    /// Toggles if turn is greater than 30 degrees Z axis 
    /// </summary>
    private bool hardTurn;
    /// <summary>
    /// Toggles if turn is greater than 0 degrees Z axis in the negative
    /// </summary>
    private bool turnLeft;
    /// <summary>
    /// Toggles if turn is greater than 0 degrees Z axis in the positive
    /// </summary>
    private bool turnRight;

    public bool skrt;
    /// <summary>
    /// Reference to the start point of  movementPath
    /// </summary>
    public Transform pointA;
    /// <summary>
    /// Reference to the end point of movementPath
    /// </summary>
    public Transform pointB;
    /// <summary>
    /// Reference to the contact point of the player to movementPath
    /// </summary>
    public Transform pointC;
    /// <summary>
    /// Reference to a generated point Prefab 
    /// </summary>
    /// 

    private int nodeAmount = 7;
    #region Bools
    /// <summary>
    /// Toggles whether path has been generated
    /// </summary>
    public bool isGenerated;

    /// <summary>
    /// Toggles whether 
    /// </summary>
    public bool canReset = false;

    /// <summary>
    /// Toggles whether  nodes are generating towards path A
    /// </summary>
    [SerializeField]
    public bool facingTowardsA = false;

    /// <summary>
    /// Toggles whether nodes are generating towards path B
    /// </summary>
    [SerializeField]
    public bool facingTowardsB = false;

    /// <summary>
    /// Protected Variable Toggles whether nodes can spawn 
    /// </summary>
    protected bool canSpawn = true;

    /// <summary>
    /// Toggles whether nodes can spawn
    /// </summary>
    [SerializeField]
   public bool CanSpawn { get { return canSpawn; } set { canSpawn = value; } }

    /// <summary>
    /// Toggles whether path will loop while generated 
    /// </summary>
    public bool canLoopWhileGenerated = false;
    #endregion

    #endregion //Private Variables

    // (Unity Named Methods)
    #region Main Methods

    private void Start()
    {
        GrindableObject grindableObject = parentGameObject.GetComponentInChildren<GrindableObject>();

        PathSequence.Clear();      
    }
    private void Awake()
    {
        CanSpawn = false;
    }
    private void FixedUpdate()
    {
        if (pointA != null)
        {
            pointA.transform.position = startofPath.transform.position;
            PathSequence[0] = pointA.transform;
        }

        if(pointC != null)
        {
            pointC.transform.position = pointCPosition;
        }
    
    }
    private void LateUpdate()
    {     
        pointUp = Vector3.up;
     //   Debug.LogFormat("Path Sequence Count is: ", PathSequence.Count);

#region Path Sequence check
        if(PathSequence != null)
        {
            pointUp = Vector3.up;

     //       Debug.Log("Yoooo are you not null, also canSpawn: " + CanSpawn.ToString());
       
            var nodeGroup = Resources.FindObjectsOfTypeAll<MovementPathNode>();

            if (pointA == null)
            {
                foreach (MovementPathNode node in nodeGroup)
                {
                    if (node.gameObject.tag == "Point A")
                    {
                        //pointA = node.transform;
                        node.gameObject.SetActive(true);
                     
                        if (!PathSequence.Contains(node.transform))
                        {                        
                            if (node.isActiveAndEnabled)
                            {
                                if (node.isPartOfPath)
                                {
                                    pointA = node.transform;

                                    //PathSequence.Add(node.transform);
                                    //PathSequence[0] = node.transform;
                                    PathSequence = PathSequence.Distinct().ToList();
                                    pointA.transform.position = startofPath.transform.position;
                                }
                               
                            }                        
                        }
                    }
                }
            }

            if (pointB == null)
            {
                foreach (MovementPathNode node in nodeGroup)
                {
                    if (node.gameObject.tag == "Point B")
                    {                    
                        node.gameObject.SetActive(true);

                        if (!PathSequence.Contains(node.transform))
                        {
                            if (node.isActiveAndEnabled)
                                    {
                                if (node.isPartOfPath)
                                {
                                    pointB = node.transform;
                                    PathSequence.Add(node.transform);


                                    if (PathSequence[PathSequence.Count - 1] != node.transform)
                                    {
                                        SkippedNodes.Add(node.transform);
                                    }
                                    PathSequence = PathSequence.Distinct().ToList();
                                    node.transform.position = endofPath.transform.position;
                                }
                            }                          
                        }                       
                    }
                }
            }

            if (pointC == null)
            {
                foreach (MovementPathNode node in nodeGroup)
                {
                    if (node.gameObject.tag == "Point C")
                    {                    
                        node.gameObject.SetActive(true);

                        if (!PathSequence.Contains(node.transform))
                        {
                            if (node.isActiveAndEnabled)
                            {
                                if (node.isPartOfPath)
                                {
                                    pointC = node.transform;
                                    PathSequence.Add(node.transform);
                                    PathSequence[1] = node.transform;
                                    PathSequence = PathSequence.Distinct().ToList();
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                foreach (MovementPathNode node in nodeGroup)
                {
                    if (!PathSequence.Contains(node.transform))
                    {
                        if (node.isActiveAndEnabled)
                        {
                            if (node.isPartOfPath)
                            {
                                PathSequence.Add(node.transform);
                                PathSequence = PathSequence.Distinct().ToList();
                            }
                            else
                                PathSequence.Remove(node.transform);
                                PathSequence = PathSequence.Distinct().ToList();
                        }
                        
                    }                   
                }
              
            }
            
#endregion
            RegisterPath();
          
            RegisterPosition();
            RegisterRotation();
        }
    }
    /// <summary>
    /// Registers a path to create for the generated path type
    /// </summary>
    public void RegisterPath()
    {
       
            
        //Check if path type is generated
        if (PathType == PathTypes.generated && CanSpawn)
        {
            movementPathSpawner = GetComponentInChildren<MovementPathSpawner>();
            movementPathSpawner.movementPathNode = GetComponentInChildren<MovementPathNode>();

            PathSequence = GeneratePath(PathSequence, pointA, pointB, pointC);
        }
        if (isGenerated && canReset)
        {
            ResetPathSequence(PathSequence, pointA, pointB, pointC);
        }
    }

    #region checkingTurn Values
    private void RegisterRotation()
    {
        if (DotResult <= Mathf.Abs(.50f))
        {
            softTurn = true;
            hardTurn = false;
            skrt = true;
        }
        if (DotResult > Mathf.Abs(.60f))
        {
            softTurn = false;
            hardTurn = true;
            skrt = true;
        }
        if (DotResult > 0)
        {
            turnLeft = false;
            turnRight = true;
            skrt = true;
            Debug.DrawRay(PathSequence[movingTo].position, Vector3.up * 100, Color.red);
        }
        else if (DotResult < 0)
        {
            turnRight = false;
            turnLeft = true;
            skrt = true;
            Debug.DrawRay(PathSequence[movingTo].position, Vector3.up * 100, Color.green);
        }
    }
    #endregion

    private void RegisterPosition()
    {       
        //For each point in the path sequence
        foreach (Transform item in PathSequence)
        {
            //If the item matches the transform in the next point in the path sequence
            if (item == PathSequence[movingTo].transform)
            {
                if (movingTo == PathSequence.Count)
                {
                    item.localRotation.SetLookRotation(PathSequence[0].transform.position, pointUp); //item.localRotation.SetLookRotation(PathSequence[0].transform.position, PathSequence[0].transform.up);
                }
                else if (movingTo >= PathSequence.Count - 1)
                {
                    movingTo = 0;
                    item.localRotation.SetLookRotation(PathSequence[movingTo].transform.position, pointUp); // item.localRotation.SetLookRotation(PathSequence[movingTo].transform.position, PathSequence[0].transform.up);
                }
                else if(movingTo + 1 < PathSequence.Count)
                {
                    item.localRotation.SetLookRotation(PathSequence[movingTo + 1].transform.position, pointUp); //item.localRotation.SetLookRotation(PathSequence[movingTo + 1].transform.position, PathSequence[movingTo + 1].transform.up);
                }
                currentPosition = item.transform.position;        
            }           
        }
    }

    //OnDrawGizmos will draw lines between our points in the Unity Editor
    //These lines will allow us to easily see the path that
    //our moving object will follow in the game
    public void OnDrawGizmos()
    {
        //Make sure that your sequence has points in it
        //and that there are at least two points to constitute a path
        if(PathSequence == null || PathSequence.Count < 2)
        {
            return; //Exits OnDrawGizmos if no line is needed
        }

        //If Path Sequence is present and not equal to null
        if(PathSequence != null)
        {
           //If Path Sequence does not have within its list any null objects
            if (!PathSequence.Contains(null))
            {
                //Loop through all of the points in the sequence of points
                for (var i = 1; i < PathSequence.Count; i++)
                {
                    //Draw a line between the points
                    Gizmos.DrawLine(PathSequence[i - 1].position, PathSequence[i].position);
                }
            }
            //Otherwise return if nulls are present within the list.
            else
                return;
        }

        //If your path loops back to the beginning when it reaches the end
        if(PathType == PathTypes.loop)
        {
            //Draw a line from the last point to the first point in the sequence
            Gizmos.DrawLine(PathSequence[0].position, PathSequence[PathSequence.Count - 1].position);

            lastPosition = PathSequence[0].position;
            currentPosition = PathSequence[PathSequence.Count - 1].position;

            lastOrientation = PathSequence[0].rotation;
            currentOrientation = PathSequence[PathSequence.Count - 1].rotation;
        }

        if(PathType == PathTypes.linear)
        {
            //Draw a line from the last point to the first point in the sequence
            Gizmos.DrawLine(PathSequence[0].position, PathSequence[PathSequence.Count - 1].position);

        }
        if (PathType == PathTypes.generated)
        {
            //Draw a line from the last point to the first point in the sequence
            Gizmos.DrawLine(PathSequence[0].position, PathSequence[PathSequence.Count - 1].position);
            
        }
    }
    #endregion //Main Methods

    //(Custom Named Methods)
    #region Utility Methods 

    #endregion //Utility Methods

    //Coroutines run parallel to other fucntions
    #region Coroutines
    //GetNextPathPoint() returns the transform component of the next point in our path
    //FollowPath.cs script will inturn move the object it is on to that point in the game
    public IEnumerator<Transform> GetNextPathPoint()
    {
        //Make sure that your sequence has points in it
        //and that there are at least two points to constitute a path
        if (PathSequence == null || PathSequence.Count < 1)
        {
            yield break; //Exits the Coroutine sequence length check fails
        }

        while(true) //Does not infinite loop due to yield return!!
        {
            if (movingTo >= PathSequence.Count)
            {
              relativePosition = (PathSequence[movingTo].transform.localPosition - PathSequence[0].transform.localPosition);
              DotResult = Vector3.Dot(PathSequence[movingTo].forward, PathSequence[0].forward);
            }
            //Return the current point in PathSequence
            //and wait for next call of enumerator (Prevents infinite loop)
            else if (movingTo == 0)
            {
                for (int i = 0; i < PathSequence.Count; i++)
                {
                    int value = i;
             
                    if (value > maxPathSequenceValue)
                    {
                        maxPathSequenceValue = value;
                        maxIndex = i;
                        break;
                    }
                }
                PathSequence[0].LookAt(PathSequence[movingTo + 1]);
                DotResult = Vector3.Dot(PathSequence[movingTo].forward, PathSequence[movingTo + 1].forward);
            }
            //If moving to the first point in the path sequence 
            else if (movingTo + 1 > PathSequence.Count - 1)
            {
               
                PathSequence[movingTo].localRotation = Quaternion.Euler(new Vector3(0, 30, 0)); //.Euler(0, 15, 0);

                nextPosition = PathSequence[movingTo].position;
                nextOrientation = PathSequence[movingTo].rotation;

                lastPosition = PathSequence[movingTo].position;
                lastOrientation = PathSequence[movingTo].rotation;
           
                //Debug.Log("RelativePosition should be looking at: " + PathSequence[movingTo].transform.name);
                //Debug.Log("PathSequence:  " + PathSequence[movingTo].transform.name + " is at position " + relativePosition);
            }
            else if (movingTo + 1 <= PathSequence.Count - 1 && movingTo != 0)
            {
                //Create a rotation to look at the next object in line for the movement
                PathSequence[movingTo].LookAt(PathSequence[movingTo + 1]);
                nextPosition = PathSequence[movingTo + 1].position;
                nextOrientation = PathSequence[movingTo + 1].rotation; //origionally localrotation

                DotResult = Vector3.Dot(PathSequence[movingTo].forward, PathSequence[movingTo + 1].forward);
           
                 
                //Debug.Log("RelativePosition should be looking at: " + PathSequence[movingTo + 1].transform.name);
                //Debug.Log("PathSequence:  " + PathSequence[movingTo + 1].transform.name + " is at position " + relativePosition);
            }

            else {lastPosition = PathSequence[movingTo - 1].position; lastOrientation = PathSequence[movingTo - 1].localRotation; }

            if (softTurn) { if (turnLeft) { softTurnBuffer = 15f; } else if (turnRight) { softTurnBuffer = -15f; turnTimer = .25f; } hardTurnBuffer = 0f; }
            if (hardTurn) { if (turnLeft) { hardTurnBuffer = 30f; } else if (turnRight) { hardTurnBuffer = -30f; turnTimer = .25f; } softTurnBuffer = 0f; }

            buffer = softTurnBuffer + hardTurnBuffer;

            PathSequence[movingTo].Rotate(0, 0, buffer);
            currentRotation = PathSequence[movingTo].localRotation;
        
            currentPosition = PathSequence[movingTo].position;          
            currentOrientation = PathSequence[movingTo].localRotation;


            if (PathType == PathTypes.generated)
            {
                //foreach (Transform item in PathSequence)
                //{
                //    if (SkippedNodes.Contains(pointB))
                //    {
                //        if(movingTo <= PathSequence.Count - 1)
                //        {
                //            yield return PathSequence[movingTo + 1];
                //        }
                       
                //    }
                //    //else if (!SkippedNodes.Contains(pointA))
                //    //{
                //    //    yield return PathSequence[movingTo];
                //    //}
                //    //else if (!SkippedNodes.Contains(pointC))
                //    //{
                //    //    yield return PathSequence[movingTo + 1];
                //    //}
                //    else
                        yield return PathSequence[movingTo];
              //  }             
            }




            yield return PathSequence[movingTo]; 
//*********************************PAUSES HERE******************************************************//
            //If there is only one point exit the coroutine
            if(PathSequence.Count == 1)
            {
                continue;
            }

            //If Linear path move from start to end then end to start then repeat
            if (PathType == PathTypes.linear)
            {
                //If you are at the begining of the path
                if (movingTo <= 0)
                {
                    movementDirection = 1; //Seting to 1 moves forward             
                }
                //Else if you are at the end of your path
                else if (movingTo >= pathSequenceListCapacity - 1)
                {
                    movementDirection = -1; //Seting to -1 moves backwards
                }
            }

            //For generated path you must create a new index when you reach 
            //the end of the path

            //Auto generate will find the beginning point and end point any rail 
            //Will generate a new point every 2-3 meters from previous point
            //From player contact point will create new points in either 
            //direction until completing the path. 

            //Path erases or sets active false upon single usage. 
            //Would be a good time to learn object pooling. 
            //9-12 points is all that should be needed one generated 
            if (PathType == PathTypes.generated)
            {
                ////If A is connected to B 
                //if (canLoopWhileGenerated)
                //{
                //    movingTo = 0;
                //}
                if(movingTo <= 0)
                {
                    movementDirection = 1;
                  //  movingTo = movingTo + movementDirection;
                }
                else if (PathSequence.Count > 10)
                {
                   // canSpawn = false;
                    isGenerated = true;
                }
                else if (movingTo + 1 >= PathSequence.Count)
                {
                    movementDirection = -1;
                }
                else if(movingTo == PathSequence.Count)
                {
                    reachedEndofPathSequence = true;
                }
            }

            movingTo = movingTo + movementDirection;
            //movementDirection should always be either 1 or -1
            //We add direction to the index to move us to the
            //next point in the sequence of points in our path

         
            //For Looping path you must move the index when you reach 
            //the begining or end of the PathSequence to loop the path
            if (PathType == PathTypes.loop)
            {               
                //If you just moved past the last point(moving forward)
                if (movingTo >= pathSequenceListCapacity)
                {
                    //Set the next point to move to as the first point in sequence
                    movingTo = 0;
                }
                //If you just moved past the first point(moving backwards)
                if (movingTo < 0)
                {
                    //Set the next point to move to as the last point in sequence
                    movingTo = pathSequenceListCapacity - 1;
                }
            }

       
        }
    }

    //Generate a point at player point of contact 
    private List<Transform> GeneratePath(List<Transform> PathSequence, Transform A, Transform B, Transform C)
    {
        #region variables for Generate Path
        //Path destination will be either A(Start Point) or B(End Point) C(Contact Point) is player location
        Vector3 pathDestination = C.position;//new Vector3(0,0,0);

        //Referencing the GO's in the field
        GameObject go;

        A = pointA;

        B = pointB;

        C = pointC;

        movementPathNode = GetComponentsInChildren<MovementPathNode>();

        if (facingTowardsA)
        {
            movementDirection = -1;
            facingTowardsB = false;
            pathDestination = A.transform.position;
        }
        if (facingTowardsB)
        {
            movementDirection = 1;
            facingTowardsA = false;
            pathDestination = B.transform.position;
        }
        #endregion
   
        float spawnZ = 1f;

        float maxTravelDistance = 1f;

        float distanceSquared = Vector3.Distance(C.position, pathDestination);

    

        var localDirection = C.transform.InverseTransformPoint(C.forward * spawnZ - pathDestination); //- pathDestination
        //If you are not close enough to each other
        if (distanceSquared >= Mathf.Abs(1))
        {
            // pointC.transform.localPosition = pointCPosition;
          
            for (int i = 0; i < distanceSquared; i++)
            { //          movementPathSpawner.SpawnNode("RailPointPrefab", new Vector3(C.transform.localPosition.x + localDirection.x, C.transform.localPosition.y + localDirection.y, C.transform.localPosition.z + localDirection.z) / 2);

                Vector3 position = Vector3.Lerp(C.position, pathDestination, i * .20f);

                movementPathSpawner.SpawnNode("RailPointPrefab", position);           
            }
          //  movementPathNode[0].OnObjectSpawn();
            //movementPathSpawner.SpawnNode("RailPointPrefab", localDirection - pathDestination);/*(C.position + Vector3.forward * spawnZ) - pathDestination);*/ //(Vector3.forward * zDistance));

            foreach (Transform node in PathSequence)
            {
                ///Basically if you're node C, B, or A. Fuck off. 
                ///For this to function properly, step on rail first, then toggle, facing bools

                if (node.gameObject.tag != "Point C")
                {
                    if (PathSequence[movingTo].transform.tag == "Point B")
                    {
                        if (distanceSquared >= Mathf.Abs(1))
                            movingTo = movingTo + 1;
                    }

                    if (node.gameObject.tag != "Point B")
                    {
                        if (node.gameObject.tag != "Point A")
                        {
                            go = node.gameObject;
                            if (go.activeSelf)
                            {
                                if (!ActiveNodes.Contains(go))
                                {
                                    ActiveNodes.Add(go);
                                }

                              //  movementPathSpawner.SpawnNode("RailPointPrefab", this.transform.position);// + localDirection); //localDirection - PathSequence[movingTo].position
                            }
                            if (Mathf.Abs(go.transform.position.z - C.position.z) <= Mathf.Abs(.15f))
                            {
                                Debug.Log("Go distance from C Position: " + (go.transform.position.z - C.position.z));
                                go.SetActive(false);
                                PathSequence.Remove(go.transform);
                            }
                            else if (C.position.z - safeDistanceBuffer > (spawnZ - nodeAmount * maxTravelDistance))
                            {
                                ActiveNodes.RemoveAt(0);
                                go.SetActive(false);
                                                          
                                return PathSequence;
                            }
                        }
                    }          
                }

            }
       
            return PathSequence;
        } 
        return PathSequence;   
    }

    public List<Transform> ResetPathSequence(List<Transform> PathSequence, Transform A, Transform B, Transform C)
    {
            PathSequence.Clear();

            foreach (GameObject node in ActiveNodes)
            {
                node.SetActive(false);
                node.transform.position = Vector3.zero;
            }

            ActiveNodes.Clear();
         
        canSpawn = false;
        facingTowardsA = false;
        facingTowardsB = false;

            return PathSequence = null;
    }
    #endregion //Coroutines
}
