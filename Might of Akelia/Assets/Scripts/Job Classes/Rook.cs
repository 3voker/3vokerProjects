using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


//PlayerInteraction is player*
//Rook is combat participant*
[RequireComponent(typeof(CombatController))]
public class Rook : JobClassFactory, IJobChanging {

    #region Requirements
    public int LevelRequirement { get; set; }
    #endregion
    #region Status Variables

    #region Health Point Variables
    /// <summary>
    /// Float representing the current health points of this Rook. (Read only?) 
    /// </summary>
    protected float healthPoints = 100;

    /// <summary>
    /// Float representing the current health points of this Rook. (Read/Write) 
    /// </summary>
    public float HealthPoints { get { return healthPoints; } set { healthPoints = value; } }

    /// <summary>
    /// Float representing the max health points of this Rook. (Read/Write) 
    /// MaxHealthPoints varies based on all active Jobs on thisPlayer (Requires 4 stat Calculator)
    /// </summary>
    public float MaxHealthPoints { get; protected set; }

    /// <summary>
    /// Toggles whether player is spell casting (Cast Time)
    /// </summary>
    protected float healthRegen = 1;


    #endregion

    #region Magic Point Variables
    /// <summary>
    /// Float representing the current magic points of this Rook. (Read only?) 
    /// </summary>
    protected float magicPoints;

    /// <summary>
    /// Float representing the current stamina points of this Rook. (Read/Write) 
    /// </summary>
    /// 
    public float MagicPoints { get { return magicPoints; } set { magicPoints = value; } }

    /// <summary>
    /// Float representing the max magic points of this Rook. 
    /// MaxMagicPoints varies based on all active Jobs on thisPlayer (Requires 4 stat Calculator)
    /// </summary>
    public float MaxMagicPoints { get; protected set; }
    #endregion

    #region Stamina Point Variables
    /// <summary>
    /// Float representing the current stamina points of this Rook. (Read only?) 
    /// </summary>
    protected float staminaPoints;
    /// <summary>
    /// Float representing the current stamina points of this Rook. (Read/Write) 
    /// </summary>
    public float StaminaPoints { get { return staminaPoints; } set { staminaPoints = value; } }
    /// <summary>
    /// Float representing the max stamina points of this Rook. 
    /// </summary>
    public float MaxStaminaPoints { get; protected set; }
    #endregion

    /// <summary>
    /// Float that appears on the HUD displaying current magic points of Rook attached to this gameObject(Read only?)
    /// MaxStaminaPoints varies based on all active Jobs on thisPlayer (Requires 4 stat Calculator)
    /// </summary>
    [SerializeField]
    protected float magicPointsHUD = 5;

    /// <summary>
    /// Float that appears on the HUD displaying current stamina points of Rook attached to this gameObject(Read only?)
    /// </summary>
    [SerializeField]
    protected float staminaPointsHUD = 5;

    /// <summary>
    /// Float that alters Rook's physical attack power
    /// </summary>
    protected float physicalAttack = 5;

    /// <summary>
    /// Float that alters Rook's magic attack power
    /// </summary>
    protected float magicalAttack = 6;


    /// <summary>
    /// Float that alters Rook's physical evasion
    /// </summary>
    protected float physicalEvasion;

    /// <summary>
    /// Float that alters Rook's magical evasion
    /// </summary>
    protected float magicalEvasion;

    /// <summary>
    /// Float that alters Rook's physical accuracy 
    /// </summary>
    protected float physicalAccuracy;

    /// <summary>
    /// Float that alters Rook's magical accuracy 
    /// </summary>
    protected float magicalAccuracy;

    /// <summary>
    /// Float that alters Rook's physical attack speed(Distance traveled / Time) 
    /// </summary>
    [Tooltip("If there's a weapon, attack speed should be half the animation time")]
    protected float physicalAttackSpeed = 0.4f;

    /// <summary>
    /// Float that alters Rook's magic attack speed(Distance traveled / Time) 
    /// </summary>
    protected float magicalAttackSpeed = 0.3f;

    /// <summary>
    /// Float that alters Rook's physical defense 
    /// </summary>
    [SerializeField]
    protected float physicalDefense = 5;

    /// <summary>
    /// Float that alters Rook's magic defense 
    /// </summary>
    protected float magicalDefense = 5;

    /// <summary>
    /// Float that alters Rook's attack timer(physical attack/weapon delay) 
    /// </summary>
    protected float physicalAttackTimer;
    /// <summary>
    /// Float that alters Rook's attack timer(magic attack/spell delay) 
    /// </summary>
    protected float magicalAttackTimer;

    /// <summary>
    /// Toggles whether player has MP available to cast next spell
    /// Meant to check current MP against any mp depleting actions
    /// </summary>
    protected bool hasMP = true;

    /// <summary>
    /// Float that alters rook's resistance to status effects
    /// </summary>
    protected float statusResistance;

    /// <summary>
    /// Float that alters rook's resistance to knock back
    /// </summary>
    protected float knockBackResistance;

    #region public Rook Stats

