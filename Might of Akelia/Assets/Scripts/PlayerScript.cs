using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class PlayerScript : MonoBehaviour {

    // Use this for initialization

    SpellManager spellManager;


    [SerializeField]
    GameObject player;
    [SerializeField]
    float speed;
    [SerializeField]
    float rotationalSpeed;
    [SerializeField]
    Vector3 jumpHeight;
    [SerializeField]
    float jumpSpeed;


    Vector3 movement;
    Rigidbody rigidBody;

    public struct SpellStruct
    {
        public int Spell;
    }

    //public class PointClass
    //{
    //    public int X;
    //}
    public void TestStruct()
    {

        var cureSpellStructArray = new SpellStruct[2];
        var spellManagerArray = new SpellManager[2];

        int spell;

        spell = cureSpellStructArray[0].Spell;

        try
        {
           spell = spellManagerArray[0].Spell;
        }
        catch (System.NullReferenceException)
        {
            Debug.Log("Null Reference");
        }

        spellManagerArray[0] = new SpellManager();

        // It's now ok
        spell = spellManagerArray[0].Spell;

        var spell1 = cureSpellStructArray[0];
        var spell2 = spellManagerArray[0];

        spell1.Spell = 1;
        spell2.Spell = 1;

        Assert.IsTrue(spell2.Spell == 1);
        Assert.IsFalse(cureSpellStructArray[0].Spell == 1);

       cureSpellStructArray[0].Spell = 1;
        Assert.IsTrue(cureSpellStructArray[0].Spell == 1);
    }
    void Start () {
        
        rigidBody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("leftJoystickHorizontal") * rotationalSpeed * Time.deltaTime; ;
       

        float moveVertical = Input.GetAxis("leftJoystickVertical") * speed * Time.deltaTime; ;
        
    
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        rigidBody.AddForce(movement * speed * Time.deltaTime);
        playerInput();
    }

    private void playerInput()
    {
        if(Input.GetButton("aButton"))
        {
            transform.position += transform.up * jumpSpeed * Time.deltaTime;
            Debug.Log("aButton was pressed!");
        }
        else if (Input.GetButton("bButton"))
        {
            //Trying to call spell from spell manager using struct to play animation on button press. Didn't work. 
            //spellManager = GetComponent<SpellManager>();
            //spellManager.CastSpell(0);
            //spellManager.CastSpell(1);
                
            Debug.Log("bButton was pressed!");
        }
        else if (Input.GetButton("xButton"))
        {
            Debug.Log("xButton was pressed!");
        }
        else if (Input.GetButton("yButton"))
        {
            Debug.Log("yButton was pressed!");
        }
        else if (Input.GetButton("leftBumper"))
        {
            Debug.Log("leftBumper was pressed!");
        }
        else if (Input.GetButton("rightBumper"))
        {
            Debug.Log("rightBumper was pressed!");
        }
        else if (Input.GetButton("backButton"))
        {
            Debug.Log("backButton was pressed!");
        }
        else if (Input.GetButton("startButton"))
        {
            Debug.Log("startButton was pressed!");
        }
        else if (Input.GetAxis("leftTrigger") > 0)
        {
            Debug.Log("leftTrigger was pressed!");
        }
        else if (Input.GetAxis("rightTrigger") > 0)
        {
            Debug.Log("rightTrigger was pressed!");
        }
    }


//        if ()
//        {
//            //Use for potential bool confirm.
//            //meshRenderer.enabled = Input.GetButton(buttonName);
//        }
//        else if ()
//        {
//            //Triggers operate differently than GetButton, require numerical float value 
//            if ((Input.GetAxis("leftTrigger") > 0))
//            {
               
//            }
//            else
//            {
              
//            }
//        }
//        else if ()
//        {
//            //Triggers operate differently than GetButton, require numerical float value 
//            if ((Input.GetAxis("rightTrigger") > 0))
//            {

              
//            }
//            else
//            {
              
//            }
//        }

//        else if (isDpad)
//        {
//            //meshRenderer.enabled = Input.GetButton(buttonName);
//            Vector3 inputDirection = Vector3.zero;
//            inputDirection.x = Input.GetAxis("dPadHorizontal");
//            inputDirection.y = Input.GetAxis("dPadVertical");
//            this.transform.position = startPos - inputDirection;
//        }
//        else
//        {
//            if (leftJoystick)
//            {
//                Vector3 inputDirection = Vector3.zero;
//                inputDirection.x = Input.GetAxis("leftJoystickHorizontal");
//                inputDirection.y = Input.GetAxis("leftJoystickVertical");
               
//            }
//            else if (!leftJoystick)
//            {
//                Vector3 inputDirection = Vector3.zero;
//                inputDirection.x = Input.GetAxis("rightJoystickHorizontal");
//                inputDirection.y = Input.GetAxis("rightJoystickVertical");
              
//            }
//        }
//    }
//}

}
