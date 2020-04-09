using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySampleAssets.Cameras;
using UnityEngine.AI;
using System;


/// <summary>
/// This horrifyling refactored script from Unity Player Controller Demo Does the following:
///     Registers player button inputs (Movement, crouch, jump, sprint)
///     Has a character state machine that is used to talk and explore the game world
///     Acts as the center hub of all player related scripts, activating each. 
///     Stitched Together by DeAndre Christopher Owens
/// </summary>

[RequireComponent(typeof(ThirdPersonPlayerCharacter))]
[RequireComponent(typeof(Rook))]
[RequireComponent(typeof(CombatController))]
public class PlayerController : MonoBehaviour
{
    protected enum CharacterState
    {
        IdleState, RoamingState, CrouchState, JumpState, DoubleJumpState, SprintState, DialogueState
    };

    protected enum TreckingState { Idle, Swimming, Crawling, Walking, Sprinting, Leaping, Grinding, WallRunning, WallSliding, Climbing, Sliding, Tumbling, Swinging, Bouncing }
    /// <summary>
    ///New State Machine to Replace current State Machines
    /// </summary>
    [SerializeField]
    public StateMachine stateMachine = new StateMachine();

    /// <summary>
    ///Layer in which items will be on
    /// </summary>
    [SerializeField]
    private LayerMask itemsLayer;

    /// <summary>
    ///Layer for the ground
    /// </summary>
    [SerializeField]
    private LayerMask groundLayer;

    /// <summary>
    ///Layer for the runnableWalls
    /// </summary>
    [SerializeField]
    private LayerMask runnableWallsLayer;

    /// <summary>
    /// Layer for the rails
    /// </summary>
    [SerializeField]
    private LayerMask railLayer;

    /// <summary>
    /// Layer for the water
    /// </summary>
    [SerializeField]
    private LayerMask waterLayer;

    /// <summary>
    /// Range in which player can view items
    /// </summary>
    [SerializeField]
    private float viewRange;

    /// <summary>
    /// Tag for items
    /// </summary>
    [SerializeField]
    private string itemsTag;

    /// <summary>
    /// Tag for ground
    /// </summary>
    [SerializeField]
    private string groundTag;

    /// <summary>
    /// Range in which player can touch Ground
    /// </summary>
    [SerializeField]
    private float groundRange;

    /// <summary>
    /// A reference to the nav Mesh Agent on a gameObject that is not the player
    /// </summary>
    private NavMeshAgent navMeshAgent;

    ///// <summary>
    ///// A reference to the control Velocity script on the railCar gameObject
    ///// </summary>
    private ControlVelocity controlVelocity;

    ///// <summary>
    ///// A reference to the grindable object script on the railCar gameObject
    ///// </summary>
    //private GrindableObject grindableObject;
    ///// <summary>
    ///// A reference to the railCar GameObject
    ///// </summary>
    public GameObject railCar;

    public ControlVelocity railCarControlVelocity;

    #region State Machines
    public CombatState playerControllerCombatState;
    /// <summary>
    /// Player
    /// </summary>
    //public TreckingState playerControllerTreckingState;



    #region Character State Variables
    /// <summary>
    /// Current state of Character State State Machine
    /// </summary>
    [SerializeField]
    private CharacterState currentCharacterState;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    private CharacterState characterState;
    /// <summary>
    /// Previous state of character state machine
    /// </summary>
    private CharacterState previousCharacterState;
    /// <summary>
    /// Readable CharacterState 
    /// </summary>
    protected CharacterState CurrentCharacterState
    {
        get { return currentCharacterState; }
        set
        {
            previousCharacterState = currentCharacterState;
            currentCharacterState = value;
            StateChange(value);
        }
    }
    /// <summary>
    /// State Machine that identifies and alters the players character State
    /// </summary>
    protected CharacterState StateChange(CharacterState characterState)
    {
        switch (CurrentCharacterState)
        {
            case CharacterState.IdleState:
                //controller.enabled = true;          
                return previousCharacterState = CharacterState.IdleState;
            case CharacterState.CrouchState:
                //controller.enabled = true;
                return previousCharacterState = CharacterState.CrouchState;

            case CharacterState.JumpState:
                // controller.enabled = true;
                return previousCharacterState = CharacterState.JumpState;

            case CharacterState.DoubleJumpState:
                //controller.enabled = true;
                return previousCharacterState = CharacterState.DoubleJumpState;

            case CharacterState.SprintState:
                //controller.enabled = false;
                return previousCharacterState = CharacterState.SprintState;

            case CharacterState.DialogueState:
                //controller.enabled = false;
                return previousCharacterState = CharacterState.DialogueState;
        }
        return CurrentCharacterState;
    }
    #endregion

    #region Trecking State Variables
    [SerializeField]
    private TreckingState currentTreckingState;

    private TreckingState treckingState;
    private TreckingState previousState;