    /// <summary>
    /// Float representing the current physical attack points of this Rook. (Read/Write) 
    /// </summary>
    public float PhysicalAttack { get { return physicalAttack; } }

    /// <summary>
    /// Float representing the current physical defense points of this Rook. (Read/Write) 
    /// </summary>
    public float PhysicalDefense { get { return physicalDefense; } }

    /// <summary>
    /// Float representing the current magical attack points of this Rook. (Read/Write) 
    /// </summary>
    public float MagicalAttack { get { return magicalAttack; } }

    /// <summary>
    /// Float representing the current magical defense points of this Rook. (Read/Write) 
    /// </summary>
    public float MagicalDefense { get { return magicalDefense; } }

    /// <summary>
    /// Float representing the current physical evasion points of this Rook. (Read/Write) 
    /// </summary>
    public float PhysicalEvasion { get { return physicalEvasion; } }

    /// <summary>
    /// Float representing the current magical evasion points of this Rook. (Read/Write) 
    /// </summary>
    public float MagicalEvasion { get { return magicalEvasion; } }

    /// <summary>
    /// Float representing the current physical accuracy points of this Rook. (Read/Write) 
    /// </summary>
    public float PhysicalAccuracy { get { return physicalAccuracy; } }

    /// <summary>
    /// Float representing the current magical accuracy points of this Rook. (Read/Write) 
    /// </summary>
    public float MagicalAccuracy { get { return magicalAccuracy; } }

    /// <summary>
    /// Float representing the current physical attack speed points of this Rook. (Read/Write) 
    /// </summary>
    public float PhysicalSpeed { get { return physicalAttackSpeed; } }

    /// <summary>
    /// Float representing the current magical attack speed points of this Rook. (Read/Write) 
    /// </summary>
    public float MagicalSpeed { get { return magicalAttackSpeed; } }

    /// <summary>
    /// Float representing the current physical attack timer of this Rook. (Read/Write) 
    /// </summary>
    public float PhysicalAttackTimer { get { return physicalAttackTimer; } set { physicalAttackTimer = value; } }
    /// <summary>
    /// Float representing the current magical attack timer of this Rook. (Read/Write) 
    /// </summary>
    public float MagicalAttackTimer { get { return magicalAttackTimer; } set { MagicalAttackTimer = value; } }

    /// <summary>
    /// Float representing the current status resistance points of this Rook. (Read/Write) 
    /// </summary>
    public float StatusResistance { get { return statusResistance; } }

    /// <summary>
    /// Float representing the current knockBack resistance points of this Rook. (Read/Write) 
    /// </summary>
    public float KnockBackResistance { get { return knockBackResistance; } }

    public string BaseJobName
    {
        get
        {
            return "Rook";
        }
    }

    public JobMeterTier jobMeterTier { get { return JobMeterTier.Primary; } }

    public JobTypeCategory jobTypeCategory { get { return JobTypeCategory.Rook; } }

    public int JobSPCost { get { return 0; } }

    public int RequiredLevel { get { return 0; } }

    public bool CanChange { get { return true; } }

    public string Description { get { return ""; } }

    public int JobClassHP { get { return 10; } }

    public int JobClassMP { get { return 10; } }

    public int MinDamage { get { return 5; } }

    public int MaxDamage { get { return 5; } }

    public int Restore { get { return 5; } }

    public int MinRestore { get { return 5; } }

    public int MaxRestore { get { return 5; } }

    public float JobChangeTime { get { return 5; } }

    public float JobChangeCoolDown { get { return 5; } }

    public float JobChangeRecoveryTime { get { return 5; } }

    #endregion

    /// <summary>
    /// Toggles whether rook is dying 
    /// </summary>
    private bool dying;
    /// <summary>
    /// Toggles whether rook is destroyed
    /// </summary>
    public bool isDestroyed = false;

    /// <summary>
    /// References the enemy health script of an enemy rook. 
    /// </summary>
    protected EnemyHealth enemyHealth;


    /// <summary>
    /// References the player rook script of a player rook. 
    /// </summary>
    protected Rook playerRook;

    /// <summary>
    /// References the combatController script of this rook. 
    /// </summary>
    protected CombatController combatController;

    #endregion

  

    #region Unity Functions
    void Awake()
    {
        
        OnAwake();
    }
    void Start()
    {
        // playerHealth = GetComponent<PlayerHealth>();
        combatController = GetComponent<CombatController>();
        //enemyHealth = GetComponent<EnemyHealth>();
        OnStart();
    }
    void Update()
    {
        OnUpdate();
    }
    #endregion
    private WeaponAnimator animator;
    // Use this for initialization
    float timePressed;
    protected virtual void OnStart()
    {
        OnUpdate();
        timePressed = 3;      
    }
    protected virtual void OnAwake()
    {
        staminaPoints = 25f;
        MaxHealthPoints = healthPoints;
        MaxMagicPoints = magicPoints;
    }
    protected virtual void OnUpdate()
    {
      
    }
    public virtual void SupportMagicStance()
    { //Create HP regeneration, it should equal a % of players max health points 
      
        //Debug.Log("Support Magic Stance >> RecoverHealth: " + healthPoints);
        //RecoverHealth(HPregen);

        TakeDamage(healthRegen);

    }
    public virtual void OffensiveMagicStance()
    { //Magic regen
        RecoverMagic(magicPoints);
    }
    public virtual void DefensiveStance()
    { //Self (Increase status resistance against current status affliction)
        IncreaseStatusResistance(statusResistance);
        IncreaseKnockBackResistance(knockBackResistance);
    }

