using UnityEngine;
using System.Collections;

public abstract class EnemyController : MonoBehaviour
{
    private PlayerInteraction player;

    public bool canMove = true;
    public float stoppingDistance = 0f;
    public float movementSpeed = 5f;
    public bool AtDestination { get; set; }
    public bool inRangeOfPlayer { get; set; }

    [HideInInspector]public Vector3 destination;
    private BoxCollider boxCollider;

    public float wanderingRaidus = 5;
    public float sightRadius = 5;
    public int m_EnemyMovementNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EnemyIdling;
    public AudioClip m_EnemyCharging;
    public float m_PitchRange = 0.2f;


    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;

    [Header("Other")]
    [Tooltip("Rigidbody Components of the turret")]
    public Rigidbody[] _Rigidbodies;
    [Tooltip("Mesh Colliders of the turret")]
    public MeshCollider[] _MeshColliders;
    //public TurretRotation _Rotation;
    //public RaycastHandler _Raycast;
    //public ShootingSystem _Shooting;
    public EnemyHealth _Health;
    public AudioHandler _Audio;
    public ShootingSystem _Shooting;
    public RotationSystem _Rotation;
    public RaycastHandler _Raycast;

    #region UnityFunctions
    void Awake()
    {
        _Rotation = this.GetComponent<RotationSystem>();
        _Raycast = this.GetComponent<RaycastHandler>();
        _Shooting = this.GetComponent<ShootingSystem>();
        _Health = this.GetComponent<EnemyHealth>();
        _Audio = this.GetComponent<AudioHandler>();
        OnAwake();
    }
  
    #endregion
    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }
    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }  
    private void EnemyAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EnemyCharging)
            {
                m_MovementAudio.clip = m_EnemyIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            if (m_MovementAudio.clip == m_EnemyIdling)
            {
                m_MovementAudio.clip = m_EnemyCharging;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }
    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }
    private void Turn()
    {
        // Adjust the rotation of the enemy based on the player's input.
        //Adjust rotation speed of enemy based on enemy level*Add soon*     
        float turn = m_TurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
    protected virtual void OnAwake()
    {

        m_Rigidbody = GetComponent<Rigidbody>();    
        #region Set sight raidus size
        BoxCollider[] colliders = this.GetComponents<BoxCollider>();
        if(colliders.Length > 1)
        {
            boxCollider = System.Array.Find(colliders, item => item.isTrigger);
            boxCollider.size = new Vector3(sightRadius, boxCollider.size.y, sightRadius);
        }
        else if (colliders.Length == 1)
        {
            boxCollider = colliders[0];
            boxCollider.size = new Vector3(sightRadius, boxCollider.size.y, sightRadius);
        }
        else
        {
            Debug.LogError("Enemy has no box collider -- can't set trigger size");
        }
        #endregion

        player = GameObject.FindObjectOfType<PlayerInteraction>();      
    }
    protected virtual void OnStart()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
        destination = RandomDestination();
    }
    protected virtual void OnUpdate()
    {
        CheckDistance();
        EnemyAudio();
        if (inRangeOfPlayer && player != null) { this.destination = player.gameObject.transform.position; }

        if (canMove)
        {
            Wandering();
            MoveToDestination();
        }
        else
        {
            LookAtDestination();
        }
    }
    protected virtual void MoveToDestination()
    {

    }
    protected virtual void CheckDistance()
    {

    }
    public void LookAtDestination()
    {
        if (destination != null)
            this.transform.LookAt(destination);
    }
    public void ToggleMovement(bool enabled)
    {
        canMove = enabled;
    }
    public void SetStoppingDistance(float dist)
    {
        stoppingDistance = dist;
    }
    protected void Wandering()
    {
        if (!inRangeOfPlayer)
        {
            if (AtDestination)
            {
                destination = RandomDestination();
            }
        }
    }
    protected Vector3 RandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderingRaidus;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, wanderingRaidus, 1);
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<PlayerInteraction>() != null)
        {
            inRangeOfPlayer = true;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        //if (collider.GetComponent<Player>() != null)
        //{
        //    inRangeOfPlayer = false;
        //    this.destination = RandomDestination();
        //}
    }
    void OnDrawGizmos()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(destination, 1);
    }
    public void isKinematicRigidbodies(bool val)
    {

        for (int i = 0; i < _Rigidbodies.Length; i++)
        {

            _Rigidbodies[i].isKinematic = val;
        }
    }
    //to enable/disable Mesh Colliders
    public void MeshCollider_Status(bool val)
    {

        for (int i = 0; i < _MeshColliders.Length; i++)
        {

            _MeshColliders[i].enabled = val;
        }
    }
}