    protected TreckingState CurrentTreckingState
    {
        get { return currentTreckingState; }
        set
        {
            previousState = currentTreckingState;
            currentTreckingState = value;
            StateChange(value);
        }
    }

    protected TreckingState StateChange(TreckingState treckingState)
    {
        switch (CurrentTreckingState)
        {
            case TreckingState.Idle:
                //controller.enabled = true;             
                return previousState = TreckingState.Idle;
            case TreckingState.Swimming:
                //controller.enabled = true;
               // isTraversing = true;
                return previousState = TreckingState.Swimming;

            case TreckingState.Crawling:
                //controller.enabled = true;
                //isTraversing = true;
                return previousState = TreckingState.Crawling;

            case TreckingState.Walking:
                //controller.enabled = true;

                return previousState = TreckingState.Walking;

            case TreckingState.Sprinting:
                //controller.enabled = true;

                return previousState = TreckingState.Sprinting;

            case TreckingState.Leaping:
                //controller.enabled = true;
               // isTraversing = true;
                return previousState = TreckingState.Leaping;

            case TreckingState.Grinding:
                //controller.enabled = true;
                //isTraversing = true;
                
                return previousState = TreckingState.Grinding;

            case TreckingState.WallRunning:
                //controller.enabled = true;
                //isTraversing = true;
                return previousState = TreckingState.WallRunning;
            case TreckingState.WallSliding:
                //controller.enabled = true;
                //isTraversing = true;
                return previousState = TreckingState.WallSliding;
            case TreckingState.Climbing:
                //controller.enabled = true;
               // isTraversing = true;
                return previousState = TreckingState.Climbing;

            case TreckingState.Sliding:
                //controller.enabled = true;
               // isTraversing = true;
                return previousState = TreckingState.Sliding;

            case TreckingState.Tumbling:
                //controller.enabled = true;
               // isTraversing = true;
                return previousState = TreckingState.Tumbling;

            case TreckingState.Swinging:
                //controller.enabled = true;
               // isTraversing = true;
                return previousState = TreckingState.Swinging;

            case TreckingState.Bouncing:
                //controller.enabled = true;
               // isTraversing = true;
                return previousState = TreckingState.Bouncing;
        }
        return CurrentTreckingState;
    }
    #endregion
    #endregion
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    #region Scripts Needed for Player
    /// <summary>
    /// A reference to the player health script on this gameobject
    /// </summary>
    [HideInInspector]
    public PlayerHealth _Health;
    /// <summary>
    /// /// A reference to the audio handler script on this gameobject
    /// </summary>
    [HideInInspector]
    public AudioHandler _Audio;
    /// <summary>
    /// A reference to the Shooting System script on this gameobject
    /// </summary>
    [HideInInspector]
    public ShootingSystem _Shooting;
    /// <summary>
    /// A reference to the rotation system script on this gameobject
    /// </summary>
    [HideInInspector]
    public RotationSystem _Rotation;
    /// <summary>
    /// A reference to the raycast handler script on this gameobject
    /// </summary>
    [HideInInspector]
    public RaycastHandler _Raycast;

    /// <summary>
    /// A reference to the ThirdPersonCharacter script on this gameobject
    /// </summary>
    ThirdPersonPlayerCharacter thirdPersonPlayerCharacter;

    /// <summary>
    /// A reference to the Rook on this gameobject
    /// </summary>
    Rook rook;
    /// <summary>
    /// A reference to the followPath Script on the railCar gameobject
    /// </summary>
    private FollowPath followPath;


    /// <summary>
    /// A reference to the playerCombatControlleron this gameobject
    /// </summary>
    private CombatController combatController;

    /// <summary>
    /// A reference to the Nav Mesh Agent script on this gameobject
    /// </summary>
    NavMeshAgent playerNavMesh;

    /// <summary>
    /// A reference to the inventory manager script on this gameobject
    /// </summary>
    public InventoryManager inventoryManager;
    /// <summary>
    /// A reference to the player menu manager script on this gameobject
    /// </summary>
    public MenuManager menuManager;


