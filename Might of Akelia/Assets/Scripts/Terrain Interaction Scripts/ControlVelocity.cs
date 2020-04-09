using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody))]
public class ControlVelocity : MonoBehaviour
{
    /// <summary>
    /// Use to switch between Force Modes
    /// </summary>
    public enum ModeSwitching { Start, Impulse, Acceleration, Force, VelocityChange };

    CombatState combatState;

    public ModeSwitching modeSwitching;

    #region railGrinding Velocity Variables

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public float minRailSpeed = 2f;

    /// <summary>
    /// Current speed GameObject is traveling on rail
    /// </summary>
    public float currentRailSpeed;

    /// <summary>
    /// Maximum speed GameObject can travel on rail
    /// </summary>
    public float maxRailSpeed = 20f;

    /// <summary>
    /// Increases rail speed a certain amount per second(Base increase)
    /// </summary>
    public float railAccelerationPerSecond;

    /// <summary>
    /// Time for rail to elapse a full cycle 
    /// </summary>
    public float railTimeElapsed = 15f;

    /// <summary>
    /// Velocity of this gameObject going forward
    /// </summary>
    private float forwardVelocity;
    /// <summary>
    /// Small slope that will increase or decrease speed based on incline
    /// </summary>
    private float smallSlope = 1f;

    /// <summary>
    /// Medium slope that will increase or decrease speed based on incline
    /// </summary>
    private float mediumSlope = 6f;

    /// <summary>
    /// Large slope that will increase or decrease speed based on incline
    /// </summary>
    private float largeSlope = 12f;

    /// <summary>
    /// Toggled true if next Vector of rail is higher than current Segment
    /// </summary>
    public bool inclineSegment;

    /// <summary>
    ///Toggled true if next Vector of rail is higher than current Segment
    /// </summary>
    public bool declineSegment;

    /// <summary>
    /// Reference to Movement Path Used
    /// </summary>
    public  bool isGrinding;
    #endregion

    /// <summary>
    /// Reference to game objects rigid body
    /// </summary>
    private Rigidbody rigidBody;
    /// <summary>
    /// Reference to start position and starting force of game object
    /// </summary>
    Vector3 startPosition, startForce;
    /// <summary>
    /// Reference to the game objects previous position saved to log speed and distance
    /// </summary>
    Vector3 prevPosition;
    /// <summary>
    /// New force applied to rigid body
    /// </summary>
    Vector3 newForce;
    /// <summary>
    /// String for the X force of rigid body that can be inputted on screen
    /// </summary>
    string forceXString = string.Empty;
    /// <summary>
    /// String for the Y force of rigid body that can be inputted on screen
    /// </summary>
    string forceYString = string.Empty;
    /// <summary>
    /// String for the Z force of rigid body that can be inputted on screen
    /// </summary>
    string forceZString = string.Empty;
    /// <summary>
    /// Reference to Movement Path Used
    /// </summary>
    public Vector3 transferredVelocity;

    /// <summary>
    /// Max Y velocity of rigidBody
    /// </summary>
    float maxYVelocity = .2f;
    /// <summary>
    /// Amount game object accelerates
    /// </summary>
    public float acceleration = 10f;

    /// <summary>
    /// Amount game object deccelerates
    /// </summary>
    public float deceleration = 10f;
    
    /// <summary>
    /// Force Direction applied to rigid body in X, Y, Z
    /// </summary>
    public float forceX, forceY, forceZ;
    /// <summary>
    /// Result of something...I can't remember
    /// </summary>
    public float result;

    /// <summary>
    ///Toggles whether player inputs crouch
    /// </summary>
    public bool isCrouching;

    /// <summary>
    ///Toggles whether player inputs jump
    /// </summary>
    public bool isJumping;

    /// <summary>
    /// Toggles whether player inputs crouch at the correct time
    /// </summary>
    public bool crouchedOnTime;
    /// <summary>
    /// Timer for crouch release
    /// </summary>
    public float crouchReleaseTimer;

    /// <summary>
    /// Time Limit for crouch release before hitting an incline
    /// </summary>
    public float crouchReleaseTimeLimit;
    /// <summary>
    /// Is utilizing stamina to increase speed
    /// </summary>

    public bool isBoosting;

    public bool isAutoBalancing;

    //Used by DollyCart to use as speed. 
    public float theSpeed;

    public float timerToChangeSpeed = .5f;

