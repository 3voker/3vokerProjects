using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    #region Editor variables
    [SerializeField]
    Text respawnText;
    [SerializeField]
    float groundDetectRadius = 0.2f;
    [SerializeField]
    Transform groundDetectPoint;
    [SerializeField]
    LayerMask whatIsGround;
    [SerializeField]
    float moveSpeed = 5;
    [SerializeField]
    float jumpVelocity = 5;
    [SerializeField]
    public GameObject bomb;
    [SerializeField]
    GameObject startingPosition;

    Animator anim;
    int jumpHash = Animator.StringToHash("Jump");
    #endregion

    #region Properties
    public Checkpoint LastCheckPoint { get; set; }
    #endregion

    public Checkpoint StartingPoint { get; set; }

    #region Private fields
    Rigidbody2D rigidBody2D;
    bool isOnGround;
    bool isAlive;
    #endregion

    //void OnCollisionEnter2D(Collision2D coll)
    //{   
    //    Debug.Log("IsOnGround?");
    //}


    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();

        respawnText.gameObject.SetActive(false);
       rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        isAlive = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("Is Alive" + isAlive);
        float move = Input.GetAxis("Vertical");
        anim.SetFloat("Speed", move); 

        if (isAlive)
        {
            HandleMovementInput();
            UpdateIsOnGround();
            HandleJumpInput();
            HandleAttackInput();
            if (this.moveSpeed > 0)
            {
               // transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                GetComponent<SpriteRenderer>();
                //anim.SetBool("IsGrounded", _controller.State.IsGrounded);
            }
        }
        else
        {
            respawnText.gameObject.SetActive(true);
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger(jumpHash);
                Respawn();
                respawnText.gameObject.SetActive(false);
            }
        }
        
    }

    private void HandleAttackInput()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Weapon Fired");
            Instantiate(bomb, (Vector2)transform.position, Quaternion.identity);
            anim.SetTrigger("Fire");
        }
    }

    private void UpdateIsOnGround()
    {
       Collider2D[] groundObjects =  
            Physics2D.OverlapCircleAll(groundDetectPoint.position, groundDetectRadius, whatIsGround);
        isOnGround = groundObjects.Length > 0;
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            float currentXVelocity = rigidBody2D.velocity.x;
            Vector2 velocityToSet = new Vector2(currentXVelocity, jumpVelocity);

            rigidBody2D.velocity = velocityToSet;
        }
    }

    private void HandleMovementInput()
    {        
        float movementInput = Input.GetAxis("Horizontal");
        float currentYVelocity = rigidBody2D.velocity.y;

        Vector2 velocityToSet = new Vector2(moveSpeed * movementInput * Time.deltaTime, currentYVelocity);
        rigidBody2D.velocity = velocityToSet;

        

        
    }


    public void TakeDamage()
    {
        isAlive = false;

    }

    public void Respawn ()
    {
        isAlive = true;
        if (LastCheckPoint == null)
        {
            transform.position = startingPosition.transform.position;
        }
        else
        {
            transform.position = LastCheckPoint.transform.position;

        }
    }
}