    #endregion
    /// <summary>
    /// Toggles whether player canMove and can input values
    /// </summary>
    [HideInInspector]
    public bool canMove = true;
    #region Player Input Variables
    //Gamepad Stuff
    public Gamepad myGamepad;
    [Header("What Player Is This?")]
    public int playerNumber = 1;
    //Keyboard Input
    [Header("Assign Keyboard Keys")]
    public KeyCode up = KeyCode.W;
    public KeyCode left = KeyCode.A;
    public KeyCode down = KeyCode.S;
    public KeyCode right = KeyCode.D;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //direction vectors for movement
    [HideInInspector]
    public Vector2 direction = new Vector2(0, 0);
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    [HideInInspector]
    public Vector2 keyDirection = new Vector2(0, 0);
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Tests for input
    public bool MovementInput
    {
        get
        {
            //Debug.Log(direction.sqrMagnitude);
            if (direction.magnitude == 0) return false;
            return true;
        }
    }
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Dead zone where inputs will not register 
    public float movementDeadZone; //Ideally want dead zone at 0.05f
                                   /// <summary>
                                   /// Minimal speed GameObject can travel on rail
                                   /// </summary>
    //Variable for horizontal Input
    private float h;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Variable for vertical Input
    private float v;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    private float x;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    private float z;
    //Bools 
    [Header("Gamepad?")]
    public bool gamepadInput = true;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public bool walkByDefault = false; // toggle for walking state

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public bool sprintByDefault = false; //NEW feature, toggle sprint feature
                                         /// <summary>
                                         /// Minimal speed GameObject can travel on rail
                                         /// </summary>
    public bool lookInCameraDirection = true;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    bool isIdle;
    // should the character be looking in the same direction that the camera is facing        
    // public int sprintSpeed; //sprint speed.
    // The position that the character should be looking towards
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    Transform cam; // A reference to the main camera in the scenes transform
                   /// <summary>
                   /// Minimal speed GameObject can travel on rail
                   /// </summary>
    Vector3 camForward; // The current forward direction of the camera
                        /// <summary>
                        /// Minimal speed GameObject can travel on rail
                        /// </summary>
    Vector3 camLeftRight;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    Vector3 move;
    #region Targeting and battleCamera toggle //Not getting used...I don't think.
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    Vector3 mainTargetPosition;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    Vector3 subTargetPosition;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    Vector3 lookPos;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    Vector3 lookAtMainTarget;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    GameObject battleCamera;
    #endregion

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    FreeLookCam freeLookcam;
    //[Header("Maneuver Variables")]
    //bool isMoving;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Determines whether player can Jump based on jump input
    public bool canJump;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Determines whether player can Double Jump based on jump input
    public bool canDoubleJump;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Timer before doubleJump can activate
    public float doubleJumpTimer;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Variable to reset doubleJumpTimer to
    public float doubleJumpTimeCounterReset;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Tells how many times player can jump
    public int numberOfDoubleJUmps;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Determines how long double Jump stays active
    public float timeDoubleJumpStateCanStayActive;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Determines max length double jump is active
    public float maxTimeDoubleJumpIsActive;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Determines the max number of double jumps player can use
    public int maxNumberOfDoubleJumps;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public bool canAirDash;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public bool canAirStomp;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public bool anyInput;
    //bool isJumping = false; 
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public bool isDoubleJumping = false;
    //bool doublejumped = false;
    //bool isFalling;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public bool canEvade = true;
    //float evadeTimer = 1;

    ////rolling variables
    //public float rollSpeed = 8;
    //bool isRolling = false;
    //public float rollduration;

    ////Weapon and Shield
    //private Weapon weapon;
    //int rightWeapon = 0;
    //int leftWeapon = 0;
    //bool isRelax = false;

    ////isStrafing/action variables
    //bool canAction = true;
    //bool isStrafing = false;
    ////bool isDead = false;
    //bool isBlocking = false;
    //public float knockbackMultiplier = 1f;
    //bool isKnockback;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    [Header("Menu Variables")]
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    bool pauseMenu = false;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    bool mapMenu = false;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    bool inventoryMenu = false;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    bool checkInventoryStance;

    //Variables to Toggle Sprint
    #region Sprint Toggle Variables
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Registers how many horizontal presses were pressed 
    public int horizontalButtonPressTotal;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Registers how many vertical presses were pressed 
    public int verticalButtonPressTotal;

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    //Activates sprint input cooler
    public float sprintInputCooler = .5f;

    /// <summary>
    /// Input Count for sprint
    /// </summary>
    public int sprintInputCount = 2;

    /// <summary>
    /// First Horizonal sprint input to the left
    /// </summary>
    public bool firstLeftInput;

    /// <summary>
    /// First Horizonal sprint input to the right
    /// </summary>
    public bool firstRightInput;

    /// <summary>
    ///First Vertical sprint input
    /// </summary>
    public bool firstForwardInput;

    /// <summary>
    /// First Vertical sprint input
    /// </summary>
    public bool firstBackInput;

    /// <summary>
    /// Second sprint input is the no directional input
    /// </summary>
    public bool movementInputReset;
    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public bool thirdSprintInput;
    #endregion
    #endregion
    void Awake()
    {
        _Rotation = this.GetComponent<RotationSystem>();
        _Raycast = this.GetComponent<RaycastHandler>();
        _Shooting = this.GetComponent<ShootingSystem>();
        _Health = this.GetComponent<PlayerHealth>();
        _Audio = this.GetComponent<AudioHandler>();
        
        inventoryManager = this.GetComponent<InventoryManager>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

    }