    void Start()
    {
        //You get the Rigidbody component you attach to the GameObject
        rigidBody = this.GetComponent<Rigidbody>();

        //This starts at first mode (nothing happening yet)
        modeSwitching = ModeSwitching.Start;

        //Initialising the force which is used on GameObject in various ways
        newForce = new Vector3(-5.0f, 1.0f, 0.0f);

        //Initialising floats
        forceX = 0;
        forceY = 0;
        forceZ = 0;
        //The forces typed in from the text fields (the ones you can manipulate in Game view)
        forceXString = "0";
        forceYString = "0";
        forceZString = "0";
        //The GameObject's starting position and Rigidbody position
        startPosition = transform.position;
        startForce = rigidBody.transform.position;

        currentRailSpeed = Vector3.Distance(transform.position, prevPosition);

        //Need a speedometer to show current speed

        railAccelerationPerSecond = maxRailSpeed / railTimeElapsed;

        forwardVelocity = 0f;

    }
    void Update()
    {
        // Trying to Limit Speed
        if (rigidBody.velocity.magnitude > maxRailSpeed)
        {
            rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxRailSpeed);
        }

        if (currentRailSpeed > maxRailSpeed)
        {
            currentRailSpeed = maxRailSpeed;
        }
      
    }
    void FixedUpdate()
    {
        //  Debug.LogFormat("Current Speed is {0}: ", currentRailSpeed.ToString());
        // prevPosition = transform.position;
        Debug.Log("ControlVelocity theSpeed: " + theSpeed);
        //Pressing the left trigger will trigger isCrouching 
        isCrouching = (Input.GetAxisRaw("leftTrigger") != 0);

        //right trigger pressed will cause boost //May not need
        isBoosting = (Input.GetAxisRaw("rightTrigger") != 0);

        declineSegment = (prevPosition.y > this.transform.position.y); 

        inclineSegment = (prevPosition.y < this.transform.position.y);
        //If the current mode is not the starting mode (or the GameObject is not reset), the force can change
        if (modeSwitching != ModeSwitching.Start)
        {
            //The force changes depending what you input into the text fields
            newForce = new Vector3(forceX, forceY, forceZ);
        }

        //Here, switching modes depend on button presses in the Game mode
        switch (modeSwitching)
        {
            //This is the starting mode which resets the GameObject
            case ModeSwitching.Start:
                //Set timer for velocity change
                timerToChangeSpeed = .5f;
                //This resets the GameObject and Rigidbody to their starting positions
                transform.position = startPosition;
                rigidBody.transform.position = startForce;

                //Countdown timer before velocity change
                timerToChangeSpeed -= Time.deltaTime;
                //This resets the velocity of the Rigidbody
                if(timerToChangeSpeed <= 0)
                {
                    modeSwitching = ModeSwitching.VelocityChange;
                }
               // rigidBody.velocity = new Vector3(0f, 0f, 0f);

                break;

            //These are the modes ForceMode can force on a Rigidbody
            //This is Acceleration mode
            case ModeSwitching.Acceleration:
                //The function converts the text fields into floats and updates the Rigidbody’s force
                //MakeCustomForce();


                //Use Acceleration as the force on the Rigidbody
                rigidBody.AddForce(newForce, ForceMode.Acceleration);

               
                break;

            //This is Force Mode, using a continuous force on the Rigidbody considering its mass
            case ModeSwitching.Force:
                //Converts the text fields into floats and updates the force applied to the Rigidbody
                MakeCustomForce();

                //Use Force as the force on GameObject’s Rigidbody
                rigidBody.AddForce(newForce, ForceMode.Force);
                break;

            //This is Impulse Mode, which involves using the Rigidbody’s mass to apply an instant impulse force.
            case ModeSwitching.Impulse:
                //The function converts the text fields into floats and updates the force applied to the Rigidbody
                MakeCustomForce();

                //Use Impulse as the force on GameObject
                rigidBody.AddForce(newForce, ForceMode.Impulse);
                
                break;

            //This is VelocityChange which involves ignoring the mass of the GameObject and impacting it with a sudden speed change in a direction
            case ModeSwitching.VelocityChange:
                //Converts the text fields into floats and updates the force applied to the Rigidbody
                MakeCustomForce();

                if (this.gameObject.name == "Rail Balancer")
                {
                    if (isGrinding)
                    {
                        //Increment forwardVelocity by railAcceleration a second times Time.DeltaTime 
                        forwardVelocity += railAccelerationPerSecond * Time.deltaTime;

                        //Assign ForceZ to forward Velocity float
                        forceZ = forwardVelocity;

                        //if (declineSegment)
                        //{
                        //    SteadilyIncreaseVelocity();
                        //}
                        //if (inclineSegment)
                        //{
                        //    SteadilyDecreaseVelocity();
                        //}
                    }
                   
                }
                //Make a Velocity change on the Rigidbody
              //  rigidBody.AddForce(newForce, ForceMode.VelocityChange);
                break;
        }
        ControlAcceleration();
        ControlDeceleration();
        //theSpeed = forwardVelocity;

       
        
        Debug.Log("Forward Velocity: " + forwardVelocity);

        prevPosition = transform.position;
    }
    #region Acceleration / Decleration Methods
    private void ControlAcceleration()
    {
        float minAcceleration = 0.1f;
        float maxAcceleration = 10f;

        if (theSpeed < maxRailSpeed){ theSpeed = theSpeed - acceleration * Time.deltaTime;}

        if (acceleration <= minAcceleration)
        {
            acceleration = minAcceleration;
        }
        if (acceleration > maxAcceleration)
        {
            acceleration = maxAcceleration;
        }

        else theSpeed = minRailSpeed + acceleration;
    }

  private void ControlDeceleration()
    {
        float minDecceleration = 0.1f;
        float maxDecceleration = 10f;

        if (theSpeed > deceleration * Time.deltaTime) { theSpeed = theSpeed - deceleration * Time.deltaTime; }

    if (deceleration <= minDecceleration)
        {
            deceleration = minDecceleration;
        }
    if (deceleration > maxDecceleration)
    {
        deceleration = maxDecceleration;
    }
    else theSpeed = minRailSpeed;
    }
    #endregion
    //The function outputs buttons, text fields, and other interactable UI elements to the Scene in Game view
    void OnGUI()
    {
        GUI.Label(new Rect(20, 60, 200, 200), "rigidbody velocity: " + rigidBody.velocity);
        GUI.Label(new Rect(20, 20, 180, 180), "ModeSwitch: " + modeSwitching.ToString());
        //Getting the inputs from each text field and storing them as strings
        forceXString = GUI.TextField(new Rect(875, 10, 200, 20), forceXString, 25);
        forceYString = GUI.TextField(new Rect(875, 30, 200, 20), forceYString, 25);

        //Press the button to reset the GameObject and Rigidbody
        if (GUI.Button(new Rect(650, 0, 150, 30), "Reset"))
        {
            //This switches to the start/reset case
            modeSwitching = ModeSwitching.Start;
        }

        //When you press the Acceleration button, switch to Acceleration mode
        if (GUI.Button(new Rect(650, 30, 150, 30), "Apply Acceleration"))
        {
            //Switch to Acceleration (apply acceleration force to GameObject)
            modeSwitching = ModeSwitching.Acceleration;
        }

        //If you press the Impulse button
        if (GUI.Button(new Rect(650, 60, 150, 30), "Apply Impulse"))
        {
            //Switch to impulse (apply impulse forces to GameObject)
            modeSwitching = ModeSwitching.Impulse;
        }

        //If you press the Force Button, switch to Force state
        if (GUI.Button(new Rect(650, 90, 150, 30), "Apply Force"))
        {
            //Switch to Force (apply force to GameObject)
            modeSwitching = ModeSwitching.Force;
        }

        //Press the button to switch to VelocityChange state
        if (GUI.Button(new Rect(650, 120, 150, 30), "Apply Velocity Change"))
        {
            //Switch to velocity changing
            modeSwitching = ModeSwitching.VelocityChange;
        }
    }

    //Changing strings to floats for the forces
    float ConvertToFloat(string Name)
    {
        float.TryParse(Name, out result);
        return result;
    }

    //Set the converted float from the text fields as the forces to apply to the Rigidbody
    void MakeCustomForce()
    {
        //This converts the strings to floats
        forceX = ConvertToFloat(forceXString);
        forceY = ConvertToFloat(forceYString);
        forceZ = ConvertToFloat(forceZString);
    }

    public Vector3 LandingOnRail(Vector3 velocity)
    {
        //Add initial boost in speed once upon contact with rail
        
        return newForce = velocity;
    }
    public float RailGrindingSpeed(Vector3 velocity, Vector3 previousPosition)
    {
        
        return theSpeed;
    }
    /// <summary>
    /// Velocity is the railGrinder 
    /// nextPosition is the railCar 
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="nextPosition"></param>
    /// <returns></returns>
    public Vector3 RailGrindingVelocity(Vector3 velocity, Vector3 nextPosition)
    {
        forceX = velocity.x;
        forceY = velocity.y; //- velocity.y;
        forceZ = velocity.z;

        //If previous position is greater in height than current position, vector position has declined
        declineSegment = (prevPosition.y > this.transform.position.y); // if (prevPosition.y > this.transform.position.y)
                                                                       //{
                                                                       //    declineSegment = true;
                                                                       //    inclineSegment = false;
                                                                       //}
                                                                       //If previous position is lower in height than current position, vector position has inclined
        inclineSegment = (prevPosition.y < this.transform.position.y);
        //{
        //    declineSegment = false;
        //    inclineSegment = true;
        //}
        return newForce = velocity;
    }

    //public void RailGrindCombatInput(CombatState state)
    //{
    //    combatState = state;
    //    //If state is equal to TechniqueStanceState(Technique Selection, Weapon Charge, Weapon Menu, Etc)
    //    if (state == CombatState.TechniqueStanceState)
    //    {
    //        isBoosting = true;
    //    }
    //    if (state == CombatState.MagicDefensiveStanceState || state == CombatState.MagicOffensiveStanceState)
    //    {

    //    }


    //    if (state == CombatState.TechniqueChargingState)
    //    {
    //        isAutoBalancing = true;
    //    }
    //}
    /// <summary>
    /// Rail Grind Input Method that receives info from Player Controller for Jump and Crouch
    /// Also reads the state of the combat controller to alter speed, rotation, and positioning.  
    /// </summary>
    public void RailGrindInput(bool crouch, bool canJump)
    {
        crouch = isCrouching;

        canJump = isJumping;
        //If crouch input
        if (isCrouching)
        {
            //Increase speed if crouching on decline 
            if (declineSegment || crouchedOnTime)
            {
                SlowlyIncreaseVelocity();
            }
            else if (inclineSegment)
            {
                SlowlyDecreaseVelocity();
            }
            //Decrease speed if crouching on an incline
            else if (inclineSegment && !crouchedOnTime)
            {
                SlowlyDecreaseVelocity();
            }
        }
        //if not crouch input
        if (!isCrouching)
        {
            crouchReleaseTimer += Time.deltaTime;

            if (crouchReleaseTimer <= crouchReleaseTimeLimit)
            {
                crouchedOnTime = true;
                //Increase speed 
            }
            //If crouch release timer is greater than crouchTimeLimit
            else if (crouchReleaseTimer > crouchReleaseTimeLimit)
            {
                //Reset crouch release timer 
                crouchReleaseTimer = 0;
                crouchedOnTime = false;

                //Decrease speed
            }
        }     
    }

    #region Velocity Alterations

    #region Velocity Decrement

    /// <summary>
    /// Decreases rigid body forward velocity by a small amount
    /// </summary>
    private void SlowlyDecreaseVelocity()
    {
        //slowDecrement = 0.01f;

        //acceleration -= slowDecrement;
        
        theSpeed -= acceleration; //forceY -= slowDecrement;
    }

    /// <summary>
    /// Decreases rigid body forward velocity by a moderate amount
    /// </summary>
    //private void SteadilyDecreaseVelocity()
    //{
    //    forwardVelocity -= railDecelerationPerSecond * Time.deltaTime;
    //    //Make forwardVelocity minimum be between its current speed and maxRailSpeed
    //    forwardVelocity = Mathf.Max(minRailSpeed, currentRailSpeed);
    //}

    #endregion

    #region Velocity Increment

    /// <summary>
    /// Increases rigid body forward velocity by a small amount
    /// </summary>
    private void SlowlyIncreaseVelocity()
    {
       // slowIncrement = .01f;

        //acceleration += slowIncrement;

        if (theSpeed < maxRailSpeed)
        {
            theSpeed = theSpeed - acceleration * Time.deltaTime;
        }
      

        //theSpeed = theSpeed - (acceleration * Time.deltaTime); //forceY += slowIncrement;
    }

    /// <summary>
    /// Increases rigid body forward velocity by a moderate amount
    /// </summary>
    //private void SteadilyIncreaseVelocity()
    //{
    //    forwardVelocity += railAccelerationPerSecond * Time.deltaTime;
    //    //Make forwardVelocity minimum be between its current speed and maxRailSpeed
    //    forwardVelocity = Mathf.Min(forwardVelocity, maxRailSpeed);
    //}
    #endregion

    #endregion
    private void LateUpdate()
    {
        rigidBody.velocity = transform.forward * forwardVelocity;

        if (isGrinding)
        {
            if (combatState == CombatState.AttackState)
            {

            }
            if (isBoosting)
            {
               theSpeed += railAccelerationPerSecond;
            }
            if (declineSegment)
            {
                SlowlyIncreaseVelocity();
            }
            if (inclineSegment)
            {
                SlowlyDecreaseVelocity();
            }
        }
        if (rigidBody.velocity.y > maxYVelocity)
        {
            rigidBody.useGravity = true;
        }
        else
        {
            rigidBody.useGravity = false;
        }

        isBoosting = false;
        isCrouching = false;
        crouchedOnTime = false;
        isAutoBalancing = false;
    }
}



