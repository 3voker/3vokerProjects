using System;
using System.Collections;
using UnityEngine;
//using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        public enum CharacterState
        {  CrouchState, JumpState, DoubleJumpState, SprintState, ResearchState, UseItemState,
            };
       
        public CharacterState State
        {
            get { return state; }
            set
            {
                state = value;
                StateChange();
            }
        }
        private CharacterState state;

        protected void StateChange()
        {
            switch (State)
            {        
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
                case CharacterState.ResearchState:
                    //controller.enabled = false;
                    break;
                case CharacterState.UseItemState:
                    //controller.enabled = false;
                    break;
            }
        }
        //IEnumerator WalkState()
        //{
        //    Debug.Log("Walk: Enter");
        //    while (state == State.WalkState)
        //    {
        //        yield return 0;
        //    }
        //    Debug.Log("Walk: Exit");
        //    NextState();
        //}
        public bool walkByDefault = false; // toggle for walking state
        public bool sprintByDefault = false; //NEW feature, toggle sprint feature
        public bool lookInCameraDirection = true;// should the character be looking in the same direction that the camera is facing        
        // public int sprintSpeed; //sprint speed.
        Vector3 lookPos; // The position that the character should be looking towards
        ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        Transform cam; // A reference to the main camera in the scenes transform
        Vector3 camForward; // The current forward direction of the camera
        Vector3 camLeftRight;
        Vector3 move;    
        bool isMoving; 
        bool jump;
        bool doubleJump;
        bool canEvade = true;
        float evadeTimer = 1;
        bool pauseMenu = false;
        bool mapMenu = false;
        bool inventoryMenu = false;
        float spellChargeTimer = 6.15f;
        bool spellCharge = false;
        float TechniqueChargeTimer = 3f;
        bool techniqueCharge = false;
        bool hasMP = true;
        private bool Fire1;
        private bool Fire2;
        private bool Fire3;
      
        void Start()
        { 
            if (Camera.main != null)
            {
                cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");                
            }               
            character = GetComponent<ThirdPersonCharacter>();
        }
        void Update()
        {
            if (!jump)
            {
                jump = (Input.GetButton("Jump"));
                if (jump)
                {
                    if (jump && !doubleJump)
                    {
                        jump = true;
                        doubleJump = (Input.GetButton("Jump"));                      
                    }
                }
            }
        }          
        private void MagicDefensiveStanceState()
        {
            Debug.Log("Activated MagicDefensiveStance");
            if (Fire1)
            {
                Debug.Log("Cast magicSpellX");
                spellCharge = true;
                spellChargeTimer--;
            }
            else if (Fire2)
            {
                Debug.Log("Cast magicSpellY");
                spellCharge = true;
                spellChargeTimer--;
            }
            else if (Fire3)
            {
                Debug.Log("Cast magicSpellB");
                spellCharge = true;
                spellChargeTimer--;
            }
            else
                spellCharge = false;
            spellChargeTimer = 6.15f;
        }
        private void MagicOffensiveStanceState()
        {
            Debug.Log("Activated MagicOffensiveStance");
            if (Fire1)
            {
                Debug.Log("Cast magicSpellX");
                spellCharge = true;
                spellChargeTimer--;
            }
            else if (Fire2)
            {
                Debug.Log("Cast magicSpellY");
                spellCharge = true;
                spellChargeTimer--;
            }
            else if (Fire3)
            {
                Debug.Log("Cast magicSpellB");
                spellCharge = true;
                spellChargeTimer--;
            }
            else
                spellCharge = false;
                spellChargeTimer = 6.15f;
        }
        private void DefensiveStanceState()
        {       
          Debug.Log("Activated DefensiveStance");
            if (canEvade)
            {
                if (Input.GetAxis("leftJoystickHorizontal") > .95f)
                {
                    Debug.Log("Evasive Manuever to the right");
                    canEvade = false;
                }
                if (Input.GetAxis("leftJoystickHorizontal") < -.95f)
                {
                    Debug.Log("Evasive Manuever to the left");
                    canEvade = false;
                }
                if (Input.GetAxis("leftJoystickVertical") > .95f)
                {
                    Debug.Log("Evasive Manuever forward roll");
                    canEvade = false;
                }
                if (Input.GetAxis("leftJoystickVertical") < -.95f)
                {
                    Debug.Log("Evasive Manuever to the back roll");
                    canEvade = false;
                }
                if (Input.GetAxis("rightTrigger") > 0)
                {
                    Debug.Log("Technique is charging while crouched");
                }
            }
        }    
        private void TechniqueStanceState()
        {
            Debug.Log("Activated TechniqueStance");           
        }
        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {        
            bool crouch = false;
            bool sprint = false;
            bool attack = false;
            
            bool hurt = false;
            bool dead = false;           
                 
            Fire1 = Input.GetButton("xButton");
            Fire2 = Input.GetButton("yButton");
            Fire3 = Input.GetButton("bButton");
            float h = Input.GetAxis("leftJoystickHorizontal");          
            float v = Input.GetAxis("leftJoystickVertical");
            crouch = (Input.GetAxisRaw("leftTrigger") != 0);

            bool magicDefensiveStance = Input.GetButton("leftBumper");
            bool magicOffensiveStance = Input.GetButton("rightBumper");
            bool defensiveStance = (Input.GetAxisRaw("leftTrigger") != 0);
            bool techniqueStance = (Input.GetAxisRaw("rightTrigger") != 0);
            bool walkToggle = Input.GetKey(KeyCode.LeftShift);

            if (magicDefensiveStance)
            {                           
                MagicDefensiveStanceState();             
            }
            if (magicOffensiveStance)
            {            
                MagicOffensiveStanceState();
            }
           if (defensiveStance)
            {             
                    // Call your event function here.
                    DefensiveStanceState();                                
            }
            if (techniqueStance)
            {           
                    TechniqueStanceState();                        
            }
            if (!mapMenu)
            {
                mapMenu = Input.GetButtonUp("yButton");
                if (mapMenu)
                {
                    ResearchState();
                }
            }
            if (cam != null)
            {
                // calculate camera relative direction to move:
                camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;         
                move = v * camForward + h * cam.right; // cam.forward;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                move = v * Vector3.forward + h * Vector3.right;
            }
            // if (move.magnitude > 1) move.Normalize();
#if !MOBILE_INPUT
            // On non-mobile builds, walk/run speed is modified by a key press.          
            //Testing Sprint feature         
            // We select appropriate speed based on whether we're walking by default, and whether the walk/run toggle button is pressed:
            float walkMultiplier = (walkByDefault ? walkToggle ? 1 : 0.5f : walkToggle ? 0.5f : 1);
            move *= walkMultiplier;
            bool sprintToggle = (Input.GetAxis("leftJoystickHorizontal")> .90f) ;
            float sprintMultiplier = (sprintByDefault ? sprintToggle ? 1 : 3f : sprintToggle ? 3f : 1);
            move *= sprintMultiplier;
            if (sprintToggle)
            {
                Debug.Log("Sprinting.");
            }
            else
            {
               // Debug.Log("Not sprinting");
            }
#endif
            // calculate the head look target position
            lookPos = lookInCameraDirection && cam != null 
            ? transform.position + cam.forward*25 : transform.position + transform.forward*25; //100
            // pass all parameters to the character control script
            character.Move(move, crouch, jump, lookPos, doubleJump, sprint);
            jump = false;
            doubleJump = false;
        }
        private void WalkState()
        {
            throw new NotImplementedException();
        }
        private void ResearchState()
        {
            bool exitMenu = Input.GetButton("bButton");
            Debug.Log("Entered Research State");
            if (exitMenu)
            {
                mapMenu = false;
                Debug.Log("Exit Research State");
            }
            StateChange();
        }  
    }
}