    void Start()
    {
        this.navMeshAgent = this.GetComponent<NavMeshAgent>();
        this.stateMachine.ChangeState(new Idle(this.groundLayer, this.gameObject, this.viewRange, this.groundTag, this.navMeshAgent, this.AirFound));

        playerControllerCombatState = CombatState.IdleCombatState;
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        }
        thirdPersonPlayerCharacter = GetComponent<ThirdPersonPlayerCharacter>();
        combatController = GetComponent<CombatController>();
        rook = GetComponent<Rook>();
        followPath = GetComponent<FollowPath>();
        //Initialize PopulateTargetlist component and finding children objects 
        //populateTargetList = cam.GetComponentInChildren<PopulateTargetList>();        

        //    if (gamepadInput) //this will basically turn it off if there's no gamepad manager -- so nothing breaks
        //    { gamepadInput = GamepadManager.Instance != null; }

        //    if (gamepadInput)
        //    {
        //        myGamepad = GamepadManager.Instance.GetGamepad(playerNumber);
        //    }
    }
    void Update()
    {
        // if (myGamepad != null) { gamepadInput = myGamepad.IsConnected; }
        // if (canMove) { GetInputsAndDirection(); }
        this.stateMachine.ExecuteStaticUpdate();
        //CheckIfIdle();

        //Check if third person player character is not on ground
        if (!thirdPersonPlayerCharacter.onGround)
        {

            if (thirdPersonPlayerCharacter.onRail)
            {
                this.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                CurrentCharacterState = CharacterState.RoamingState;
                //currentTreckingState = TreckingState.Grinding;
                StateChange(TreckingState.Grinding);

              //  StateChange(CurrentCharacterState);
                isIdle = false;
            }
            if (thirdPersonPlayerCharacter.onWall)
            {
                this.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                CurrentCharacterState = CharacterState.RoamingState;
                //StateChange(CurrentCharacterState);

                //thirdPersonPlayerCharacter.StateChange();
                isIdle = false;
            }
            CurrentCharacterState = CharacterState.JumpState;
            StateChange(CurrentCharacterState);
            isIdle = false;
            TriggerJumpState();
        }
        if (thirdPersonPlayerCharacter.onGround)
        {
            this.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            CheckIfIdle();
            //CurrentCharacterState = CharacterState.IdleState;
            //StateChange(CurrentCharacterState);

            timeDoubleJumpStateCanStayActive = maxTimeDoubleJumpIsActive;
            numberOfDoubleJUmps = 0;
            isDoubleJumping = false;
            doubleJumpTimer = doubleJumpTimeCounterReset;
        }
        //#region InventoryToggle
        //if (Input.GetButton("leftJoystickButton"))
        //{
        //    CheckingInventoryState();
        //    CurrentCharacterState = CharacterState.CheckingInventoryState;
        //}
        //#endregion
        if (!canJump && CurrentCharacterState != CharacterState.JumpState || CurrentCharacterState != CharacterState.DoubleJumpState)
        {       
            canJump = (Input.GetButton("Jump"));

            if (canJump)
            {
                this.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                CurrentCharacterState = CharacterState.JumpState;
                StateChange(CurrentCharacterState);
            }
        }
        //Check if Player is in jump state and has input the jump button
        if (CurrentCharacterState == CharacterState.JumpState && canJump)
        {           
            //If Player jumps while not on ground and the doubleJumpTimer is beneath the threshold 
            if (!thirdPersonPlayerCharacter.onGround && doubleJumpTimer <= 0f)
            {
                //Set canDoubleJump to canJump input button(Jump)
                canDoubleJump = Input.GetButtonDown("Jump"); // canJump;

                if (canDoubleJump)
                {
                    //Set to true to show player is double jumping
                    isDoubleJumping = true;
                  
                    timeDoubleJumpStateCanStayActive = maxTimeDoubleJumpIsActive;
                    //Take Away a jump from number of Jumps
                    AddNumberofTimesJumped();
                }

                if (isDoubleJumping)
                {
                    CurrentCharacterState = CharacterState.DoubleJumpState;
                    StateChange(CurrentCharacterState);
                }
            }
        }
        
        //Check if player is in jump or doubleJump state to begin countdown timer to reading player second jump input 
        if (CurrentCharacterState == CharacterState.JumpState || CurrentCharacterState == CharacterState.DoubleJumpState)
        {
            //Lowers timer to specific threshold to allow player to use double jump function
            doubleJumpTimer -= Time.deltaTime;
            //doubleJumpTimer reaches past a certain threshold prevent jump/Double JumpInputs
            if (numberOfDoubleJUmps >= maxNumberOfDoubleJumps)
            {
                canJump = false;
                canDoubleJump = false;
                CurrentCharacterState = CharacterState.JumpState;
                StateChange(CurrentCharacterState);
            }
            if(CurrentCharacterState == CharacterState.DoubleJumpState)
            {


                timeDoubleJumpStateCanStayActive -= Time.deltaTime;
                if(timeDoubleJumpStateCanStayActive <= 0)
                {
                    CurrentCharacterState = CharacterState.IdleState;
                }
            }
        }
        lookInCameraDirection = (Input.GetAxisRaw("leftTrigger") != 0
            || (Input.GetAxisRaw("rightTrigger") != 0));

        #region Character State Machine in Update
        switch (CurrentCharacterState)
        {
            case CharacterState.IdleState:
                //controller.enabled = true;          
                break;
            case CharacterState.CrouchState:
                //controller.enabled = true;
                break;

            case CharacterState.JumpState:
                // controller.enabled = true;
                break;

            case CharacterState.DoubleJumpState:
                //controller.enabled = true;
                break;

            case CharacterState.SprintState:
                //controller.enabled = false;
                break;

            case CharacterState.DialogueState:
                //controller.enabled = false;
                break;
       
        }     
        #endregion
    }

    private void AddNumberofTimesJumped()
    {
        numberOfDoubleJUmps += 1;
    }

    private void CheckIfIdle()
    {
        h = Input.GetAxis("leftJoystickHorizontal");
        v = Input.GetAxis("leftJoystickVertical");

        if (!thirdPersonPlayerCharacter.isTraversing)
        {
            if (Mathf.Abs(h) <= Mathf.Abs(movementDeadZone))
            {
                isIdle = true;
            }
            if (Mathf.Abs(v) <= Mathf.Abs(movementDeadZone))
            {
                isIdle = true;
            }
        }
        else
            isIdle = false;
    }
    private void GetInputsAndDirection()
    {
        //Resets the directions each frame
        ResetDirection();
        //Sets direction based on input
        RightAndLeft();
        UpAndDown();
        //sets the direction based on the keyDirection and normalizes it
        direction += keyDirection;
    }
    private void ResetDirection()
    {
        //Sets them both back to 0
        keyDirection = new Vector2(0, 0);
        direction = new Vector2(0, 0);
    }
    public virtual void RightAndLeft()
    {
        #region Keyboard Input
        if (KeyboardInput.IsHoldingKey(right))
        {
            keyDirection.x += 1;
        }
        if (KeyboardInput.IsHoldingKey(left))
        {
            keyDirection.x += -1;
        }
        #endregion
        //#region Gamepad Input
        //if (myGamepad != null && gamepadInput)
        //{
        //    if (myGamepad.GetStick_L().X < 0) //GAMEPAD LEFT
        //    {
        //        keyDirection.x += -1;
        //    }
        //    if (myGamepad.GetStick_L().X > 0) //GAMEPAD RIGHT
        //    {
        //        keyDirection.x += 1;
        //    }
        //}
        //#endregion
    }
    public virtual void UpAndDown()
    {
        #region Keyboard Input
        if (KeyboardInput.IsHoldingKey(up))
        {
            keyDirection.y += 1;
        }
        if (KeyboardInput.IsHoldingKey(down))
        {
            keyDirection.y = -1;
        }
        #endregion
        #region Gamepad Input
        if (myGamepad != null && gamepadInput)
        {
            if (myGamepad.GetStick_L().Y < 0) //GAMEPAD DOWN
            {
                keyDirection.y += -1;
            }
            if (myGamepad.GetStick_L().Y > 0) //GAMEPAD UP
            {
                keyDirection.y += 1;
            }
        }
        #endregion
    }
    //protected CharacterState CheckingInventoryState()
    //{  
    //    if (Input.GetButton("leftJoystickButton"))
    //    { 
    //        return CurrentCharacterState = CharacterState.IdleState;
    //    }
    //    return CurrentCharacterState = CharacterState.CheckingInventoryState;
    //}
    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        #region player controller bools
        
        bool crouch = false;
        bool walk = false;
        bool sprint = false;
        bool attack = false;
        bool stomp = false;

        bool hurt = false;
        bool dead = false;
        bool canMoveCamera = false;

        Vector3 lastPlayerInputPosition;

        h = Input.GetAxis("leftJoystickHorizontal");
        v = Input.GetAxis("leftJoystickVertical");


        crouch = (Input.GetAxisRaw("leftTrigger") != 0);

        bool walkToggle = CheckForWalk(walk); ; //Input.GetKey(KeyCode.LeftShift);
        bool sprintToggle = CheckForSprint(sprint);
        bool targetToggle = (Input.GetAxisRaw("rightTrigger") != 0f);

        #endregion


        #region Trecking State Machine In Update
        switch (CurrentTreckingState)
        {
            case TreckingState.Idle:

                break;
            case TreckingState.Swimming:
                //controller.enabled = true;
                //isTraversing = true;
                break;

            case TreckingState.Crawling:
                sprint = false;
                break;

            case TreckingState.Walking:
                //controller.enabled = true;
                sprint = false;
                break;

            case TreckingState.Sprinting:
                //controller.enabled = true;
                sprint = true;
                break;

            case TreckingState.Leaping:
                //  isJumping = true;
                //  isTraversing = true;
                break;

            case TreckingState.Grinding:
                // onRail = true;
                //isTraversing = true;
                break;

            case TreckingState.WallRunning:
                //   onWall = true;
                //    isTraversing = true;
                break;
            case TreckingState.WallSliding:
                //controller.enabled = true;
                //   isTraversing = true;
                break;
            case TreckingState.Climbing:
                //controller.enabled = true;
                //   isTraversing = true;
                break;

            case TreckingState.Sliding:
                //controller.enabled = true;
                //   isTraversing = true;
                break;

            case TreckingState.Tumbling:
                //controller.enabled = true;
                //    isTraversing = true;
                break;

            case TreckingState.Swinging:
                //isSwinging = true;
                //   isTraversing = true;
                break;

            case TreckingState.Bouncing:
                //isBouncing = true;
                //   isTraversing = true;
                break;
        }
        #endregion
        //During Technique Stance, face target to charge weapon or prepare parry
        if (playerControllerCombatState == CombatState.TechniqueStanceState || playerControllerCombatState == CombatState.DefensiveStanceState)
        {
            //Look in camera direction
            lookPos = lookInCameraDirection && cam != null
      ? transform.position + cam.forward * 100 : transform.position + transform.forward * 100;
            //Move body in camera direction
            move = v * camForward + h * cam.right;

        }
        if (!thirdPersonPlayerCharacter.onGround)
        {        
            canAirStomp = true;
            if (CurrentCharacterState == CharacterState.DoubleJumpState)
            {
                //If player movement input is less than the threshold activate HandleDoubleJump in thirdPersonPlayerCharacter Script
                canAirDash = true;
                if (h <= Mathf.Abs(.75f) && v <= Mathf.Abs(.75f))
                {
                    canAirDash = false;
                    canAirStomp = false;
                    thirdPersonPlayerCharacter.HandleDoubleJump();
                }
                //If player movement input is greater than the threshold activate HandleAirDash in thirdPersonPlayerCharacter Script
                if (h > Mathf.Abs(.75f) || v > Mathf.Abs(.75f))
                {
                    if(canAirDash == true)
                    { 
                        thirdPersonPlayerCharacter.HandleAirDash();
                    }
                }              
            }
            //If combatController Combat State is equal to AttackState 
            if (combatController.CurrentCombatState == CombatState.AttackState)
            {
                stomp = Input.GetButton("bButton");
                //If stomp is pressed activate HandleAirStomp in thirdPersonPlayerCharacter Script
                if (stomp && canAirStomp)
                {
                    canAirDash = false;
                    thirdPersonPlayerCharacter.HandleAirStomp();
                }
            }
        }
        if (!mapMenu)
        {
            mapMenu = Input.GetButtonUp("yButton");
            if (mapMenu)
            {
                ResearchState();
            }
        }
      
        if (isIdle)
        {
            TriggerIdleState();
            //currentCharacterState = CharacterState.IdleState;
            //StateChange(CurrentCharacterState);
            //menuManager.menuManagerCharacterState = (CharacterState)((int)CurrentCharacterState);
        }
        if (cam != null)
        {
            // calculate camera relative direction to move:
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;

            if (move.magnitude <= .5f)
            {
                move.Set(0, 0, 0);
            }

            move = v * camForward + h * cam.right; // camFoward; 
            lastPlayerInputPosition = move;
            //  lookPos = move;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            move = v * Vector3.forward + h * Vector3.right;
            //    lookPos = move;
        }
        if (move.magnitude > 1)
        move.Normalize();
       
