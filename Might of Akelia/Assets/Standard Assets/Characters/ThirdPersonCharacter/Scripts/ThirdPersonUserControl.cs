using System;
using UnityEngine;
//using UnitySampleAssets.CrossPlatformInput;

namespace UnitySampleAssets.Characters.ThirdPerson
{

    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {

        public bool walkByDefault = false; // toggle for walking state
        public bool sprintByDefault = false; //NEW feature, toggle sprint feature
        public bool lookInCameraDirection = true;// should the character be looking in the same direction that the camera is facing        
        // public int sprintSpeed; //sprint speed.
        private Vector3 lookPos; // The position that the character should be looking towards
        private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        private Transform cam; // A reference to the main camera in the scenes transform
        private Vector3 camForward; // The current forward direction of the camera
        private Vector3 move;    
        private bool isMoving;
        private bool jump;
        private bool doubleJump;
        // the world-relative desired move direction, calculated from the camForward and user input.

        // Use this for initialization
        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            character = GetComponent<ThirdPersonCharacter>();
        }
        void Update()
        {

            if (!jump)
            {
                jump = (Input.GetButton("Jump"));
            }

            if(jump && !doubleJump)
            {
                doubleJump = (Input.GetButton("Jump"));
            }
                
        }
        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
       //     movement();
            bool crouch = false;
            bool sprint = false;

            // float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float h = Input.GetAxis("leftJoystickHorizontal");
            float v = Input.GetAxis("leftJoystickVertical");
            //float v = CrossPlatformInputManager.GetAxis("Vertical");
            crouch = (Input.GetButton("bButton"));
            

            // calculate move direction to pass to character
            if (cam != null)
            {
                // calculate camera relative direction to move:
                camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
                move = v*camForward + h*cam.forward;            
            }
            else 
            {
                // we use world-relative directions in the case of no main camera
                move = v * Vector3.forward + h * Vector3.right;
            }
           // if (move.magnitude > 1) move.Normalize();
#if !MOBILE_INPUT
            // On non-mobile builds, walk/run speed is modified by a key press.
            bool walkToggle = (Input.GetButton("leftBumper"));
            //Testing Sprint feature
            
            // We select appropriate speed based on whether we're walking by default, and whether the walk/run toggle button is pressed:
            float walkMultiplier = (walkByDefault ? walkToggle ? 1 : 0.5f : walkToggle ? 0.5f : 1);
            move *= walkMultiplier;

            bool sprintToggle = (Input.GetButton("rightBumper"));

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
                          ? transform.position + cam.forward*100
                          : transform.position + transform.forward*100;

            // pass all parameters to the character control script
            character.Move(move, crouch, jump, lookPos, doubleJump, sprint);
            jump = false;
            doubleJump = false;
        }
    }
}