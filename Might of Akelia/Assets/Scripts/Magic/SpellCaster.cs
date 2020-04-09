using System;
using UnityEngine;
using UnityEngine.UI;
using UnitySampleAssets.Characters.ThirdPerson;

public class SpellCaster : MonoBehaviour {

    // Use this for initialization
    ThirdPersonUserControl playerChar;
    ITargetable spellTarget;
    Spell spell;
    Spells spells;
    SpellManager spellManager;
    enum CastingState
    { MagicDefensiveCastState, MagicOffensiveCastState, ManaCastState };
    public int caster_Number = 1;
    public Rigidbody m_Spell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;


    public bool hasMP = false;
    public float m_MaxChargeTime = 6.15f;

    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;

    private bool magicDefensivestate;
    private bool offensiveMagicSpellStance;

    private bool magicSpellX;
    private bool magicSpellY;
    private bool magicSpellB;

    private bool tier1MagicSpell;
    private bool tier2MagicSpell;
    private bool tier3MagicSpell;

    [SerializeField]
    private CombatState combatState;
    private CombatState previousState;
    public CombatState CurrentCombatState
    {
        get { return combatState; }
        set
        {
            previousState = combatState;
            combatState = value;
            //CombatStateChanged();
        }
    }


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }
    private void Start()
    {
        playerChar = GameObject.Find("ThirdPersonCharacter").GetComponent<ThirdPersonUserControl>();
        CastingState castingState;
        // castingState = ThirdPersonUserControl.CharacterState.CastingState;      
        //  mCharScript.CharState = CharacterState.Dialog;
        magicSpellX = Input.GetButtonDown("Fire1");
        magicSpellY = Input.GetButtonDown("yButton");
        magicSpellB = Input.GetButtonDown("bButton");
        // magicSpellCasting 

        offensiveMagicSpellStance = Input.GetButton("rightBumper");
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }
    private void Update()
    {
        //Soon to separate stanceStates to individual scripts 
        //Will encompass buffs and debuffs depending on state
    }
    protected void CombatStateChanged()
    {
        //    if (previousState != CombatState.Blocking && combatState == CombatState.Blocking)
        //    {
        //        PlayBlockAnimation();
        //    }
        //    else if (previousState == CombatState.Blocking && combatState != CombatState.Blocking)
        //    {
        //        PlayUnBlockAnimation();
        //    }
        //    else if (combatState == CombatState.Nothing)
        //    {
        //        PlayCombatNothingAnimation();
        //    }
        //}
        if (previousState == CombatState.MagicOffensiveStanceState)
        {
            if (magicSpellX)
            {
                Debug.Log("Spell Caster Script: magicSpellX");
                //        PlayBlockAnimation();
                CastMagicSpell();
            }
            if (magicSpellY)
            {
                Debug.Log("Spell Caster Script: magicSpellY");
                //        PlayBlockAnimation();
            }
            if (magicSpellB)
            {
                Debug.Log("Spell Caster Script: magicSpellB");
                //        PlayBlockAnimation();
            }
        }
    }

    private void CastMagicSpell()
    {
        spells.Cast(this, spellTarget);
    }
    public void StopCastingSpell()
    {
        Debug.Log("Spell Interrupted.");
    }
    public void SubtractMana(int mp)
    {
        
        if(mp <= 0)
        {
            StopCastingSpell();
        }
    }
    
    private void FixedUpdate()
    {    
      
    }
        //Now lets say we want to deal damage depending upon what weapon player has.
        //WeaponType weaponType;
        //void Start()
        //{
        //    weaponType = WeaponType.Axe; // Let's say the player owns Axe right know 
        //}
        //public void TakeDamage()
        //{
        //    switch (CurrentState)
        //    {
        //        case CurrentState.MagicDefensiveState: damage = 100f; break;
        //        case WeaponType.Axe: damage = 70f; break;

        //        case WeaponType.Bow: damage = 45f; break;
        //        case WeaponType.Dagger: damage = 60f; break;
        //    }
        //}
    private void DefensiveMagicSpellStance()
    {
        spellManager.ElementCheck(this, spellTarget);
        float SpellChargeTimer = 6.15f;
        bool SpellCharge = false;   
        if (hasMP)
            while (Input.GetButton("xButton"))
            {
                SpellChargeTimer--;
               // SubtractMana(mp); Make it subtract from casters mp
                Debug.Log("Charging spell");
            }
        Debug.Log("Activated MagicDefensiveStance");
    }
    private void OffensiveMagicSpellStance()
    {
        spellManager.ElementCheck(this, spellTarget); //Check Current Element of caster.
        m_AimSlider.value = m_MinLaunchForce;
        bool offensiveSpellButton = false;      
        switch (magicSpellX)
        {
            case true:
                magicSpellX = offensiveSpellButton;
                switch (magicSpellX)
                {
                    case true:
                        MagicSpellCast();
                        Debug.Log("Casting Offensive magicSpellX");
                        break;
                    default:
                        Fire();
                        Debug.Log("Firing Offensive magicSpellX");
                        break;
                }
                break;
        }
        switch (magicSpellY)
        {
            case true:
                magicSpellY = offensiveSpellButton;
                switch (magicSpellY)
                {
                    case true:
                        MagicSpellCast();
                        Debug.Log("Casting Offensive magicSpellY");
                        break;
                    default:
                        Fire();
                        Debug.Log("Firing Offensive magicSpellY");
                        break;
                }
                break;
        }
        switch (magicSpellB)
        {
            case true:
                magicSpellB = offensiveSpellButton;
                switch (magicSpellB)
                {
                    case true:
                        MagicSpellCast();
                        Debug.Log("Casting Offensive magicSpellY");
                        break;
                    default:
                        Fire();
                        Debug.Log("Firing Offensive magicSpellB");
                        break;
                }
                break;
        } 
        // Track the current state of the fire button and make decisions based on the current launch force.    
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            // at max charge, not yet fired
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if (offensiveSpellButton && !m_Fired)
        {
            //Have we pressed fire for the first time?
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (offensiveSpellButton && !m_Fired)
        {
            //Holding the fire button, not fired yet
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if (offensiveSpellButton && !m_Fired)
        {
            //We released the button, having not fired yet
            Fire();
        }
    }
    private void MagicSpellCast()
    {
        Debug.Log("Casting Max spell in " + m_MaxChargeTime);

    }
    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;
        Rigidbody shellInstance = Instantiate(m_Spell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        shellInstance.velocity = m_CurrentLaunchForce * m_FireTransform.forward;
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();
        m_CurrentLaunchForce = m_MinLaunchForce;
    }

}