#if !MOBILE_INPUT
        // On non-mobile builds, walk/run speed is modified by a key press.          
        //Testing Sprint feature         
        // We select appropriate speed based on whether we're walking by default, and whether the walk/run toggle button is pressed:
        float walkMultiplier = (walkByDefault ? walkToggle ? 1 : 0.5f : walkToggle ? 0.5f : 1);
        move *= walkMultiplier;

        float sprintMultiplier = (sprintByDefault ? sprintToggle ? 1 : 3f : sprintToggle ? 3f : 1);
        move *= sprintMultiplier;
        if (walkToggle)
        {
            isIdle = false;
            currentCharacterState = CharacterState.RoamingState;
            StateChange(CurrentCharacterState);
        }
        if (sprintToggle)
        {
            this.stateMachine.ChangeState(new Idle(this.groundLayer, this.gameObject, this.viewRange, this.groundTag, this.navMeshAgent, this.AirFound));
            this.stateMachine.ChangeState(new Sprinting(this.groundLayer, this.gameObject, this.thirdPersonPlayerCharacter, this.groundRange, this.groundTag, this.WallFound));
            isIdle = false;
            currentCharacterState = CharacterState.SprintState;
            StateChange(CurrentCharacterState);
            //if (thirdPersonPlayerCharacter != null)
            //    thirdPersonPlayerCharacter.thirdPersonPlayerCharacterState = (CharacterState)((int)CurrentCharacterState);
            if (rook != null)
                rook.LoseStamina(rook.StaminaPoints);
            Debug.Log("Stamina Points: " + rook.StaminaPoints);
        }
