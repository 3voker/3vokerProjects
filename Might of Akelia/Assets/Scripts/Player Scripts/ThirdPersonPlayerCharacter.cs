using UnityEngine;
using System.Collections;
using System;

//public enum Weapon
//{
//    UNARMED = 0,
//    RELAX = 8
//}

/// <summary>
/// This horrifyling refactored script from Unity 3rdPersonCharacter Demo Does the following:
///     Receives commands from player controller
///     Has a Trecking state machine that is used to alter player animations 
///     Trigger the players control velocity when 
///     Acts as the dancing puppet for the player controller and combat controller 
///     Stitched Together by DeAndre Christopher Owens
/// </summary>
public class ThirdPersonPlayerCharacter : MonoBehaviour
{
    PlayerController playerController;
    CombatController combatController;
    RailCarScript railCarScript;
    Rook rook;
    ControlVelocity controlVelocity;

    //public CharacterState thirdPersonPlayerCharacterState;

    //Assigned rail Balancer for RailGrinding
    public GameObject railCar;

    [SerializeField]
    private float jumpPower = 12;// determines the jump force applied when jumping (and therefore the jump height)

    //NEW Double jump is not normally part of third person character script!
    [SerializeField]
    private float doubleJumpPower = 8;
    [SerializeField]
    private float airSpeed = 6; // determines the max speed of the character while airborne
    [SerializeField]
    private float airControl = 2; // determines the response speed of controlling the character while airborne
    [Range(1, 4)]
    [SerializeField]
    public float gravityMultiplier = 2; // gravity modifier - often higher than natural gravity feels right for game characters
    [SerializeField]
    [Range(0.1f, 3f)]
    private float moveSpeedMultiplier = 1; // how much the move speed of the character will be multiplied by
    [SerializeField]
    [Range(0.1f, 3f)]
    private float animSpeedMultiplier = 1; // how much the animation of the character will be multiplied by
    [SerializeField]
    private AdvancedSettings advancedSettings; // Container for the advanced settings class , thiss allows the advanced settings to be in a foldout in the inspector

    [System.Serializable]
    public class AdvancedSettings
    {
        public float stationaryTurnSpeed = 180; // additional turn speed added when the player is stationary (added to animation root rotation)
        public float movingTurnSpeed = 360; // additional turn speed added when the player is moving (added to animation root rotation)
        public float headLookResponseSpeed = 2; // speed at which head look follows its target
        public float crouchHeightFactor = 0.6f; // collider height is multiplied by this when crouching
        public float crouchChangeSpeed = 4; // speed at which capsule changes height when crouching/standing
        public float autoTurnThresholdAngle = 100; // character auto turns towards camera direction if facing away by more than this angle
        public float autoTurnSpeed = 2; // speed at which character auto-turns towards cam direction
        public PhysicMaterial zeroFrictionMaterial; // used when in motion to enable smooth movement
        public PhysicMaterial slightFrictionMaterial;
        public PhysicMaterial bouncyFrictionMaterial;
        public PhysicMaterial highFrictionMaterial; // used when stationary to avoid sliding down slopes
        public float jumpRepeatDelayTime = 0.25f; // amount of time that must elapse between landing and being able to jump again
        public float doubleJumpDelayTime = .5f; //NEW amount of time before can do double jump
        public float runCycleLegOffset = 0.2f; // animation cycle offset (0-1) used for determining correct leg to jump off
        public float groundStickyEffect = 5f; // power of 'stick to ground' effect - prevents bumping down slopes.
    }

    #region Variables
    public Transform lookTarget { get; set; } // The point where the character will be looking at

    public Vector3 targetDashDirection;

    /// <summary>
    /// Layer Mask checks for Ground 
    /// </summary>

    public LayerMask groundLayerMask;

    /// <summary>
    /// Layer mask checks for scenarios player will crouch
    /// </summary>
    public LayerMask crouchLayerMask;

    /// <summary>
    /// Layer mask checks for grindable rails
    /// </summary>
    public LayerMask railGrindLayerMask;

    /// <summary>
    /// Layer mask checks for walls player can run on.
    /// </summary>
    public LayerMask wallRunLayerMask;

    /// <summary>
    /// bool that determines whether player is not moving
    /// </summary>
    public bool isTraversing;

    /// <summary>
    /// bool checks the character on the ground
    /// </summary>
    public bool onGround;
    /// <summary>
    /// bool that determines whether player is bouncing?
    /// </summary>
    public bool onBounce;
    /// <summary>
    /// bool that determines whether player is the character in water
    /// </summary>
    public bool inWater;
    /// <summary>
    /// bool that determines whether player is on a rail
    /// </summary>
    public bool onRail;

    /// <summary>
    /// bool that determines whether player is  on a wall
    /// </summary>
    public bool onWall;

    /// <summary>
    /// bool that determines whether player is not moving
    /// </summary>
    private bool isJumping;
    /// <summary>
    /// bool that determines whether player is falling
    /// </summary>
    private bool isFalling;
    /// <summary>
    /// bool that determines whether player should start fall
    /// </summary>
    bool startFall;

    float fallingVelocity = -1f;

    // Used for continuing momentum while in air

    float maxVelocity = 2f;
    float minVelocity = -2f;
    private bool isHurt;
    private bool isDead;


    private Vector3 currentLookPos; // The current position where the character is looking
    private float originalHeight; // Used for tracking the original height of the characters capsule collider
    private Animator animator; // The animator for the character

    private Animation characterAnimation;  //Animation for this character
    public float lastAirTime;// USed for checking when the character was last in the air for controlling jumps
    public float lastJumpTime; //NEW feature for double jump feature
    private CapsuleCollider capsule; // The collider for the character
    private const float half = 0.5f; // whats it says, it's a constant for a half
    private Vector3 moveInput;
    Vector3 newVelocity;
    private bool sprintInput;
    private bool crouchInput;
    private bool jumpInput;
    private bool doubleJumpInput;
    bool doubleJump;
    bool canDoubleJump = false;
    bool isDoubleJumping = false;
    public bool doublejumped = false;
    bool canAirDashed = false;
    public bool airDashed = false;

    bool canStomp;
    bool stomped;

