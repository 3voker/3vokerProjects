using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState
{
    IdleCombatState, DieState, AttackState, TakenDamageState, KnockedBackState, EvadeState,
    SpellCastingState, ItemPreparingState, TechniqueChargingState, ShootingState, 
    ThrowingState, MagicDefensiveStanceState,
    MagicOffensiveStanceState, CheckingInventoryState, DefensiveStanceState, TechniqueStanceState, 
};
[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(ThirdPersonPlayerCharacter))]
public class CombatController : Rook
{
    #region Variables 
    #region Script References
    /// <summary>
    /// A reference to the player Controller script on this gameobject
    /// </summary>
    public PlayerController playerController { get; protected set; }

    /// <summary>
    /// A reference to the third Person Player Character script on this gameobject
    /// </summary>
    private ThirdPersonPlayerCharacter thirdPersonPlayerCharacter;

    /// <summary>
    /// A reference to the player Interaction script on this gameobject
    /// </summary>
    private PlayerInteraction playerInteraction;

    /// <summary>
    /// A reference to the user Input Manager script on this gameobject
    /// </summary>
    private UserInputManager userInputManager;

    /// <summary>
    /// A reference to the menu Manager script on this gameobject
    /// </summary>
    private MenuManager menuManager;

    /// <summary>
    /// A reference to the enemy Controller script on this gameobject
    /// </summary>
    private EnemyController enemyController;

    /// <summary>
    /// A reference to the player Control Velocity script on the player gameobject
    /// </summary>
    private ControlVelocity playerControlVelocity;

    /// <summary>
    /// A reference to the enemy Control Velocity script on the enemy gameobject
    /// </summary>
    private ControlVelocity enemyControlVelocity;

    /// <summary>
    /// A reference to the player Control Velocity script on the railBalancer gameobject
    /// </summary>
  //  private ControlVelocity railBalancerVelocity;

    /// <summary>
    /// A reference to the rail Balancer gameobject for trecking
    /// </summary>
 //   public GameObject railBalancer;

    /// <summary>
    /// A reference to the rook script on this gameobject
    /// </summary>
    private Rook thisRook;

    /// <summary>
    /// A reference to the rook script on an enemy gameobject
    /// </summary>
    private Rook enemyRook; 

    /// <summary>
    /// A reference to the _Health script on this gameobject
    /// </summary>
    public Rook _Health;

    /// <summary>
    /// A reference to the _Audio script on this gameobject
    /// </summary>
    public AudioHandler _Audio;

    /// <summary>
    /// A reference to the _Shooting script on this gameobject
    /// </summary>
    public ShootingSystem _Shooting;

    /// <summary>
    /// A reference to the _Rotation script on this gameobject
    /// </summary>
    public RotationSystem _Rotation;

    /// <summary>
    /// A reference to the raycast handler script on this gameobject
    /// </summary>
    public RaycastHandler _Raycast;
    #endregion

    #region Keypad References
    [Header("Keyboard Keys")]
    public KeyCode attackKBKey;
    public KeyCode blockKBKey;
    [Header("Gamepad Keys")]
    public GamepadButtons attackGPKey;
    public GamepadButtons blockGPKey;
    #endregion

    /// <summary>
    /// Toggles if gameObject can auto rotate towards target
    /// </summary>
    public bool autoRotate = true;
    /// <summary>
    /// Transform that acts as an aim cursor for player
    /// </summary>
    [Tooltip("Player should rotate to aim at the target")]
    public Transform aimPoint;
    /// <summary>
    /// Rotation speed while firing long ranged attacks
    /// </summary>
    [Tooltip("Rotation speed of the Turret")]
    public float rotationSpeed = 1;

    #region Target References
    /// <summary>
    /// References GameObject with Rook Script that is current target for attacking
    /// </summary>
    [SerializeField]
    private Rook target;
    /// <summary>
    /// References GameObject with Rook Script that is current main Target
    /// </summary>
    public Rook Target { get { return target; } }
    /// <summary>
    /// References GameObject with Rook Script that is current target for attacking
    /// </summary>
    protected Rook targetToAttack { get { return target; } }
    #endregion

    /// <summary>
    /// Minimal speed GameObject can travel on rail
    /// </summary>
    public List<CombatController> targetsAttackingThis = new List<CombatController>();

    /// <summary>
    /// Number of times combatant can block
    /// </summary>
    private int blockHitTimes;
    /// <summary>
    /// Toggles whether combatant can Attack
    /// </summary>
    [HideInInspector]
    public bool canAttack;

    /// <summary>
    /// Toggles whether player has weapon
    /// </summary>
    public bool hasWeapon;

    #region Input References

    /// <summary>
    /// Toggles whether player X(Fire1)
    /// </summary>
    private bool Fire1;