    private void IncreaseStatusResistance(float statusResistance)
    {
        //Increase Focus(Phys. Resist / Mgc. Resist) 
        //Phys. Resist lowers physical knockback, enemy grapple, enemy phys. crit hit
        //Mgc. Resist lowers magical knockback, status effects duration, enemy mgc. crit hit 
    
    }

    private void IncreaseKnockBackResistance(float knockBackResistance)
    {
     //Increase KnockbackResistance both magical and physical only after the block button is presssed.
    }

    public virtual void TechniqueStance()
    { //Stamina regen
        RecoverStamina(staminaPoints);
    }
 
    public virtual void Death()
    {

        Debug.Log("Running Rook.Death Method");
        //this.gameObject.SetActive(false);

      
        //Destroy(this.gameObject, 3);
    }

    public virtual void RecoverHealth(float recover)
    {
        healthPoints += recover;
        if (healthPoints >= MaxHealthPoints)
            healthPoints = MaxHealthPoints;

        if (healthPoints - recover < MaxHealthPoints)
        {
            healthPoints += recover;
        }
        else
        {      
            healthPoints = MaxHealthPoints;        
        }
    }
    public virtual void RecoverMagic(float recover)
    {
        for (int i = 0; MagicPoints < MaxMagicPoints; i++)
        {
            recover = (MaxMagicPoints / 100) * Time.fixedDeltaTime;
        }
         magicPoints += recover;
        if (magicPoints >= MaxMagicPoints)
            magicPoints = MaxMagicPoints;
    }
#region Stamina 
    public virtual void RecoverStamina(float recover)
    {
        staminaPoints += recover;
        if (staminaPoints >= MaxStaminaPoints)
            staminaPoints = MaxStaminaPoints;
    }
    public virtual void LoseStamina(float spCost)
    {
        spCost -= (MaxStaminaPoints / 100) * Time.fixedDeltaTime;      
        staminaPoints -= spCost;
        if (staminaPoints <= 0)
            staminaPoints = 0;
    }
#endregion
    //Apply Damage to target(enemy) receiving damage
    public void ApplyDamage(float damage)
    {
        if (healthPoints - damage > 0)
        {
            healthPoints -= damage;
        }
        else
        {
            isDestroyed = true;
            healthPoints = 0;
            // Destroy();
        }
        //combatController._Audio.Play_GetHit();
    }
   
    //Player takes damage 
    public void TakeDamage(float damage)
    {
        if (healthPoints - damage > 0)
        {
            healthPoints -= damage;
        }
        else
        {
            dying = true;
            isDestroyed = true;
            healthPoints = 0;
           // playerRook.Death();
        }
        //controller._Audio.Play_GetHit();
    }
    //public IEnumerator PhysicalAttackDelay()
    //{
    //    yield return new WaitForSeconds(PhysicalAttackDelay);
    //}
  
    public IEnumerator DeathDelay()
    {
        dying = true;
        yield return new WaitForSeconds(1f);
        Death();
    }
    #region combatManuevers
  
    public virtual void DieState()
    { //notify thirdpersonCharacter for die animation
        //Deactivate playerController 
        throw new NotImplementedException();
    }
    public virtual void AttackState()
    {
        //Debug.Log("Attack State!");
    }
    public virtual void TakeDamageState()
    {
       
        Debug.Log("TakenDamageState!");
    }
    public virtual void EvadeState()
    {
        Debug.Log("EvadeState!");
    }
    public virtual void ShootingState()
    {
        Debug.Log("ShootingState!");
    }
    public virtual void TechniqueState()
    {
        Debug.Log("TechniqueState!");
    }
public virtual void ThrowingState()
    {
        Debug.Log("UseitemState!");
    }

    public void magicChargeMultiplier(float spellChargeTimer, int MagicPoints)
    {
        //Instant cast spell
        if (spellChargeTimer >= 4.75f)
        {

        }
        //Spell charged to first tier
        if (spellChargeTimer >= 3.5f)
        {

        }
        //Spell Charged to second tier
        if (spellChargeTimer >= 2.25f)
        {

        }
        //Spell charged to third and final tier
        if (spellChargeTimer < 1f)
        {

        }
    }

    public virtual void ChargingTechniqueState()
    {
        throw new NotImplementedException();
    }

    public virtual void PreppingItemState()
    {
        throw new NotImplementedException();
    }

    public virtual void SpellCastingState()
    {
        throw new NotImplementedException();
    }
    #endregion
}