using UnityEngine;
using System.Collections;
using System;

public class PlayerFlightControl : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    float m_Speed = 12f;
    [SerializeField]
    float m_TurnSpeed = 90f;

    [SerializeField]
    float planeSpeed; 
    float m_SlowTurnSpeed = 10f;

    const float maxSpeed = 150f;
    const float minSpeed = 1f; 
    [SerializeField]
    AudioSource m_MovementAudio;
    [SerializeField]
     AudioClip m_EngineIdling;
    [SerializeField]
    AudioClip m_EngineDriving;

    //Variables private
    Vector3 eulerAngleVelocity;

    Rigidbody m_Rigidbody;

   
    const float Z_ANGLE_MIN = -15F;
    const float Z_ANGLE_MAX = 15F;
    string m_MovementAxisName;
    string m_TurnAxisName;


    //Pitch, Roll, Yaw
    //Pitch is tilt Up/Down (Y axis)  || Vertical refactor to Pitch
    //Yaw is moving forward/back (Z axis  || Movement refactor to Yaw
    //Roll is tilting left/right (X axis) || Turn refactor to Roll
    float m_HorizontalInputValue;
    float m_VerticalInputValue;
    float m_OriginalPitch;
    
    //mvem
    /// <summary>
    //
    /// </summary>
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        //m_movementInput
        m_HorizontalInputValue = 0f;
        //m_turnInput
        m_VerticalInputValue = 0f;
    }
    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update () {
        m_HorizontalInputValue = Input.GetAxis("Horizontal");
        m_VerticalInputValue = Input.GetAxis("Vertical");

    }
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        m_Rigidbody = GetComponent<Rigidbody>();
        //rb is now m_Rigidbody
       
        Roll();  //Roll is tilting left/right (X axis)
        Pitch(); //Pitch is tilt Up/Down (Y axis)
        Yaw();  //Yaw is moving forward/back (Z axis

        AutoRotate();
        AutoMovement();
    }

    private void AutoMovement()
    {
        bool gas = Input.GetButtonDown("Jump");
            if (Input.GetButtonDown("Jump"))           
            {
                planeSpeed += Time.deltaTime;             
                transform.Translate(Vector3.forward * planeSpeed * Time.deltaTime);
                transform.Translate(Vector3.up * planeSpeed * Time.deltaTime);
            if (planeSpeed > maxSpeed)
            {
                planeSpeed = 149.9f;
            }
        }
                
        bool brake = Input.GetButtonUp("Jump");
        if (Input.GetButtonUp("Jump"))
        {
            planeSpeed -= Time.deltaTime;
            transform.Translate(Vector3.forward * planeSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * m_SlowTurnSpeed * Time.deltaTime);
            if(planeSpeed < minSpeed)
            {
                planeSpeed = 1f;
            }
        }
        transform.Translate(Vector3.forward * m_Speed * Time.deltaTime);
        transform.Translate(Vector3.up * m_Speed * Time.deltaTime);
    }

    private void AutoRotate()
    {
        Quaternion balancedRotation = Quaternion.Euler(0f, 0f, 0f);
        //  if(currentRotation != balancedRotation)
        //{ if (playerMovementInput == 0)}
    }
    private void Roll()
    {
        // Adjust the rotation of the tank based on the player's input.
        //moveHorizonal is now roll 
        float roll = m_VerticalInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, 0f, roll);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
    private void Pitch()
    {
        //moveHorizonal is now pitch
       
        float pitch = m_VerticalInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, pitch, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        transform.Translate(Vector3.up * m_SlowTurnSpeed * Time.deltaTime);
    }

    private void Yaw()
    {
        //moveHorizonal is now yaw
        //float currentZ;
        float yaw = m_HorizontalInputValue * m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, 0f, yaw);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        yaw = Mathf.Clamp(transform.rotation.z, Z_ANGLE_MIN, Z_ANGLE_MAX);
      
        // Vector3 movement = transform.forward * m_VerticalInputValue * m_Speed * Time.deltaTime;
    }
}