    /// <summary>
    /// Toggles whether player inputs Y(Fire2)
    /// </summary>
    private bool Fire2;

    /// <summary>
    /// Toggles whether player inputs B(Fire3)
    /// </summary>
    private bool Fire3;

    /// <summary>
    /// Toggles whether player inputs LB
    /// </summary>
    bool magicDefensiveStance;

    /// <summary>
    /// Toggles whether player inputs RB
    /// </summary>
    bool magicOffensiveStance;

    /// <summary>
    /// Toggles whether player inputs LT
    /// </summary>
    bool defensiveStance;

    /// <summary>
    /// Toggles player is blocking by pressing LT 
    /// </summary>
    bool isBlocking;
    /// <summary>
    /// Toggles player is grabbing by pressing LT and X
    /// </summary>
    bool isGrabbing;

    /// <summary>
    /// Toggles player is tackling by pressing LT and Y
    /// </summary>
    bool isTackling;
    /// <summary>
    /// Toggles player is evading by pressing LT and directional input
    /// </summary>
    bool isEvading;
    /// <summary>
    /// Toggles whether player inputs RT
    /// </summary>
    bool techniqueStance;
    /// <summary>
    /// Toggles player is parrying by pressing RT 
    /// May add window timer to countdown before isParrying turns false
    /// </summary>
    bool isParrying;
    /// <summary>
    /// Toggles whether player is trecking and attacking(Attack, Spell/Technique Release, Shooting,)(Think immediate release, return to normal)
    /// </summary>
    public bool isTreckingAndAttacking;

    /// <summary>
    /// Use this to open menus in main menu manager 
    /// </summary>
    public bool menuToggle;


    #region Timers
    /// <summary>
    /// Float that decreases in time depending on how long any combat stance is held for longer than 6.5 seconds. 
    /// </summary>
    [SerializeField]
    public float menuActivationTimer = 6.2f;

    /// <summary>
    /// Float that decreases while rook casts spell
    /// Rook will automatically release spell once timer reaches 0
    /// Waiting until 0 increases spells magic attack, magic attack speed, magic attack defense, magic defense, and magic accuracy  
    /// </summary>
    [Header("Magic Variables")]
    public float spellCastTimer = 6.15f;

    /// <summary>
    /// Toggles whether player is spell casting (Cast Time)
    /// </summary>
    protected bool isCastingSpell = false;

    /// <summary>
    /// Float that decreases while rook charges technique
    /// Rook will automatically release technique once timer reaches 0
    /// Waiting until 0 increases technique damage output, increase parry overpowering effect, increases accuracy
    /// </summary>
    protected float TechniqueChargeTimer = 3f;

    /// <summary>
    /// Toggles whether player is charging a technique charging(Charge Time)
    /// </summary>
    protected bool isChargingTechnique = false;

    /// <summary>
    ///Toggles whether player is preparing an item(Prep Time)
    /// </summary>
    protected bool isPreparingItem;

    /// <summary>
    /// Float that decreases while rook preps item
    /// Rook will automatically use item once timer reaches 0
    /// Waiting until 0 on itemPrepTimer increases item effectiveness, reduces 'DUD' items, or chance of rook dropping item
    /// </summary>
    protected float itemPrepTimer = 3f;
    #endregion

    #endregion


