using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGrindScript : MonoBehaviour {

    // Use this for initialization
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
    // The offset position is determined by xOffset, yOffset and zOffset
    // It is relative to the player.
    private Vector3 offsetPosition;

    [SerializeField]
    float m_Speed = 20f;
    float m_MinSpeed;
    float m_MaxSpeed;

    //Slow down variable 
    [SerializeField]
    float brakeVariable = 2f;

    [SerializeField]
    float m_TurnSpeed = 180f;

    //Audio for moving rail
    [SerializeField]
    AudioSource hopOnRailSound;

    [SerializeField]
    AudioClip slowRailGrind;

    [SerializeField]
    AudioClip railGrindingSound;

    public MeshCollider railCollider;

    public GameObject player;

    [SerializeField]
    GameObject railCar;


    /// <summary>
    /// Need these two to control the railCars Appearance
    /// </summary>
    public bool isGameObjectActive;


    [SerializeField]
    private GameObject confirmGrindThingy;

    PlayerController playerController;

    ThirdPersonPlayerCharacter thirdPersonPlayerCharacter;

    Rigidbody playerRigidBody;

    public bool IsTouching(Collider otherCollider) { if (otherCollider == railCar.gameObject) return true; else return false; }  

    Cinemachine.CinemachineDollyCart cinemachineCart;

    ControlVelocity controlVelocity;
    //RailCar's Rigidbody
    Rigidbody m_Rigidbody;
    
    public bool isGrinded;

    //Bool to check if balanced
    public bool isBalanced;
    /// <summary>
    /// Toggle to invert the Y axis when grinding rails(Tilts on the X axis) 
    /// </summary>
    private bool invertY;

    /// <summary>
    /// Toggle to invert the X axis when grinding rails(Tilts on the Z axis)
    /// </summary>
    private bool invertX;

    /// <summary>
    /// Amplifies input of players input to add rotation value
    /// </summary>
    public float rotationalPlayerInputBuffer = 30;

    public float grindTimer = .5f;
    public float timer = 0f;
    Vector3 eulerAngleVelocity;
    Vector3 currentPosition;
    Vector3 newPosition;


    #region AutoRotation variables 
    //AutoRotation variables 
    public float autoRotationSpeed = 10F;
    private float startTime;
    private float journeyLength;
    private float recoverTime = 5f;
    #endregion

    #region debug tools
    public bool isFacingPointA;
    public bool isFacingPointB;

    public bool playerIsOnRail;
    #endregion

    void OnAwake()
    {
        railCar.SetActive(false);
        cinemachineCart.enabled = false;
    }

    void Start ()
    {
       // isGrinded = Input.GetButton("Jump"/* + m_PlayerNumber*/); //Prolly shouldn't do this...
        railCollider = this.gameObject.GetComponent<MeshCollider>();

        cinemachineCart = railCar.gameObject.GetComponent<Cinemachine.CinemachineDollyCart>();
        controlVelocity = railCar.gameObject.GetComponent<ControlVelocity>();

        player = null;

        railCar.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        isGameObjectActive = confirmGrindThingy.activeSelf;

        if(player.transform.forward == railCollider.transform.forward)
        {
            isFacingPointB = false;
            isFacingPointA = true;
        }
        else if(player.transform.forward != railCollider.transform.forward)
        {
            isFacingPointA = false;
            isFacingPointB = true;
        }
        if (isGrinded)
        {
            if(thirdPersonPlayerCharacter == null)
            {
                thirdPersonPlayerCharacter = player.GetComponent<ThirdPersonPlayerCharacter>();
                thirdPersonPlayerCharacter.onRail = true;
            }
            if (!IsTouching(railCar.gameObject.GetComponent<Collider>())) { Debug.Log("NOT TOUCHING!");  playerIsOnRail = false; }
            player.transform.position = railCar.transform.position;
           // if(Input.GetButton("Jump"/* + m_PlayerNumber*/)){ isGrinded = false; }
        }

        
        if(!playerIsOnRail)
        {
            grindTimer -= Time.deltaTime;   
            if(grindTimer <= 0)
            {
                controlVelocity.isGrinding = false;
                thirdPersonPlayerCharacter.onRail = false;
                cinemachineCart.enabled = false;
                confirmGrindThingy.SetActive(false);
                player = null;
            }           
        }
        isGrinded = railCar.activeSelf;
    }

    private void OnCollisionExit(Collision collision)
    {
       // Debug.Log("Exit Collision");
        if (collision.gameObject.tag == "Player")
        {        
            //  Debug.Log("Exit Collision is player, now make false");
            playerIsOnRail = false;   
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if(collision.gameObject.tag == "Player")
        {
            grindTimer = .5f;
            player = collision.gameObject;
           
            playerRigidBody = player.GetComponent<Rigidbody>();
            thirdPersonPlayerCharacter = player.GetComponent<ThirdPersonPlayerCharacter>();
            playerController = player.GetComponent<PlayerController>();
            playerController.railCar = railCar;
            confirmGrindThingy.SetActive(true);
            railCar.SetActive(true);

            playerIsOnRail = true;
            controlVelocity.isGrinding = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {    
        controlVelocity.isGrinding = true;
        playerIsOnRail = true;
    }
    
  
    protected virtual void FixedUpdate()
    {
       Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
       m_Rigidbody = railCar.GetComponent<Rigidbody>();

        if (railCar.gameObject.activeSelf)
        {         
            cinemachineCart.enabled = true;
          //  isGrinded = true;
            CenterOnRail();
           // AutoMovement();
            ReadPlayerInput();
            // railCar.transform.forward = FaceCorrectWay();
        }
        else
            cinemachineCart.enabled = false;          
    }

    private void ReadPlayerInput()
    {
        
       
    }

    private void AutoMovement()
    {
        m_MinSpeed = 2f;
        m_MaxSpeed = 20f;

        //   Vector3 railDirection = player.transform.forward;

      //  cinemachineCart.m_Speed = m_MinSpeed;

        //Make player go the same place as the railCar
       player.transform.position = railCar.transform.position;
        //m_Speed -= transform.forward.y * 2.0f * Time.deltaTime * 10f;
        //if (m_Speed < m_MinSpeed)
        //{
        //    m_Speed = m_MinSpeed;

        //}
        //else if (m_Speed > m_MaxSpeed)
        //{
        //    m_Speed = m_MaxSpeed;
        //}
    }

    private void AutoRotate()
    {

       // currentPosition = railCar.transform.position;

       //// newPosition = railCollider.transform.position;
       // Quaternion currentRotation = m_Rigidbody.rotation;
       //// Quaternion balancedRotation = railCollider.transform.rotation;


       // journeyLength = Vector3.Distance(currentPosition, newPosition);
       // float distCovered = (Time.time - startTime) * autoRotationSpeed;
       // float fracJourney = distCovered / journeyLength;
       // startTime = Time.time;

        //Quaternion autoRotation = Quaternion.Lerp(currentRotation, balancedRotation, fracJourney);

        //if (currentRotation != balancedRotation)
        //{
        //    isBalanced = false;
        //}
        //else isBalanced = true;
    }

    private Vector3 FaceCorrectWay()
    {
        Vector3 prevLocation = railCar.transform.position;
        Vector3 Difference;
        
        Difference = railCar.transform.position - prevLocation;
     
        if (Difference.x < 0)
        {
            Debug.Log("Moved left");
        }
        if (Difference.x > 0)
        {
            Debug.Log("Moved right");
        }
        if (Difference.y > 0)
        {
            Debug.Log("Moved up");
        }
        if (Difference.y < 0)
        {
            Debug.Log("Moved down");
        }
        if (Difference.z > 0)
        {
            Debug.Log("Moved forward");
        }
        if (Difference.z < 0)
        {
            Debug.Log("Moved back");
        }
        prevLocation = railCar.transform.position;
       
        return -Difference;
    }
    private void CenterOnRail()
    {
        Vector3 railContactPoint = railCollider.ClosestPoint(player.transform.position);

       // railCar.transform.localRotation = SetRailCarPath(0, 0, 0); //SetRailCarPath(railCar.transform.localRotation.x, player.transform.eulerAngles.y, FaceCorrectWay().z);

        player.transform.position = railCar.transform.position + offsetPosition;

        playerRigidBody.constraints = RigidbodyConstraints.FreezePositionX;
        playerRigidBody.constraints = RigidbodyConstraints.FreezePositionY;
        playerRigidBody.constraints = RigidbodyConstraints.FreezePositionZ;

        playerRigidBody.constraints = RigidbodyConstraints.FreezeRotationY;

        player.transform.rotation = railCar.transform.rotation;
        // //float centerDot = Vector3.Dot(player.transform.forward, (startPointReference.transform.position - player.transform.forward).normalized);
        // //if (centerDot < 0f) { /* Debug.Log("Dot facing toward A: " + centerDot);*/ movePath.facingTowardsB = true; }
        // //else if (centerDot > 0f) { /*Debug.Log("Dot facing toward B: " + centerDot);*/ movePath.facingTowardsA = true; }
    }

    //Allows player to control railCar instead of player
    public Quaternion SetRailCarPath(float playerYInput, float railBalancerYAxis, float playerXInput)
    {
        //Assign player y input to the railCar X rotational axis 
        playerYInput = Input.GetAxis("leftJoystickVertical");
        if (invertY) { playerYInput = -Input.GetAxis("leftJoystickVertical"); }

        //Assign player x input to the railCar Z rotational axis 
        playerXInput = Input.GetAxis("leftJoystickHorizontal");
        if (invertX) { playerXInput = -Input.GetAxis("leftJoystickHorizontal"); }

        Quaternion rotationalValue = Quaternion.Euler(playerYInput * rotationalPlayerInputBuffer, cinemachineCart.transform.rotation.y, playerXInput * rotationalPlayerInputBuffer);

        //Debug.Log("CineCart Rotation Y: " + cinemachineCart.transform.rotation.y);
        float anyInput = playerYInput + playerXInput;
   
        if (anyInput < .25f)
        {
            AutoRotate();
        }

        return rotationalValue;
    }

    private void LateUpdate()
    {
        if (isGrinded)
        {
            railCar.transform.rotation = SetRailCarPath(railCar.transform.rotation.x, 0, railCar.transform.rotation.z);
        }
        if (!isBalanced)
        {
            AutoRotate();
        }         
    }
}