    private float turnAmount;
    private float forwardAmount;
    private Vector3 velocity;
    private IComparer rayHitComparer;
    public float lookBlendTime;
    public float lookWeight;

    //bool target;
    public GameObject target;
    bool canEvade = true;
    float evadeTimer = 1;

    //rolling variables
    public float rollSpeed = 8;
    public float rollduration;
    public bool isRolling = false;

    #region 
    //movement variables
    bool canMove = true;
    public float walkSpeed = 1.35f;
    float moveSpeed;
    public float runSpeed = 6f;
    public float sprintSpeed = 12f;

    //Speed of character while on rail
    public float railThrust = 20f;

    float rotationSpeed = 40f;
    //Time it takes sprint to start up
    float sprintStartTime;
    #endregion
    //Weapon and Shield
    private Weapon weapon;
    int rightWeapon = 0;
    int leftWeapon = 0;
    bool isRelax = false;
    bool caminControl = false;
    //isStrafing/action variables
    bool canAction = true;
    bool isStrafing = false;
    //bool isDead = false;
    bool isBlocking = false;
    public float knockbackMultiplier = 1f;
    bool isKnockback;
    [Header("Menu Variables")]
    bool pauseMenu = false;
    bool mapMenu = false;
    bool inventoryMenu = false;
    [Header("Magic Variables")]
    float spellChargeTimer = 6.15f;
    bool spellCharge = false;
    float TechniqueChargeTimer = 3f;
    bool techniqueCharge = false;
    bool hasMP = true;

    Rigidbody rigidBody;
    new Collider collider;

    #endregion

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        combatController = GetComponent<CombatController>();