    #region CombatStateChange Variables
    [SerializeField]
    private CombatState combatState;
    private CombatState previousState;
    public CombatState CurrentCombatState
    {
        get { return combatState; } //combatState
        set
        {
            previousState = combatState;
            combatState = value;
            CombatStateChanged(value);
        }
    }
    /// <summary>
    /// CombatState Change Machine
    /// </summary>
    protected CombatState CombatStateChanged(CombatState State)
    {
        switch (CurrentCombatState)
        {
            case CombatState.IdleCombatState:
                //controller.enabled = true;
                // PlayCombatNothingAnimation();

                return previousState = State;

            case CombatState.DieState:
                {
                   // DieState();
                }
                //controller.enabled = false;
                return previousState = State;
            case CombatState.AttackState:
                {
                    //Timed attacks can lead parry enemy attacks. 
                    //Can parry projectiles                               
                   // AttackState();
                }
                // controller.enabled = true;
                return previousState = State;
            case CombatState.EvadeState:
                {
                    //While evading physical/magical dmg that hits fighter is reduced by 1/4th
                    //Critical hits and multihits deal more damage however.
                 
                    //EvadeState();
                }
                return previousState = State;
            case CombatState.TakenDamageState:
                {
                   // TakenDamageState();
                }
                //controller.enabled = true;
                return previousState = State;
            case CombatState.SpellCastingState:
                {
                    //•	LB – Support magic stance gradually restores HP for self but is 10 % weaker to 
                    //physical knockback while casting.                    
                    //thisRook.CastingState();
                }
                //controller.enabled = false;
                return previousState = State;
            case CombatState.ShootingState:
                {
                    //Can parry other projectiles
                    //fighter is 5% weaker to projectiles, magic, and attacks while shooting.                                  
                    //this.ShootingState();
                }
                //controller.enabled = false;
                return previousState = State;
            case CombatState.TechniqueChargingState:
                {
                    //thisRook.TechniqueState();
                }
                //controller.enabled = false;
                return previousState = State;
            case CombatState.ThrowingState:
                {
                    //Can parry projectiles and attacks
                    //Fighter is 5% weaker to projectiles, magic, and attacks while using items.          
                    //thisRook.ThrowingState();
                }
                //controller.enabled = true;
                return previousState = State;
            case CombatState.MagicDefensiveStanceState:
                {
                    //•	LB – Support magic stance gradually restores HP for self
                    //is 10 % weaker to physical attacks while holding this button down.                   
                    //thisRook.SupportMagicStance();
                }
                //controller.enabled = false;
                return previousState = State;
            case CombatState.MagicOffensiveStanceState:
                {
                    //•	RB – Offensive magic stance gradually restores MP for self
                    //is 10 % weaker to physical attacks while holding this button down.
                   // thisRook.OffensiveMagicStance();
                }
                //controller.enabled = false;
                return previousState = State;
            case CombatState.DefensiveStanceState:
                //•	LT – Defensive stance drastically lowers physical damage taken
                //is 10 % weaker to magical attacks while holding this button down.                   
                //thisRook.DefensiveStance();
                //controller.enabled = false;
                // PlayBlockAnimation();
                return previousState = State;
            case CombatState.TechniqueStanceState:
                {
                    //•	RT – Technique stance gradually restores SP for self 
                    //is 10 % weaker to magical attacks while holding this button down.                 
                   // thisRook.TechniqueStance();
                }
                //controller.enabled = false;
                return previousState = State;
        }
        return previousState;
    }
    #endregion

    #region SpellSystem
    /// <summary>
    /// All spells available in the array
    /// </summary>
    public Spell[] AllSpells;
    /// <summary>
    /// All spells available to player
    /// </summary>

    public Spell[] PlayerSpells;

    #endregion
    /// <summary>
    ///Gui Texture 
    /// </summary>
    [SerializeField]
    public Texture texture;

    #endregion
    private void Start()
    {

        menuManager = GameObject.FindObjectOfType<MenuManager>();
        playerInteraction = this.GetComponent<PlayerInteraction>();
        playerController = GetComponent<PlayerController>();
        thirdPersonPlayerCharacter = GetComponent<ThirdPersonPlayerCharacter>();

        //The rook that is operating on this gameObject
        thisRook = this.GetComponent<Rook>();
        //railBalancerVelocity = railBalancer.GetComponent<ControlVelocity>();
        //userInputManager = GameObject.FindGameObjectWithTag
        

        //Any other rook that is being acted upon. Probably simply check for rook on interaction based on tag or layer before assigning enemyRook
        //enemyRook = this.GetComponent<Rook>();
    }
    void Awake()
    {
        //Get Spell Info interface for slot 0 
        PlayerSpells[0].SpellID = AllSpells[0].SpellID;
        PlayerSpells[0].Icon = AllSpells[0].Icon;
        PlayerSpells[0].SpellName = AllSpells[0].SpellName;
        PlayerSpells[0].ManaCost = AllSpells[0].ManaCost;
        PlayerSpells[0].Element = AllSpells[0].Element;
        PlayerSpells[0].Description = AllSpells[0].Description;

        //Spell parameters for slot 0 
        PlayerSpells[0].SpellCategory = AllSpells[0].SpellCategory;
        PlayerSpells[0].SpellRange = AllSpells[0].SpellRange;
        PlayerSpells[0].SupportSpellType = AllSpells[0].SupportSpellType;
        PlayerSpells[0].OffensiveSpellType = AllSpells[0].OffensiveSpellType;

        //Get Spell Requirements for slot 0 
        PlayerSpells[0].RequiredLevel = AllSpells[0].RequiredLevel;
        PlayerSpells[0].CanCast = AllSpells[0].CanCast;
        //Get Spell Damage for slot 0 
        PlayerSpells[0].Damage = AllSpells[0].Damage;
        PlayerSpells[0].MinDamage = AllSpells[0].MinDamage;
        PlayerSpells[0].MaxDamage = AllSpells[0].MaxDamage;
        //Get Spell Restore for slot 0 
        PlayerSpells[0].Restore = AllSpells[0].Restore;
        PlayerSpells[0].MinRestore = AllSpells[0].MinRestore;
        PlayerSpells[0].MaxRestore = AllSpells[0].MaxRestore;
        //Get Spell Casting for slot 0 
        PlayerSpells[0].SpellCastTime = AllSpells[0].SpellCastTime;
        PlayerSpells[0].SpellRecastTime = AllSpells[0].SpellRecastTime;
        PlayerSpells[0].CasterRecoveryTime = AllSpells[0].CasterRecoveryTime;
    }
    private void FixedUpdate()
    {
        //Checks if buttons were input to activate stances
        #region input bool checks 
        magicDefensiveStance = Input.GetButton("leftBumper");
        magicOffensiveStance = Input.GetButton("rightBumper");
        defensiveStance = (Input.GetAxisRaw("leftTrigger") != 0);
        techniqueStance = (Input.GetAxisRaw("rightTrigger") != 0);

        Fire1 = Input.GetButton("Fire1");
        Fire2 = Input.GetButton("yButton");
        Fire3 = Input.GetButton("bButton");

        AttackCheck();

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
            DefensiveStanceState();
        }
        if (techniqueStance)
        {
            TechniqueStanceState();
        }
        if (isCastingSpell)
        {
            SpellCastingState();
        }
        if (isChargingTechnique)
        {
            ChargingTechniqueState();
        }
        if (isPreparingItem)
        {
            PreppingItemState();
        }
        if(previousState != CurrentCombatState)
        {
            spellCastTimer = 6.15f;
            itemPrepTimer = 3f;
            TechniqueChargeTimer = 3f;
            menuActivationTimer = 6.2f;
        }
        else
            