#endif
        ///You may want to delete RailGrindMovement and just use .Move
        ///The playerController needs one move input. 
        ///In 3rd Person character select which parts of methods get used.
        if(treckingState == TreckingState.Grinding)
        {
            //If rail car is not present even though player is in grind state
            //if(railCar == null)
            //{ 
            //    //Look for it. 
            //    followPath = GameObject.FindGameObjectWithTag("RailCar").GetComponent<FollowPath>();
            //}
            ////Assign followPath to followPath Script on rail car
            ////followPath = railBalancer.GetComponent<FollowPath>();

            ////Assign controlVelocity to controlVelocity Script on rail car
            railCarControlVelocity = railCar.GetComponent<ControlVelocity>();
            
            if(playerControllerCombatState == CombatState.TechniqueChargingState)
            {
                Debug.Log("You should be reading true.");
                railCarControlVelocity.isBoosting = true;
            }
          
            ////combatController.railBalancer = railBalancer;
            ////Call setRailCarPath method on railCar 
            ////Uses Horizontal input(Zrotation(h)) and Vertical input(Xrotation(v))
            ////Pass 0 for the Y rotational axis
            ////The railCar turns based on railBalancer Z 
            //RailCarScript railCarScript = railCar.GetComponent<RailCarScript>();

            //railCarScript.SetRailCarPath(v, 0, h);

            ////Call RailGrindInput method on railCar
            ////Uses crouch, canJump, and playerControllerCombatState for speed altering
            ////controlVelocity.RailGrindInput(crouch, canJump);

            //h = 0;
            //v = 0;

            CurrentCharacterState = CharacterState.RoamingState;
            StateChange(CurrentCharacterState);       
        }
        // calculate the head look target position
        //100  
        // pass all parameters to the character control script
      
            thirdPersonPlayerCharacter.Move(move, crouch, canJump, lookPos, canDoubleJump, sprint);
            //combatController.Fight()
            canJump = false;
            canDoubleJump = false;
   }
    //Check to see if character is moving around and not idle0
    private bool CheckForWalk(bool w)
    {
        if(CurrentCharacterState != CharacterState.RoamingState)
        {
            //Account for deadzone combo of (h).05f + (v).05f 
            if (h != 0)
            {
                if(!isIdle)
                return true;
            }
            if (v != 0)
            {
                if (!isIdle)
                    return true;
            }
            return false;
        }
        return false;
    }
    #region Sprint Check
    private bool CheckForSprint(bool s)
    {
        if(CurrentCharacterState == CharacterState.SprintState)
        {
            if (h + v <= Mathf.Abs(.30f))
            {              
                //thirdPersonPlayerCharacter.thirdPersonPlayerCharacterState = (CharacterState)((int)CurrentCharacterState);
                return false;
            }
            return true;
        }
        
        if (CurrentCharacterState != CharacterState.SprintState)
        {
            #region Check Left Joystick Inputs going Left      
            //Check if Horizontal input greater than the value of the threshold for activating sprintInputCount
            if ((Input.GetAxisRaw("leftJoystickHorizontal")) < -.75f)
            {               
                //If Vertical was pressed before Horizontal, cancel the firstHSprintInput 
                if (verticalButtonPressTotal != 0)
                    firstLeftInput = false;
                else
                {   //Add one point to horizontal button press
                    horizontalButtonPressTotal = 1;
                    firstLeftInput = true;
                    firstRightInput = false;
                    firstForwardInput = false;
                    firstBackInput = false;
                }                        
            }
            if (firstLeftInput)
            {
                sprintInputCooler += Time.deltaTime;
                if (firstRightInput)
                {
                    firstLeftInput = false;
                }                    
                if (Mathf.Abs(Input.GetAxisRaw("leftJoystickHorizontal")) < .5f)
                {                
                    movementInputReset = true;
                }
                if (sprintInputCooler <= .5f && movementInputReset)
                {
                    if ((Input.GetAxisRaw("leftJoystickHorizontal")) < -.75f)
                        thirdSprintInput = true;                 
                }
            }
            #endregion

            #region  Check Left Joystick Inputs going right  
            //Check if Horizontal input greater than the value of the threshold for activating sprintInputCount
            if ((Input.GetAxisRaw("leftJoystickHorizontal")) > .75f)
            {
                //If Vertical was pressed before Horizontal, cancel the firstHSprintInput 
                if (verticalButtonPressTotal != 0)
                    firstRightInput = false;
                else
                {   //Add one point to horizontal button press
                    horizontalButtonPressTotal = 1;
                    firstRightInput = true;
                    firstLeftInput = false;
                    firstForwardInput = false;
                    firstBackInput = false;
                }
            }
            if (firstRightInput)
            {
                sprintInputCooler += Time.deltaTime;
                if (firstLeftInput)
                {
                    firstRightInput = false;
                }
                if (Mathf.Abs(Input.GetAxisRaw("leftJoystickHorizontal")) < .5f)
                {
                    movementInputReset = true;
                }
                if (sprintInputCooler <= .5f && movementInputReset)
                {
                    if ((Input.GetAxisRaw("leftJoystickHorizontal")) > .75f)
                        thirdSprintInput = true;
                }
            }
            #endregion

            #region Check Vertical Sprint Input going forward
            if ((Input.GetAxisRaw("leftJoystickVertical")) > .75f)
            {
                //If Horizontal was pressed before Vertical, cancel the firstVSprintInput 
                if (horizontalButtonPressTotal != 0)
                    firstForwardInput = false;
                else
                {   //Add one point to vertical button press
                    verticalButtonPressTotal = 1;
                    firstForwardInput = true;
                    firstLeftInput = false;
                    firstRightInput = false;
                    firstBackInput = false;
                }
            }
            if (firstForwardInput)
            {
                sprintInputCooler += Time.deltaTime;
                //Horizontal was pressed before Vertical, cancel the firstVSprintInput 
                if (horizontalButtonPressTotal != 0)
                    firstForwardInput = false;
          
                if (firstBackInput)
                {
                    firstForwardInput = false;
                }
                if (Mathf.Abs(Input.GetAxisRaw("leftJoystickVertical")) < .5f)
                {
                    movementInputReset = true;
                }
                if (sprintInputCooler <= .5f && movementInputReset)
                {
                    if ((Input.GetAxisRaw("leftJoystickVertical")) > .75f)
                        thirdSprintInput = true;
                }
            }
            #endregion

            #region Check Vertical Sprint Input going Back
            if ((Input.GetAxisRaw("leftJoystickVertical")) < -.75f)
            {
                //If Horizontal was pressed before Vertical, cancel the firstVSprintInput 
                if (horizontalButtonPressTotal != 0)
                    firstBackInput = false;
                else
                {   //Add one point to vertical button press
                    verticalButtonPressTotal = 1;
                    firstBackInput = true;
                    firstLeftInput = false;
                    firstRightInput = false;
                    firstForwardInput = false;
                }
            }
            if (firstBackInput)
            {
                sprintInputCooler += Time.deltaTime;
              
                if (horizontalButtonPressTotal != 0)
                    firstForwardInput = false;

                if (firstForwardInput)
                {
                    firstBackInput = false;
                }
                if (Mathf.Abs(Input.GetAxisRaw("leftJoystickVertical")) < .5f)
                {
                    movementInputReset = true;
                }
                if (sprintInputCooler <= .5f && movementInputReset)
                {
                    if ((Input.GetAxisRaw("leftJoystickVertical")) < -.75f)
                        thirdSprintInput = true;
                }
            }
            #endregion

            #region Check Final Sprint Input
            if ((sprintInputCooler < .5) && thirdSprintInput)
            {
                CurrentCharacterState = CharacterState.SprintState;
                return true;
            }
            else if (sprintInputCooler >= .5f)
            {
                firstRightInput = false;
                firstLeftInput = false;
                firstForwardInput = false;
                firstBackInput = false;
                movementInputReset = false;
                thirdSprintInput = false;
                sprintInputCooler = 0;
                horizontalButtonPressTotal = 0;
                verticalButtonPressTotal = 0;
                return false;
            }
            #endregion
        }
        return false;
    }    
  
    #endregion
    private void ResearchState()
    {
        bool exitMenu = Input.GetButton("bButton");
        Debug.Log("Entered Research State");
        if (exitMenu)
        {
            mapMenu = false;
            Debug.Log("Exit Research State");
        }
        StateChange(CurrentCharacterState);
    }
    //Ground is found and within range land
    public void GroundFound(JumpResults jumpResults)
    {
        var groundCollisions = jumpResults.allHitObjectsInSearchRadius;       
    }

    private void TriggerIdleState()
    {
        currentCharacterState = CharacterState.IdleState;
        StateChange(CurrentCharacterState);
        stateMachine.ChangeState(new Idle(this.groundLayer, this.gameObject, this.viewRange, this.groundTag, this.navMeshAgent, this.AirFound));
    }
    private void TriggerJumpState()
    { 
        stateMachine.ChangeState(new Jump(this.groundLayer, this.gameObject, this.groundRange, this.groundTag, this.navMeshAgent, this.GroundFound));
    }

    private void TriggerDoubleJumpState()
    {
        //thirdPersonPlayerCharacter.HandleAirborneVelocities();
        //currentCharacterState = CharacterState.DoubleJumpState;              
    }

    private void TriggerWallRunState()
    {
        Debug.Log("Wall Run State");
    }
    private void TriggerRailGrindingState()
    {
        Debug.Log("Rail Grinding State");
    }

    public void AirFound(IdleResults idleResults)
    {
        var aerialItems = idleResults.allHitObjectsInSearchRadius;
        
    }
    public void WallFound(SprintResults sprintResults)
    {
        
    }

}