        rook = GetComponent<Rook>();

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        characterAnimation = GetComponentInChildren<Animation>();
        capsule = GetComponent<CapsuleCollider>();
        collider = GetComponent<CapsuleCollider>();
        //caminControl = Input.GetAxisRaw("rightJoystickHorizontal") != 0;
        // as can return null so we need to make sure thats its not before assigning to it
        if (capsule == null)
        {
            Debug.LogError(" collider cannot be cast to CapsuleCollider");
        }
        else
        {
            originalHeight = capsule.height;
            capsule.center = Vector3.up * originalHeight * half;
        }
        rayHitComparer = new RayHitComparer();
        SetUpAnimator();
        // give the look position a default in case the character is not under control
        currentLookPos = Camera.main.transform.position;
    }
    IEnumerator BlendLookWeight()
    {
        float t = 0f;
        while (t < lookBlendTime)
        {
            lookWeight = t / lookBlendTime;
            t += Time.deltaTime;
            yield return null;
        }
        lookWeight = 1f;
    }
    void OnEnable()
    {
        if (lookWeight == 0f)
        {
            StartCoroutine(BlendLookWeight());
        }
    }
    // The Move function is designed to be called from a separate component
    // based on User input, or an AI control script
    public void Move(Vector3 move, bool crouch, bool jump, Vector3 lookPos, bool doubleJump, bool sprint)
    {
        if (move.magnitude > 1) move.Normalize();
        // transfer input parameters to member variables.
        this.moveInput = move;
        this.crouchInput = crouch;
        this.sprintInput = sprint;
        this.jumpInput = jump;
        this.doubleJumpInput = doubleJump;
        this.currentLookPos = lookPos;

        //Target dash direction effects roll direction.
        targetDashDirection = move;

        // grab current velocity, we will be changing it.
        velocity = rigidBody.velocity;

        ConvertMoveInput(); // converts the relative move vector into local turn & fwd values

        if (combatController.CurrentCombatState == CombatState.DefensiveStanceState || combatController.CurrentCombatState == CombatState.TechniqueStanceState)
        {
            // makes the character face the way the camera is looking
            TurnTowardsCameraForward();
        }
        if (combatController.CurrentCombatState == CombatState.DieState)
        {
            StartCoroutine(_Death());
        }

        PreventStandingInLowHeadroom(); // so the character's head doesn't penetrate a low ceiling

        ScaleCapsuleForCrouching(); // so you can fit under low areas when crouching

        ApplyExtraTurnRotation(); // this is in addition to root rotation in the animations

        // CheckForGrounded();
       // RailGrindCheck();

        //If player is on ground and on rail(fix soon please!!) 
        if (onRail & onGround)
        {
            //0 is not controlled by player, instead controlled by railBalancer's local forward


            //treckingState = TreckingState.Grinding;
            //StateChange(treckingState);

            //If railCar is null
            if (railCar == null)
            {
                //Assign railCar from playerController
              //  railCar = playerController.railCar;

                //If railCar is STILL null
                if (railCar == null)
                {
                    //Look for it 
                    railCarScript = GameObject.FindGameObjectWithTag("RailCar").GetComponent<RailCarScript>();
                }
            }
            //Assign followPath to railCar followPath Script
            railCarScript = railCar.GetComponent<RailCarScript>();

            //Assign controlVelocity to railCar controlVelocity Script
           // controlVelocity = railCar.GetComponent<ControlVelocity>();

            //Use this characters mass, velocity, etc
            //To create a force on initial contact with Rail
            controlVelocity.LandingOnRail(velocity); //(maxVelocity, fallingVelicty(y), 0)

            doublejumped = false;
            airDashed = false;


            if (jump)
            {
               
            }

            HandleRailGrindingVelocities(crouch, jump);
        }

        RunnableWallCheck();

        if (onWall)
        {
            
           // treckingState = TreckingState.WallRunning;

            doublejumped = false;
            airDashed = false;

            HandleWallRunningVelocities(crouch, jump);
        }

        GroundCheck(); // detect and stick to ground

        SetFriction(); // use low or high friction values depending on the current state

        // control and velocity handling is different when grounded and airborne:
        if (onGround && !onRail)
        {
            doublejumped = false;
            airDashed = false;

            HandleGroundedVelocities(crouch, jump);
            //If player is not moving go into idle 
            if (move.magnitude == 0f && !isTraversing) //move.magnitude == 0f
            {
                //thirdPersonPlayerCharacterState = CharacterState.IdleState;
                //treckingState = TreckingState.Idle;
                //StateChange(treckingState);
                //Return movement Speed multiplier to its default setting. 
                moveSpeedMultiplier = 1.25f;
            }

            //If player is moving 
            //Return moveSpeed to default
            //Go into CharacterState.RoamingState 
            //Handle grounded movement
            //if (playerController.CurrentCharacterState == CharacterState.RoamingState)
            //{
            //    thirdPersonPlayerCharacterState = CharacterState.RoamingState;

            //    if (!sprint)
            //    {
                    //treckingState = TreckingState.Walking;
                    //StateChange(treckingState);
                    //Return movement Speed multiplier to its default setting. 
                    moveSpeedMultiplier = 1.25f;

                    //If player is sprinting 
                    //Increase moveSpeed
                    //Go into CharacterState.RoamingState 
                    //Go into TreckingState.Sprinting 
                    //Handle grounded movement
            //    }
            //}
            if (sprint) //thirdPersonPlayerCharacterState == CharacterState.SprintState
            {
                //thirdPersonPlayerCharacterState = CharacterState.SprintState;
                //treckingState = TreckingState.Sprinting;
                //StateChange(treckingState);
                //Update movespeedMultiplier to boost speed 
                //Slowly decrease speed over time 
                //Utilize Stamina Bar
                rigidBody.AddForce(moveInput * 5, ForceMode.VelocityChange);
                moveSpeedMultiplier = 2f;
            }

            if (jump)
            {
                //thirdPersonPlayerCharacterState = CharacterState.JumpState;
                //treckingState = TreckingState.Leaping;
            }
        }
        else
        {
            HandleAirborneVelocities();
        }
        UpdateMovement();
        UpdateAnimator(); // send input and other state parameters to the animator
                          // reassign velocity, since it will have been modified by the above functions.
        rigidBody.velocity = velocity;
    }


    private void ConvertMoveInput()
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (onGround)
        {
            Vector3 localMove = transform.InverseTransformDirection(moveInput);
            turnAmount = Mathf.Atan2(localMove.x, localMove.z);
            forwardAmount = localMove.z;
        }
    }
    private void TurnTowardsCameraForward()
    {
        Vector3 NeutralMovement = new Vector3(0, 0, 0);
        if (this.moveInput == NeutralMovement)
        {
            //Debug.Log("cam was last input");
            if (Mathf.Abs(forwardAmount) < .01f) //.01f original float value
            {
                Vector3 lookDelta = transform.InverseTransformDirection(currentLookPos - transform.position);
                float lookAngle = Mathf.Atan2(lookDelta.x, lookDelta.z) * Mathf.Rad2Deg;
                // are we beyond the threshold of where need to turn to face the camera?
                if (Mathf.Abs(lookAngle) > advancedSettings.autoTurnThresholdAngle)
                {
                    turnAmount += lookAngle * advancedSettings.autoTurnSpeed * .001f;
                }
            }
        }
    }
    private void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (!crouchInput)
        {
            Ray crouchRay = new Ray(rigidBody.position + Vector3.up * capsule.radius * half, Vector3.up);
            float crouchRayLength = originalHeight - capsule.radius * half;
            if (Physics.SphereCast(crouchRay, capsule.radius * half, crouchRayLength, crouchLayerMask))
            {
                crouchInput = true;
            }
        }
    }
    private void ScaleCapsuleForCrouching()
    {
        // scale the capsule collider according to
        // if crouching ...
        if (onGround && crouchInput && (capsule.height != originalHeight * advancedSettings.crouchHeightFactor))
        {
            capsule.height = Mathf.MoveTowards(capsule.height, originalHeight * advancedSettings.crouchHeightFactor, Time.deltaTime * 4);
            capsule.center = Vector3.MoveTowards(capsule.center, Vector3.up * originalHeight * advancedSettings.crouchHeightFactor * half, Time.deltaTime * 2);
        } // ... everything else 
        else if (capsule.height != originalHeight && capsule.center != Vector3.up * originalHeight * half)
        {
            capsule.height = Mathf.MoveTowards(capsule.height, originalHeight, Time.deltaTime * 4);
            capsule.center = Vector3.MoveTowards(capsule.center, Vector3.up * originalHeight * half, Time.deltaTime * 2);
        }
    }
    private void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(advancedSettings.stationaryTurnSpeed, advancedSettings.movingTurnSpeed,
                                     forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        if (moveInput != Vector3.zero && !isStrafing && !isRolling && !isBlocking)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveInput), Time.deltaTime * advancedSettings.movingTurnSpeed);
        }
    }
    #region Movement 
    //This method isn't getting used Dre...
    float UpdateMovement()
    {
        Vector3 motion = moveInput;
        if (onGround)
        {
            //reduce input for diagonal movement
            if (motion.magnitude > 1)
            {
                motion.Normalize();
            }
            if (playerController.canMove && playerController.playerControllerCombatState != CombatState.DefensiveStanceState)
            {
                //set speed by walking / running
                if (isStrafing)
                {
                    newVelocity = motion * walkSpeed;
                }

                else
                {
                    newVelocity = motion * runSpeed;
                }
                //if rolling use rolling speed and direction
                if (isRolling)
                {
                    //force the dash movement to 1
                    targetDashDirection.Normalize();
                    newVelocity = rollSpeed * targetDashDirection;
                }
            }
        }
        //if (onRail)
        //{
        //    if (motion.magnitude > 1)
        //    {
        //        motion.Normalize();
        //    }
        //}
        else
        {
            //if we are falling use momentum
            newVelocity = rigidBody.velocity;
        }
        if (!isStrafing || !playerController.canMove)
        {
            // playerController.CameraRelativeMovement();
            //RotateTowardsMovementDir(); //Test it out
        }
        if (isStrafing && !isRelax)
        {
            //make character point at target
            Quaternion targetRotation;
            Vector3 targetPos = target.transform.position;
            targetRotation = Quaternion.LookRotation(targetPos - new Vector3(transform.position.x, 0, transform.position.z));
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, (rotationSpeed * Time.deltaTime) * rotationSpeed);
        }
        newVelocity.y = rigidBody.velocity.y;
        rigidBody.velocity = newVelocity;
        //return a movement value for the animator
        return moveInput.magnitude;
    }
    #endregion

    #region Jumping

    //checks if character is within a certain distance from the ground, and markes it IsGrounded
    //void CheckForGrounded()
    //{
    //    Debug.Log("Check for ground");
    //    float distanceToGround;
    //    float threshold = .45f;
    //    RaycastHit hit;
    //    Vector3 offset = new Vector3(0, .4f, 0);
    //    if (Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
    //    {
    //        distanceToGround = hit.distance;
    //        if (distanceToGround < threshold)
    //        {
    //            Debug.Log("Is on ground");
    //            onGround = true;
    //            playerController.canJump = true;
    //            startFall = false;
    //            doublejumped = false;
    //            canDoubleJump = false;
    //            isFalling = false;        
    //            if (!isJumping)
    //            {
    //                animator.SetInteger("Jumping", 0);
    //            }

    //        }
    //        else
    //        {
    //            onGround = false;
    //        }
    //    }
    //}

    //void Jumping()
    //{
    //    if (onGround)
    //    {
    //        if (playerController.canJump && Input.GetButtonDown("Jump"))
    //        {          
    //            StartCoroutine(_Jump());
    //        }
    //    }
    //    else
    //    {
    //        canDoubleJump = true;
    //        playerController.canJump = false;
    //        if (isFalling)
    //        {
    //            //set the animation back to falling
    //            animator.SetInteger("Jumping", 2);
    //            //prevent from going into land animation while in air
    //            if (!startFall)
    //            {
    //                animator.SetTrigger("JumpTrigger");
    //                startFall = true;
    //            }
    //        }
    //        if (canDoubleJump && isJumping && Input.GetButtonUp("Jump") && !doublejumped && isFalling)
    //        {
    //            if (Input.GetButtonDown("Jump"))
    //            {

    //                startFall = false;
    //                StartCoroutine(_DoubleJump());

    //                // Apply the current movement to launch velocity
    //                animator.SetInteger("Jumping", 3);
    //                doublejumped = true;
    //            }

    //        }
    //    }
    //}

    //private IEnumerator _DoubleJump()
    //{
    //    isJumping = true;
    //    rigidBody.velocity += doubleJumpPower * Vector3.up;
    //    playerController.canDoubleJump = false;
    //    yield return new WaitForSeconds(.5f);
    //    isJumping = false;
    //}

    //IEnumerator _Jump()
    //{    
    //    isJumping = true;
    //    animator.SetInteger("Jumping", 1);
    //    animator.SetTrigger("JumpTrigger");
    //    // Apply the current movement to launch velocity
    //    rigidBody.velocity += jumpPower * Vector3.up;
    //    playerController.canJump = false;

    //    new WaitForSeconds(.2f);

    //    if (Input.GetButtonDown("Jump") && !onGround)
    //    {

    //            velocity.y = 0;
    //            rigidBody.AddForce(new Vector2(0, doubleJumpPower));

    //        yield return StartCoroutine(_DoubleJump());
    //    }
    //    yield return new WaitForSeconds(.5f);
    //    isJumping = false;         
    //}
    public void HandleAirborneVelocities()
    {
        // we allow some movement in air, but it's very different to when on ground
        // (typically allowing a small change in trajectory)

        Vector3 airMove = new Vector3(moveInput.x * airSpeed, velocity.y, moveInput.z * airSpeed);
        if (!onGround && canDoubleJump) //GetCharacterState(Future Refactor)
        {
            #region air variables 
            Vector3 motion = moveInput;
            bool airDash = (Input.GetButton("Jump") && moveInput.z != 0);  //Jump plus players forward direction        
            float velocityX = 0;
            float velocityZ = 0;
            motion *= (Mathf.Abs(moveInput.x) == 1 && Mathf.Abs(moveInput.z) == 1) ? 0.7f : 1;
            rigidBody.AddForce(motion * airSpeed, ForceMode.Acceleration);
            //limit the amount of velocity we can achieve          
            #endregion
            if (rigidBody.velocity.x > maxVelocity)
            {
                velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
                if (velocityX < 0)
                {
                    velocityX = 0;
                }
                rigidBody.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
            }
            if (rigidBody.velocity.x < minVelocity)
            {
                velocityX = rigidBody.velocity.x - minVelocity;
                if (velocityX > 0)
                {
                    velocityX = 0;
                }
                rigidBody.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
            }
            if (rigidBody.velocity.z > maxVelocity)
            {
                velocityZ = rigidBody.velocity.z - maxVelocity;
                if (velocityZ < 0)
                {
                    velocityZ = 0;
                }
                rigidBody.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
            }
            if (rigidBody.velocity.z < minVelocity)
            {
                velocityZ = rigidBody.velocity.z - minVelocity;
                if (velocityZ > 0)
                {
                    velocityZ = 0;
                }
                rigidBody.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
            }
            if (sprintInput)
            {
                airSpeed *= 6;
                jumpPower *= 2;
            }
        }

        velocity = Vector3.Lerp(velocity, airMove, Time.deltaTime * airControl);
        rigidBody.useGravity = true;

        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
        rigidBody.AddForce(extraGravityForce);
    }

    //If player movement input is greater than the threshold add inmpulse in X > Z vector
    public void HandleAirDash()
    {
        airDashed = true;

        //If doubleJumped has not been toggled perform airdash method
        if (doublejumped != true)
        {
            doublejumped = false;
            rigidBody.AddForce(moveInput * 2, ForceMode.VelocityChange); //MoveInput * 25

            rigidBody.useGravity = false;
            rigidBody.AddForce(new Vector3(moveInput.x / 3, doubleJumpPower / 6, moveInput.z / 3), ForceMode.Impulse); // something something
            airSpeed *= 1;
            jumpPower *= 1;
            velocity = moveInput * airSpeed;
            //velocity.y += doubleJumpPower / 6;
        }
        rigidBody.useGravity = true;
    }

    //If player movement input is less than the threshold add force impulse only to the Y vector
    public void HandleDoubleJump()
    {
        doublejumped = true;

        if (airDashed != true)
        {
            airDashed = false;

            rigidBody.AddForce(moveInput.x / 2f, doubleJumpPower / 4f, moveInput.z / 2f, ForceMode.Impulse);

            rigidBody.useGravity = false;
            // rigidBody.AddForce(new Vector3(moveInput.x / 2, doubleJumpPower / velocity.y, moveInput.z / 2), ForceMode.Impulse);
            //rigidBody.AddForce(new Vector3(moveInput.x, doubleJumpPower, moveInput.z), ForceMode.Impulse);
            airSpeed *= 1;
            jumpPower *= 1;
            velocity = moveInput * airSpeed;
            velocity.y += doubleJumpPower;
        }
        rigidBody.useGravity = true;
    }

    public void HandleAirStomp()
    {
        stomped = true;

        if (airDashed != true)
        {
            airDashed = false;

            rigidBody.AddForce(0, -doubleJumpPower, 0, ForceMode.VelocityChange);

            rigidBody.useGravity = false;
            rigidBody.AddForce(new Vector3(moveInput.x / 2, -doubleJumpPower / 4f, moveInput.z / 2), ForceMode.Impulse);
            //rigidBody.AddForce(new Vector3(moveInput.x, doubleJumpPower, moveInput.z), ForceMode.Impulse);
            airSpeed *= 1;
            jumpPower *= 1;
            velocity = moveInput * airSpeed;
            velocity.y -= doubleJumpPower;
        }
        rigidBody.useGravity = true;
    }

    /*  void AirControl()
 {
     //CameraRelativeMovement();
     if (!onGround)
     {
         #region air variables 
         Vector3 motion = moveInput;
         bool airDash = (Input.GetButton("Jump") && moveInput.z != 0);  //Jump plus players forward direction        
         float velocityX = 0;
         float velocityZ = 0;
         motion *= (Mathf.Abs(moveInput.x) == 1 && Mathf.Abs(moveInput.z) == 1) ? 0.7f : 1;
         rigidBody.AddForce(motion * airSpeed, ForceMode.Acceleration);
         //limit the amount of velocity we can achieve          
         #endregion
         if (rigidBody.velocity.x > maxVelocity)
         {
             velocityX = GetComponent<Rigidbody>().velocity.x - maxVelocity;
             if (velocityX < 0)
             {
                 velocityX = 0;
             }
             rigidBody.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
         }
         if (rigidBody.velocity.x < minVelocity)
         {
             velocityX = rigidBody.velocity.x - minVelocity;
             if (velocityX > 0)
             {
                 velocityX = 0;
             }
             rigidBody.AddForce(new Vector3(-velocityX, 0, 0), ForceMode.Acceleration);
         }
         if (rigidBody.velocity.z > maxVelocity)
         {
             velocityZ = rigidBody.velocity.z - maxVelocity;
             if (velocityZ < 0)
             {
                 velocityZ = 0;
             }
             rigidBody.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
         }
         if (rigidBody.velocity.z < minVelocity)
         {
             velocityZ = rigidBody.velocity.z - minVelocity;
             if (velocityZ > 0)
             {
                 velocityZ = 0;
             }
             rigidBody.AddForce(new Vector3(0, 0, -velocityZ), ForceMode.Acceleration);
         }
     }
 }
 */ //Soon to be ghost code hopefully
    #endregion
    #region MiscMethods
    //0 = No side
    //1 = Left
    //2 = Right
    //3 = Dual

    public void CombatMovement(bool isInCombatStance, bool magic, bool defend, bool tech)
    {
        this.crouchInput = defend;
        this.canAction = isInCombatStance;
        this.spellCharge = magic;
        this.techniqueCharge = tech;
    }
    public void Attack(int attackSide)
    {
        animator.SetTrigger("IdleCombatStance");
        if (canAction)
        {
           
            if (weapon == Weapon.UNARMED)
            {
                int maxAttacks = 3;
                int attackNumber = 0;
                if (attackSide == 1 || attackSide == 3)
                {
                    attackNumber = UnityEngine.Random.Range(3, maxAttacks);
                }
                else if (attackSide == 2)
                {
                    attackNumber = UnityEngine.Random.Range(6, maxAttacks + 3);
                }
                if (onGround)
                {
                    if (attackSide != 3)
                    {
                        animator.SetTrigger("Attack" + (attackNumber).ToString() + "Trigger");
                        if (leftWeapon == 12 || leftWeapon == 14 || rightWeapon == 13 || rightWeapon == 15)
                        {
                            StartCoroutine(_LockMovementAndAttack(0, .75f));
                        }
                        else
                        {
                            StartCoroutine(_LockMovementAndAttack(0, .6f));
                        }
                    }
                    //else
                    //{
                    //    animator.SetTrigger("AttackDual" + (attackNumber).ToString() + "Trigger");
                    //    StartCoroutine(_LockMovementAndAttack(0, .75f));
                    //}
                }
            }
            //2 handed weapons
            else
            {
                if (onGround)
                {
                    animator.SetTrigger("Attack" + (6).ToString() + "Trigger");
                    StartCoroutine(_LockMovementAndAttack(0, .85f));
                }
            }
        }
    }
    void AttackKick(int kickSide)
    {
        if (onGround)
        {
            if (kickSide == 1)
            {
                animator.SetTrigger("AttackKick1Trigger");
            }
            else
            {
                animator.SetTrigger("AttackKick2Trigger");
            }
            StartCoroutine(_LockMovementAndAttack(0, .8f));
        }
    }
    //0 = No side
    //1 = Left
    //2 = Right
    //3 = Dual
    public void CastAttack(int attackSide)
    {
        if (weapon == Weapon.UNARMED)
        {
            int maxAttacks = 3;
            if (attackSide == 1)
            {
                int attackNumber = UnityEngine.Random.Range(0, maxAttacks);
                if (onGround)
                {
                    animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
                    StartCoroutine(_LockMovementAndAttack(0, .8f));
                }
            }
            if (attackSide == 2)
            {
                int attackNumber = UnityEngine.Random.Range(3, maxAttacks + 3);
                if (onGround)
                {
                    animator.SetTrigger("CastAttack" + (attackNumber + 1).ToString() + "Trigger");
                    StartCoroutine(_LockMovementAndAttack(0, .8f));
                }
            }
            if (attackSide == 3)
            {
                int attackNumber = UnityEngine.Random.Range(0, maxAttacks);
                if (onGround)
                {
                    animator.SetTrigger("CastDualAttack" + (attackNumber + 1).ToString() + "Trigger");
                    StartCoroutine(_LockMovementAndAttack(0, 1f));
                }
            }
        }
    }
    public void GetHit()
    {

        int hits = 5;
        int hitNumber = UnityEngine.Random.Range(0, hits);
        animator.SetTrigger("GetHit" + (hitNumber + 1).ToString() + "Trigger");
        StartCoroutine(_LockMovementAndAttack(.1f, .4f));
        //apply directional knockback force
        if (hitNumber <= 1)
        {
            StartCoroutine(_Knockback(-transform.forward, 8, 4));
        }
        else if (hitNumber == 2)
        {
            StartCoroutine(_Knockback(transform.forward, 8, 4));
        }
        else if (hitNumber == 3)
        {
            StartCoroutine(_Knockback(transform.right, 8, 4));
        }
        else if (hitNumber == 4)
        {
            StartCoroutine(_Knockback(-transform.right, 8, 4));
        }
    }

    IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount)
    {
        isKnockback = true;
        StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
        yield return new WaitForSeconds(.1f);
        isKnockback = false;
    }

    IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount)
    {
        while (isKnockback)
        {
            rigidBody.AddForce(knockDirection * ((knockBackAmount + UnityEngine.Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
            yield return null;
        }
    }

    IEnumerator _Death()
    {
        animator.SetTrigger("Death1Trigger");
        StartCoroutine(_LockMovementAndAttack(.1f, 1.5f));
        isDead = true;
        animator.SetBool("Moving", false);
        moveInput = new Vector3(0, 0, 0);
        yield return null;
    }

    IEnumerator _Revive()
    {
        animator.SetTrigger("Revive1Trigger");
        isDead = false;
        yield return null;
    }

    #endregion

    #region Rolling

    public void Rolling()
    {
        if (!isRolling && onGround)
        {
            if (Input.GetAxis("leftJoystickVertical") > .5 || Input.GetAxis("leftJoystickVertical") < -.5 || Input.GetAxis("leftJoystickHorizontal") > .5 || Input.GetAxis("leftJoystickHorizontal") < -.5)
            {

                StartCoroutine(_DirectionalRoll(Input.GetAxis("leftJoystickVertical"), Input.GetAxis("leftJoystickHorizontal")));
            }
        }
    }

    public IEnumerator _DirectionalRoll(float x, float v)
    {
        //check which way the dash is pressed relative to the character facing
        float angle = Vector3.Angle(targetDashDirection, -transform.forward);
        float sign = Mathf.Sign(Vector3.Dot(transform.up, Vector3.Cross(targetDashDirection, transform.forward)));
        // angle in [-179,180]
        float signed_angle = angle * sign;
        //angle in 0-360
        float angle360 = (signed_angle + 180) % 360;
        //deternime the animation to play based on the angle
        if (angle360 > 315 || angle360 < 45)
        {
            StartCoroutine(_Roll(1));
        }
        if (angle360 > 45 && angle360 < 135)
        {
            StartCoroutine(_Roll(2));
        }
        if (angle360 > 135 && angle360 < 225)
        {
            StartCoroutine(_Roll(3));
        }
        if (angle360 > 225 && angle360 < 315)
        {
            StartCoroutine(_Roll(4));
        }
        yield return null;
    }

    IEnumerator _Roll(int rollNumber)
    {
        if (rollNumber == 1)
        {
            animator.SetTrigger("RollForwardTrigger");
        }
        if (rollNumber == 2)
        {
            animator.SetTrigger("RollRightTrigger");
        }
        if (rollNumber == 3)
        {
            animator.SetTrigger("RollBackwardTrigger");
        }
        if (rollNumber == 4)
        {
            animator.SetTrigger("RollLeftTrigger");
        }
        isRolling = true;
        yield return new WaitForSeconds(rollduration);
        isRolling = false;
    }

    #endregion

    #region _Coroutines

    //method to keep character from moveing while attacking, etc
    public IEnumerator _LockMovementAndAttack(float delayTime, float lockTime)
    {
        yield return new WaitForSeconds(delayTime);
        canAction = false;
        //playerController.canMove = false;
      //  animator.SetBool("Moving", false);
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        moveInput = new Vector3(0, 0, 0);
      //  animator.applyRootMotion = true;
        yield return new WaitForSeconds(lockTime);
        canAction = true;
        playerController.canMove = true;
        animator.applyRootMotion = false;
    }

    #endregion

    #region GUI
    //void OnGUI()
    //{
    //    if (!isDead)
    //    {
    //        if (canAction && !isRelax)
    //        {
    //            if (onGround)
    //            {
    //                if (!isBlocking)
    //                {
    //                    if (!isBlocking)
    //                    {
    //                        if (GUI.Button(new Rect(25, 15, 100, 30), "Roll Forward"))
    //                        {
    //                            targetDashDirection = transform.forward;
    //                            StartCoroutine(_Roll(1));
    //                        }
    //                        if (GUI.Button(new Rect(130, 15, 100, 30), "Roll Backward"))
    //                        {
    //                            targetDashDirection = -transform.forward;
    //                            StartCoroutine(_Roll(3));
    //                        }
    //                        if (GUI.Button(new Rect(25, 45, 100, 30), "Roll Left"))
    //                        {
    //                            targetDashDirection = -transform.right;
    //                            StartCoroutine(_Roll(4));
    //                        }
    //                        if (GUI.Button(new Rect(130, 45, 100, 30), "Roll Right"))
    //                        {
    //                            targetDashDirection = transform.right;
    //                            StartCoroutine(_Roll(2));
    //                        }
    //                        //ATTACK LEFT
    //                        if (GUI.Button(new Rect(25, 85, 100, 30), "Attack L"))
    //                        {
    //                            Attack(1);
    //                        }
    //                        //ATTACK RIGHT
    //                        if (GUI.Button(new Rect(130, 85, 100, 30), "Attack R"))
    //                        {
    //                            Attack(2);
    //                        }
    //                        if (weapon == Weapon.UNARMED)
    //                        {
    //                            if (GUI.Button(new Rect(25, 115, 100, 30), "Left Kick"))
    //                            {
    //                                AttackKick(1);
    //                            }
    //                            if (GUI.Button(new Rect(130, 115, 100, 30), "Right Kick"))
    //                            {
    //                                AttackKick(2);
    //                            }
    //                        }
    //                        if (GUI.Button(new Rect(30, 240, 100, 30), "Get Hit"))
    //                        {
    //                            GetHit();
    //                        }
    //                    }
    //                }
    //            }
    //            if (playerController.canJump || canDoubleJump)
    //            {
    //                if (onGround)
    //                {
    //                    if (GUI.Button(new Rect(25, 165, 100, 30), "Jump"))
    //                    {
    //                        if (playerController.canJump && onGround)
    //                        {
    //                            StartCoroutine(_Jump());
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    if (GUI.Button(new Rect(25, 165, 100, 30), "Double Jump"))
    //                    {
    //                        if (canDoubleJump && !isDoubleJumping)
    //                        {
    //                            StartCoroutine(_Jump());
    //                        }
    //                    }
    //                }
    //            }
    //            if (!isBlocking && onGround)
    //            {
    //                if (GUI.Button(new Rect(30, 270, 100, 30), "Death"))
    //                {
    //                    StartCoroutine(_Death());
    //                }
    //            }
    //        }
    //    }
    //    if (isDead)
    //    {
    //        if (GUI.Button(new Rect(30, 270, 100, 30), "Revive"))
    //        {
    //            StartCoroutine(_Revive());
    //        }
    //    }
    //}
    #endregion
    #region Layer Masks 

    //Check for ground layer mask
    private void GroundCheck()
    {

        Ray ray = new Ray(transform.position + Vector3.up * .1f, -Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, .5f, groundLayerMask);
        System.Array.Sort(hits, rayHitComparer);

        if (velocity.y < jumpPower * .5f) //Figure out what .5 stands for and replace it
        {
            onGround = false;
            rigidBody.useGravity = true;

            foreach (var hit in hits)
            {
                // check whether we hit a non-trigger collider (and not the character itself)
                if (!hit.collider.isTrigger)
                {
                    // this counts as being on ground.

                    // stick to surface - helps character stick to ground - specially when running down slopes
                    if (velocity.y <= 0)
                    {
                        rigidBody.position = Vector3.MoveTowards(rigidBody.position, hit.point,
                                                                 Time.deltaTime * advancedSettings.groundStickyEffect);
                    }

                    onGround = true;
                    rigidBody.useGravity = false;
                    break;
                }
            }
        }
        // remember when we were last in air, for jump delay
        if (!onGround)
        {
            lastAirTime = Time.time;
            lastJumpTime = Time.time;
        }
    }
    //Unfinished!
    //Check for grindable rail layer masks
    private void RailGrindCheck()
    {
        Ray ray = new Ray(transform.position + Vector3.up * .1f, -Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, .5f, railGrindLayerMask);
        System.Array.Sort(hits, rayHitComparer);


       
        if (velocity.y < jumpPower * .5f) //Figure out what .5 stands for and replace it
        {
            onRail = false;
            rigidBody.useGravity = true;

            foreach (var hit in hits)
            {
                // check whether we hit a non-trigger collider (and not the character itself)
                if (!hit.collider.isTrigger)
                {
                    // this counts as being on ground.

                    // stick to surface - helps character stick to ground - specially when running down slopes
                    if (velocity.y <= 0)
                    {

                        rigidBody.position = Vector3.MoveTowards(rigidBody.position, hit.point,
                                                                 Time.deltaTime * advancedSettings.groundStickyEffect / 2); // Time.deltaTime * advancedSettings.groundStickyEffect
                    }

                    onRail = true;
                    rigidBody.useGravity = true;
                    break;
                }
            }
        }
        // remember when we were last in air, for jump delay
        if (!onRail)
        {
            lastAirTime = Time.time;
            lastJumpTime = Time.time;
        }
    }
    //Unfinished!
    //Check for runnable surfaces layer masks
    private void RunnableWallCheck()
    {
        Ray ray = new Ray(transform.position + Vector3.up * .1f, -Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, .5f, wallRunLayerMask);
        System.Array.Sort(hits, rayHitComparer);

        if (velocity.y < jumpPower * .5f) //Figure out what .5 stands for and replace it
        {
            onWall = false;
            rigidBody.useGravity = true;

            foreach (var hit in hits)
            {
                // check whether we hit a non-trigger collider (and not the character itself)
                if (!hit.collider.isTrigger)
                {
                    // this counts as being on ground.

                    // stick to surface - helps character stick to ground - specially when running down slopes
                    if (velocity.y <= 0)
                    {
                        rigidBody.position = Vector3.MoveTowards(rigidBody.position, hit.point,
                                                                 Time.deltaTime * advancedSettings.groundStickyEffect);
                    }

                    onWall = true;
                    rigidBody.useGravity = false;
                    break;
                }
            }
        }
    }

    #endregion
    private void SetFriction()
    {
        if (onGround)
        {
            // set friction to low or high, depending on if we're moving
            if (moveInput.magnitude == 0)
            {
                // when not moving this helps prevent sliding on slopes:
                collider.material = advancedSettings.highFrictionMaterial;
            }
            else
            {
                // but when moving, we want no friction:
                collider.material = advancedSettings.zeroFrictionMaterial;
            }
        }
        else if (onRail)
        {
            collider.material = advancedSettings.slightFrictionMaterial;
            // rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            // rigidBody.AddRelativeForce(0,-1, railThrust, ForceMode.Force);
        }
        else if (onBounce)
        {
            collider.material = advancedSettings.bouncyFrictionMaterial;
        }
        else
        {
            // while in air, we want no friction against surfaces (walls, ceilings, etc)
            collider.material = advancedSettings.zeroFrictionMaterial;
        }
    }
    #region Character Terrain Physics(Trecking Enum) 
    //This is the real stuff 
    private void HandleGroundedVelocities(bool crouch, bool jump)
    {
        canDoubleJump = Time.time > lastJumpTime + advancedSettings.doubleJumpDelayTime;
        velocity.y = 0;

        if (moveInput.magnitude == 0)
        {
            // when not moving this prevents sliding on slopes:
            velocity.x = 0;
            velocity.z = 0;
        }
        // check whether conditions are right to allow a jump:
        bool animationGrounded = animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded");
        //bool okToRepeatJump = Time.time > lastAirTime + advancedSettings.jumpRepeatDelayTime;

        //NEW Create different condition that doesn't rely on time/grounded variable         
        if (jumpInput && !crouchInput && animationGrounded)
        {
            // jump!
            if (sprintInput && onGround)
            {
                airSpeed *= 6; //Change to velocity  times airspeed
                jumpPower *= 2; //Also check velocity times jump power
            }
            else
            {
                airSpeed *= 1; //Velocity times airspeed
                jumpPower *= 1; //velocity times jump power
                onGround = false;
                velocity = moveInput * airSpeed;
                velocity.y = jumpPower;
            }
        }
    }
    private void HandleRailGrindingVelocities(bool crouch, bool jump)
    {
        //Make slippery
        collider.material = advancedSettings.zeroFrictionMaterial;

        canDoubleJump = Time.time > lastJumpTime + advancedSettings.doubleJumpDelayTime;

        //Determine direction in which player moves based on directional velocity
        velocity = Quaternion.FromToRotation(Vector3.forward, transform.forward) * rigidBody.velocity;

        //Based on velocity propel player in either forward or backwards. 

        // check whether conditions are right to allow a jump:
        bool animationGrinding = animator.GetCurrentAnimatorStateInfo(0).IsName("Grinding");

        //if (!crouchInput)
        //{
        //    animation["Unarmed-Land"].time = .01f;
        //    animation["Unarmed-Land"].speed = 0.1f;
        //    animation.Play("Unarmed-Land");
        //}


        //NEW Create different condition that doesn't rely on time/grounded variable         
        if (jumpInput && !crouchInput && animationGrinding)
        {
            ///Add new velocity to player that is amplified by speed on rail
            if (sprintInput && onRail)
            {
                airSpeed *= 6; //Change to velocity  times airspeed
                jumpPower *= 2; //Also check velocity times jump power
            }
            else
            {
                airSpeed *= 1; //Velocity times airspeed
                jumpPower *= 1; //velocity times jump power
                onRail = false;
                velocity = moveInput * airSpeed;
                velocity.y = jumpPower;
            }
        }
    }
    //Handle velocity and speed of player character while on walls
    //Player should need stamina to maintain velocity 

    private void HandleWallRunningVelocities(bool crouch, bool jump)
    {
        canDoubleJump = Time.time > lastJumpTime + advancedSettings.doubleJumpDelayTime;
        velocity.y = 0;

        //Once moveinput magnitude falls below a certain threshold or stamina depletes player should grip wall 
        //Enter wallattackstance 
        if (moveInput.magnitude == 0)
        {
            // when not moving this prevents sliding on slopes:

            velocity.x = 0;
            velocity.z = 0;
        }
        // check whether conditions are right to allow a jump:
        bool animationGrounded = animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded");
        //bool okToRepeatJump = Time.time > lastAirTime + advancedSettings.jumpRepeatDelayTime;

        //NEW Create different condition that doesn't rely on time/grounded variable         
        if (jumpInput && !crouchInput && animationGrounded)
        {
            // jump!
            if (sprintInput && onGround)
            {
                airSpeed *= 6; //Change to velocity  times airspeed
                jumpPower *= 2; //Also check velocity times jump power
            }
            else
            {
                airSpeed *= 1; //Velocity times airspeed
                jumpPower *= 1; //velocity times jump power
                onGround = false;
                velocity = moveInput * airSpeed;
                velocity.y = jumpPower;
            }
        }
    }

    #endregion
    private void UpdateAnimator()
    {
        // Here we tell the animator what to do based on the current states and inputs.
        // only use root motion when on ground:
        animator.applyRootMotion = onGround;
        // update the animator parameters
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
        animator.SetBool("Crouch", crouchInput);
        animator.SetBool("OnGround", onGround);
        animator.SetBool("OnRail", onRail);

        animator.SetBool("AttackStance", combatController.CurrentCombatState == CombatState.IdleCombatState);
        if (!onGround)
        {
            animator.SetFloat("Jump", velocity.y);
            //TEST TO SEE WHAT DO
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime + advancedSettings.runCycleLegOffset, 1);
        float jumpLeg = (runCycle < half ? 1 : -1) * forwardAmount;
        if (onGround)
        {
            if (onRail)
            {
                animator.SetFloat("Jump", velocity.y);
                animator.applyRootMotion = onRail;
            }

            animator.SetFloat("JumpLeg", jumpLeg);
        }
        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (onGround && moveInput.magnitude > 0)
        {
            animator.speed = animSpeedMultiplier;
        }
        else
        {
            // but we don't want to use that while airborne
            animator.speed = 1;
        }
    }
    private void OnAnimatorIK(int layerIndex)
    {
        // we set the weight so most of the look-turn is done with the head, not the body.
        animator.SetLookAtWeight(lookWeight, 0.2f, 2.5f);

        // if a transform is assigned as a look target, it overrides the vector lookPos value

        if (lookTarget != null)
        {
            currentLookPos = lookTarget.position;
        }
        // Used for the head look feature.
        animator.SetLookAtPosition(currentLookPos);
    }
    private void SetUpAnimator()
    {
        // this is a ref to the animator component on the root.
        animator = GetComponent<Animator>();

        // we use avatar from a child animator component if present
        // (this is to enable easy swapping of the character model as a child node)
        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != animator)
            {
                animator.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }
    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        rigidBody.rotation = animator.rootRotation;
        if (onGround && Time.deltaTime > 0)
        {
            Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;
            // we preserve the existing y part of the current velocity.
            v.y = rigidBody.velocity.y;
            rigidBody.velocity = v;
        }
    }
    void OnDisable()
    {
        lookWeight = 0f;
    }
    //used for comparing distances
    private class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit)x).distance.CompareTo(((RaycastHit)y).distance);
        }
    }

    void LateUpdate()
    {

    }
}