        isCastingSpell = false;
        isChargingTechnique = false;
        isPreparingItem = false;
        #endregion

        if(menuActivationTimer <= 0)
        {
            Debug.LogFormat("Open corresponding menu(CombatState: {0}, MenuToggle: {1}", combatState, menuToggle);
            menuManager.OpenCorrespondingMenu(combatState, menuToggle);
        }

        #region Switch Statement for Combat Controller
      
            switch (CurrentCombatState)
            {
            ///Idle Combat Occurs when within vicinity of enemy combatant or incoming attack
            ///Notity Camera 
                case CombatState.IdleCombatState:

                
                    spellCastTimer = 6.15f;
                    itemPrepTimer = 3f;
                    TechniqueChargeTimer = 3f;
                    menuActivationTimer = 6.2f;
                canAttack = true;
                    if (thirdPersonPlayerCharacter.onRail)
                    {
                        
                    }
                        // PlayCombatNothingAnimation();
                        isTreckingAndAttacking = false;
                    break;
                case CombatState.DieState:
                    {
                        DieState();
                    }
                    //controller.enabled = false;
                    break;
                case CombatState.AttackState:
                    {
                        thirdPersonPlayerCharacter.Attack(1);
                        isTreckingAndAttacking = true;
                    }

                    break;
                case CombatState.EvadeState:
                    {
                        //While evading physical/magical dmg that hits fighter is reduced by 1/4th
                        //Critical hits and multihits deal more damage however.
                        ///Don't think player will be able to evade attacks while trecking...check notes to be sure
                        EvadeState();
                    }
                    break;
                case CombatState.TakenDamageState:
                    {
                        TakenDamageState();
                        isTreckingAndAttacking = false;
                    }
                    //controller.enabled = true;
                    break;
                case CombatState.SpellCastingState:
                    {
                        spellCastTimer -= Time.deltaTime;
                    //•	LB – Support magic stance gradually restores HP for self but is 10 % weaker to 
                    //physical knockback while casting.                    
                    //SpellCastingState();
                        isTreckingAndAttacking = true;
                    }
                    //controller.enabled = false;
                    break;
                case CombatState.ShootingState:
                    {
                        //Can parry other projectiles
                        //fighter is 5% weaker to projectiles, magic, and attacks while shooting.                                  
                        this.ShootingState();
                        isTreckingAndAttacking = true;
                    }
                    //controller.enabled = false;
                    break;
                case CombatState.TechniqueChargingState:
                    {
                       // ChargingTechniqueState();
                        
                        thisRook.TechniqueState();
                        isTreckingAndAttacking = true;
                    }
                    //controller.enabled = false;
                    break;
                case CombatState.ThrowingState:
                    {
                        //Can parry projectiles and attacks
                        //Fighter is 5% weaker to projectiles, magic, and attacks while using items.          
                        thisRook.ThrowingState();
                        isTreckingAndAttacking = true;
                    }
                    //controller.enabled = true;
                    break;
                case CombatState.ItemPreparingState:
                    {
                        PreppingItemState();
                        isTreckingAndAttacking = true;
                    }
                    //controller.enabled = true;
                    break;
                case CombatState.CheckingInventoryState:
                    {
          
                    }
                    //controller.enabled = true;
                    break;
                case CombatState.MagicDefensiveStanceState:
                    {
                        menuActivationTimer -= Time.deltaTime;
                        //•	LB – Support magic stance gradually restores HP for self
                        //is 10 % weaker to physical attacks while holding this button down.                   
                        thisRook.SupportMagicStance();
                        isTreckingAndAttacking = true;
                    }
                    //controller.enabled = false;
                    break;
                case CombatState.MagicOffensiveStanceState:
                    {
                        menuActivationTimer -= Time.deltaTime;
                        //•	RB – Offensive magic stance gradually restores MP for self
                        //is 10 % weaker to physical attacks while holding this button down.
                        thisRook.OffensiveMagicStance();
                        isTreckingAndAttacking = true;
                    }
                    //controller.enabled = false;
                    break;
                case CombatState.DefensiveStanceState:

                    menuActivationTimer -= Time.deltaTime;
                    //•	LT – Defensive stance drastically lowers physical damage taken
                    //is 10 % weaker to magical attacks while holding this button down.                   
                    thisRook.DefensiveStance();
                    isTreckingAndAttacking = false;
                    //controller.enabled = false;
                    // PlayBlockAnimation();
                    break;
                case CombatState.TechniqueStanceState:
                    {
                        menuActivationTimer -= Time.deltaTime;
                        //•	RT – Technique stance gradually restores SP for self 
                        //is 10 % weaker to magical attacks while holding this button down.                 
                        thisRook.TechniqueStance();
                        isTreckingAndAttacking = false;
                    }
                    //controller.enabled = false;
                    break;
            }
        thirdPersonPlayerCharacter.CombatMovement(canAttack, isCastingSpell, defensiveStance, isChargingTechnique);
        #endregion
    }

    public override void ChargingTechniqueState()
    {
        TechniqueChargeTimer -= Time.deltaTime;

        base.ChargingTechniqueState();
    }

    public override void PreppingItemState()
    {
        itemPrepTimer -= Time.deltaTime;
     
        base.PreppingItemState();       
    }

    void Update()
    {
        if (autoRotate && CanTarget())
        {
            //Rotate toward the Target with rotation speed
            Quaternion targetRotation = Quaternion.LookRotation(playerController._Shooting.target.transform.position - aimPoint.transform.position, aimPoint.transform.up);
            aimPoint.transform.rotation = Quaternion.Lerp(aimPoint.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //If thirdPersonPlayerCharacter is OnRail 
        if (thirdPersonPlayerCharacter.onRail)
        {

        }
    }

    #region combatManuevers
    void CombatManuevers()
    {

    }

    #region Check for targets before shooting
    bool CanTarget()
    {
        if (target != null)
        {
            enemyRook = target;
            _Shooting.target = target.gameObject.transform;
            if (enemyRook.isDestroyed)
            {
                return false;
            }
            if (_Shooting.target)
            {
                if (Vector3.Distance(this.transform.position, playerController._Shooting.target.position) < enemyController._Shooting.range)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        else
            return false;
    }
    #endregion

    #region Set Target 
    public void SetTarget(Rook newTarget)
    { //May need to be revised, needs to target new enemy and lose old enemy
        if (newTarget != target)
        {
            if (target != null) { this.targetsAttackingThis.Remove(this); }

            if (newTarget != null)
            {
                target = newTarget;
                this.targetsAttackingThis.Add(this);
            }
        }
    }
    #endregion

    #region AttackState
    private void AttackState()
    {
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
    }
    #endregion

    #region AttackCheck
    private void AttackCheck()
    {//Might refactor and combine later(KeyboardInput.IsHoldingKey(attackKBKey) || (playerController.gamepadInput && playerController.myGamepad.GetButton(attackGPKey)))
        if (Input.GetButton("Fire1"))
        {
            CurrentCombatState = CombatState.AttackState;
            CombatStateChanged(CurrentCombatState);
            thirdPersonPlayerCharacter.Attack(1);
        }
        else if (Input.GetButton("bButton"))
        {
            CurrentCombatState = CombatState.AttackState;
            CombatStateChanged(CurrentCombatState);
            thirdPersonPlayerCharacter.Attack(2);
        }
        else if (Input.GetButton("yButton"))
        {
            CurrentCombatState = CombatState.AttackState;
            CombatStateChanged(CurrentCombatState);
            thirdPersonPlayerCharacter.Attack(3);
        }
        else if (Input.anyKey == false)
        {
            CurrentCombatState = CombatState.IdleCombatState;
            CombatStateChanged(CurrentCombatState);
            if (playerController != null)
                playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            else
                playerController = this.gameObject.GetComponent<PlayerController>();
        }
    }
    #endregion

    #region AttackOnCoolDown
    private void AttackOnCooldown()
    {
        if (CurrentCombatState == CombatState.AttackState)
        {
            if (thisRook.PhysicalAttackTimer >= 0 && thisRook.PhysicalAttackTimer <= 0.2f)
            {
                // PlayAttackAnimation();
            }
            thisRook.PhysicalAttackTimer += Time.deltaTime;
            if (thisRook.PhysicalAttackTimer > thisRook.PhysicalSpeed)
            {
                if (hasWeapon) { canAttack = !canAttack; }
                thisRook.PhysicalAttackTimer = 0;
                if (!hasWeapon) { this.Attack(); }
            }
        }
    }
    #endregion

    #region BeingAttacked
    public virtual void BeingAttacked(Rook attackedBy)
    {
        switch (CurrentCombatState)
        {
            case CombatState.IdleCombatState:
                break;
            case CombatState.DefensiveStanceState:
                {
                    Block(attackedBy);
                }
                break;
            case CombatState.AttackState:
                {
                    Parry(attackedBy);
                }
                break;
            case CombatState.EvadeState:
                Dodge();
                break;
            case CombatState.MagicDefensiveStanceState:
                {
                    thisRook.RecoverHealth(thisRook.StaminaPoints);
                    PunitiveStanceDamage(attackedBy);
                }
                break;
            case CombatState.MagicOffensiveStanceState:
                {
                    thisRook.RecoverMagic(thisRook.MagicPoints);
                    PunitiveStanceDamage(attackedBy);
                }
                break;
            case CombatState.TechniqueStanceState:
                {
                    thisRook.RecoverStamina(thisRook.StaminaPoints);
                    Parry(attackedBy);
                }
                break;
        }
    }
    #endregion

    #region Physical Attack
    public virtual void Attack()
    {
        //do the attack thing
        DealDamage(thisRook.PhysicalAttack);
        if (this.target != null)
        { this.BeingAttacked(enemyRook); }
    }
    #endregion

    #region Magical Attack
    public virtual void MagicAttack()
    {
        //do the attack thing
        this.DealDamage(thisRook.MagicalAttack);
        if (this.target != null)
        { this.BeingAttacked(enemyRook); }
    }
    #endregion

    #region PunitiveStance Damage
    private float PunitiveStanceDamage(Rook attackedBy)
    {
        float extraDamage = attackedBy.PhysicalAttack / 2;

        return (int)extraDamage;
    }
    #endregion

    //Activate Parry Event
    #region Parry State
    private void Parry(Rook attackedBy)
    {
        //Create stats to gauge enemy and players str, dex, inputs 

        //Damage both player and enemies weapon 
        WeaponDamage(attackedBy);

    }
    #endregion

    //Damage to weapon after performing a parry
    #region Weapon Damage
    private float WeaponDamage(Rook attackedBy)
    {
        float weaponDamage = attackedBy.PhysicalAttack / 10;

        return (int)weaponDamage;
        throw new NotImplementedException();
    }
    #endregion

    #region Deal Damage
    public virtual void DealDamage(float amount)
    {
        if (targetToAttack != null)
        { targetToAttack.TakeDamage(amount); }
    }
    #endregion

    #region MagicDefensiveStanceState
    private void MagicDefensiveStanceState()
    {
        CurrentCombatState = CombatState.MagicOffensiveStanceState;
        CombatStateChanged(CurrentCombatState);

        menuManager.menuManagerCombatState = (CombatState)((int)CurrentCombatState); 
        previousState = CurrentCombatState;

      
        if(CurrentCombatState == CombatState.MagicDefensiveStanceState)
        {
            this.RecoverHealth(this.healthRegen);
            //this.SupportMagicStance();
        }
        
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
        
        if (Fire1)
        {
            isCastingSpell = true;
            Debug.Log("Cast magicSpellX");
            UsedSpell(PlayerSpells[0].SpellID);
           
            thirdPersonPlayerCharacter.CastAttack(1);           
        }
        else if (Fire2)
        {
            isCastingSpell = true;
            Debug.Log("Cast magicSpellY timer: " + spellCastTimer);
            
            thirdPersonPlayerCharacter.CastAttack(2);
            //thisRook.magicChargeMultiplier(spellChargeTimer, PlayerSpells[0].SpellID, PlayerSpells[0].ManaCost, PlayerSpells[0].minDamage, PlayerSpells[0].maxDamage, PlayerSpells[0].damage);
        }
        else if (Fire3)
        {
            isCastingSpell = true;
            Debug.Log("Cast magicSpellB");
           
            thirdPersonPlayerCharacter.CastAttack(3);

        }
        else if (Input.anyKey == false)
        {
            CurrentCombatState = CombatState.MagicDefensiveStanceState;
            if (playerController != null)
                playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            else
                playerController = this.gameObject.GetComponent<PlayerController>();
        }
    }

    private void UsedSpell(int spellID)
    {
        switch (spellID)
        {
            case 0:
                print("Used Spell 0");
                break;
            case 1:
                print("Used Spell 1");
                break;
            case 2:
                print("Used Spell 2");
                break;
            case 3:
                print("Used Spell 3");
                break;
            case 4:
                print("Used Spell 4");
                break;
            case 5:
                print("Used Spell 5");
                break;
            case 6:
                print("Used Spell 6");
                break;
            default:
                print("Default");
                break;
        }
    }
    #endregion

    #region MagicOffensiveStanceState
    protected void MagicOffensiveStanceState()
    {
        CurrentCombatState = CombatState.MagicOffensiveStanceState;
        CombatStateChanged(CurrentCombatState);

        menuManager.menuManagerCombatState = (CombatState)((int)CurrentCombatState);
        previousState = CurrentCombatState;

        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
        
        if (Fire1)
        { 
            Debug.Log("Cast magicSpellX");
            isCastingSpell = true;         
            thirdPersonPlayerCharacter.CastAttack(1);         
        }
        else if (Fire2)
        {
            Debug.Log("Cast magicSpellY");
            isCastingSpell = true;         
            thirdPersonPlayerCharacter.CastAttack(2);
        }
        else if (Fire3)
        {
            Debug.Log("Cast magicSpellB");
            isCastingSpell = true;        
            thirdPersonPlayerCharacter.CastAttack(3);
        }
        else if (Input.anyKey == false)
        {
            CurrentCombatState = CombatState.IdleCombatState;
            if (playerController != null)
                playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            else
                playerController = this.gameObject.GetComponent<PlayerController>();
        }
    }
    #endregion

    #region DefensiveStanceState
    protected void DefensiveStanceState()
    {
        CurrentCombatState = CombatState.DefensiveStanceState;
        CombatStateChanged(CurrentCombatState);

        menuManager.menuManagerCombatState = (CombatState)((int)CurrentCombatState);
        previousState = CurrentCombatState;
        //Contact base(Rook) to activate increased knockback/status resistance 
        //base.DefensiveStance();
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();


        if (Fire1)
        {
            isGrabbing = true;
          
        }
        else if (Fire2)
        {
            isTackling = true;
        }
        else if (Fire3)
        {
         
        }
        if (Input.GetAxis("leftJoystickHorizontal") > .95f)
        {
            StartCoroutine(thirdPersonPlayerCharacter._DirectionalRoll(Input.GetAxis("leftJoystickVertical"), Input.GetAxis("leftJoystickHorizontal")));
            CurrentCombatState = CombatState.EvadeState;
            CombatStateChanged(CurrentCombatState);
        
            if (playerController != null)
                playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            else
                playerController = this.gameObject.GetComponent<PlayerController>();
            Debug.Log("Evasive Manuever to the right");
            playerController.canEvade = false;
        }
        if (Input.GetAxis("leftJoystickHorizontal") < -.95f)
        {
            StartCoroutine(thirdPersonPlayerCharacter._DirectionalRoll(Input.GetAxis("leftJoystickVertical"), Input.GetAxis("leftJoystickHorizontal")));
            CurrentCombatState = CombatState.EvadeState;
            CombatStateChanged(CurrentCombatState);
        
            if (playerController != null)
                playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            else
                playerController = this.gameObject.GetComponent<PlayerController>();
            Debug.Log("Evasive Manuever to the left");
            playerController.canEvade = false;
        }
        if (Input.GetAxis("leftJoystickVertical") > .95f)
        {
            StartCoroutine(thirdPersonPlayerCharacter._DirectionalRoll(Input.GetAxis("leftJoystickVertical"), Input.GetAxis("leftJoystickHorizontal")));
            CurrentCombatState = CombatState.EvadeState;
            CombatStateChanged(CurrentCombatState);
            if (playerController != null)
                playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            else
                playerController = this.gameObject.GetComponent<PlayerController>();
            Debug.Log("Evasive Manuever forward roll");
            playerController.canEvade = false;
        }
        if (Input.GetAxis("leftJoystickVertical") < -.95f)
        {
            StartCoroutine(thirdPersonPlayerCharacter._DirectionalRoll(Input.GetAxis("leftJoystickVertical"), Input.GetAxis("leftJoystickHorizontal")));
            CurrentCombatState = CombatState.EvadeState;
            CombatStateChanged(CurrentCombatState);
            if (playerController != null)
                playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            else
                playerController = this.gameObject.GetComponent<PlayerController>();
            Debug.Log("Evasive Manuever to the back roll");
            playerController.canEvade = false;
        }

        if (Input.GetAxisRaw("rightTrigger") != 0)
        {//Will require some debugging, technique used while crouched should be unable to charge...?
            CurrentCombatState = CombatState.TechniqueChargingState;
            CombatStateChanged(CurrentCombatState);
            if (playerController != null)
                playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
            else
                playerController = this.gameObject.GetComponent<PlayerController>();
            Debug.Log("Technique is used while crouched");
        }
        previousState = CurrentCombatState;
    }
    #endregion

    #region Block
    public virtual void Block(Rook attackedBy)
    {
        //check sturdiness
        if (blockHitTimes > attackedBy.PhysicalDefense)
        {
            //break block - no damage recovery
        }
        else
        {
            blockHitTimes++;
            //normal blocking - maybe cut damage
            thisRook.TakeDamage(attackedBy.PhysicalAttack - (attackedBy.PhysicalAttack / 4));
        }
    }
    #endregion

    #region TechniqueStanceState
    protected void TechniqueStanceState()
    {
        CurrentCombatState = CombatState.TechniqueStanceState;
        CombatStateChanged(CurrentCombatState);

        menuManager.menuManagerCombatState = (CombatState)((int)CurrentCombatState);
        previousState = CurrentCombatState;

        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();

        if (Fire1)
        {
            Debug.Log("Cast magicSpellX");
            isCastingSpell = true;
            isChargingTechnique = true;
            thirdPersonPlayerCharacter.CastAttack(1);

        }
        else if (Fire2)
        {
            Debug.Log("Cast magicSpellY");
            isCastingSpell = true;
            isChargingTechnique = true;
            thirdPersonPlayerCharacter.CastAttack(2);
        }
        else if (Fire3)
        {
            Debug.Log("Cast magicSpellB");
            isCastingSpell = true;
            isChargingTechnique = true;
            thirdPersonPlayerCharacter.CastAttack(3);
        }

        //else if (Input.anyKey == false)
        //{
        //    CurrentCombatState = CombatState.IdleCombatState;
        //    if (playerController != null)
        //        playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        //    else
        //        playerController = this.gameObject.GetComponent<PlayerController>();
        //}
    }
    #endregion

    #region DieState
    private void DieState()
    {
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
    }
    #endregion

    #region DeathState
    public virtual void Death()
    {

        Debug.Log("Running Rook.Death Method");
        //this.gameObject.SetActive(false);

        //Destroy(this.gameObject, 3);
    }
    #endregion

    #region TakenDamageState
    private void TakenDamageState()
    {
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
        Debug.Log("TakenDamageState!");

        TakeDamageState();
        thirdPersonPlayerCharacter.GetHit();
    }
    #endregion

    #region EvadeState
    private void EvadeState()
    {   
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
        Debug.Log("EvadeState!");
        thirdPersonPlayerCharacter.Rolling();
    }
    #endregion

    #region Dodge
    public virtual void Dodge()
    {
        //do the dodge thing
        Debug.Log("Dodged");
    }
    #endregion

    #region CastingState
    public override void SpellCastingState()
    {
        CurrentCombatState = CombatState.SpellCastingState;
        if(!Fire1)
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
        Debug.Log("CastingState!");

        base.SpellCastingState();
    }
    #endregion

    #region ShootingState
    public override void ShootingState()
    {
        menuManager.menuManagerCombatState = (CombatState)((int)CurrentCombatState);
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
        Debug.Log("ShootingState!");

        base.ShootingState();
    }
    #endregion

    #region TechniqueState
    public override void TechniqueState()
    {
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
        Debug.Log("TechniqueState!");

        base.TechniqueState();
    }
    #endregion

    #region ThrowingState
    public override void ThrowingState()
    {
        menuManager.menuManagerCombatState = (CombatState)((int)CurrentCombatState);
        if (playerController != null)
            playerController.playerControllerCombatState = (CombatState)((int)CurrentCombatState);
        else
            playerController = this.gameObject.GetComponent<PlayerController>();
        Debug.Log("UseitemState!");

        base.ThrowingState();
    }
    #endregion

    #endregion

    //Test Gui
    #region Test Gui for Spells
    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(20, 30, 120, 70), texture);

        Rect rect1 = new Rect(Screen.width / 2, Screen.height - 64, 32, 32);

        if (GUI.Button(new Rect(Screen.width / 2, Screen.height - 64, 32, 32), "5"))
        {
            UsedSpell(PlayerSpells[0].SpellID);
        }
        if (rect1.Contains(Event.current.mousePosition))
        {
            GUI.DrawTexture(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 150, 200, 200), texture);
            GUI.Label(new Rect(Input.mousePosition.x + 20, Screen.height - Input.mousePosition.y - 150, 200, 200),
                "Spell name: " + PlayerSpells[0].SpellName + "\n" + "Spell Description:  " + PlayerSpells[0].Description + "\n" + "Spell ID: " + PlayerSpells[0].SpellID);
        }
        
    }
    #endregion
}


