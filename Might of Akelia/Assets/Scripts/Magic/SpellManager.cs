using UnityEngine;
using System;

[RequireComponent(typeof(UnityEngine.UIElements.Button))]
public class SpellManager 
{
    /// <summary>
    /// Enum that toggles between menu states that display between support magic, offensive magic, job magic
    /// </summary>
    protected enum MagicScreenMenuState { ClosedMenu, OpenMenu, SupportMagicMenu, OffensiveMagicMenu, JobMagicMenu }
    Spells spells;
    SpellCaster spellCaster;

   
    [SerializeField]
    private Magic currentMagicElement;
    private Magic previousMagicElement;
    private Magic opposingMagicElement;
    private BaseMagic baseMagic;

    public static string spellElement;

    GameObject[] ElementSymbols; 

    GameObject supporteMagicSpellWindow;
    GameObject supportManaTechWindow;

    GameObject offensiveMagicSpellWindow;
    GameObject offensiveManaTechWindow;
    GameObject jobClassSpellWindow;

    UnityEngine.UIElements.Button button;

    Animator animator;//animator I think goes to spell caster

    Rook rook = new Rook(); //No

    #region Magic Menu Variables
    
    [SerializeField]
    MagicScreenMenuState currentMagicMenuState;

    /// <summary>
    /// Previous Window of MenuManagerState 
    /// </summary>
    [SerializeField]
    MagicScreenMenuState previousMagicMenuState;

    /// <summary>
    /// Gets Current Main Menu State
    /// </summary>
    protected MagicScreenMenuState CurrentMagicMenuState
    {
        get { return currentMagicMenuState; }
        set
        {
            previousMagicMenuState = currentMagicMenuState;
            currentMagicMenuState = value;
            MagicMenuScreenChange(value);
        }
    }
    #endregion

    #region Magic Menu State Machine 
    /// <summary>
    /// Controls main menu screen change
    /// </summary>
    protected MagicScreenMenuState MagicMenuScreenChange(MagicScreenMenuState menu)
    {
        switch (CurrentMagicMenuState)
        {
            case MagicScreenMenuState.ClosedMenu:


               // isAMenuScreenOpen = false;
                return previousMagicMenuState = menu;
          
        }
        return previousMagicMenuState = menu;
    }
    #endregion

    private void Start()
    {
        CheckForButton();
        supporteMagicSpellWindow = UnityEngine.Object.FindObjectOfType<SupportMagicSpellWindow>().gameObject;
        supportManaTechWindow = UnityEngine.Object.FindObjectOfType<SupportManaTechWindow>().gameObject;
        offensiveMagicSpellWindow = UnityEngine.Object.FindObjectOfType<OffensiveMagicSpellWindow>().gameObject;
        offensiveManaTechWindow = UnityEngine.Object.FindObjectOfType<OffensiveManaTechWindow>().gameObject;
        jobClassSpellWindow = UnityEngine.Object.FindObjectOfType<JobClassSpellWindow>().gameObject;
    }

    private void CheckForButton()
    {
        
    }

    public Magic CurrentElement
    {
        get { return currentMagicElement; }
        set
        {
            previousMagicElement = currentMagicElement;
            currentMagicElement = value;
            //CombatStateChanged();
        }
    }
      
    public void ElementCheck(SpellCaster caster, ITargetable targetedCaster)
    {     
        switch (previousMagicElement)
        {
            case Magic.water:
                spellElement = "water";
                ElementSymbols[0].SetActive(true);
                break;
            case Magic.fire:
                spellElement = "fire";
                ElementSymbols[1].SetActive(true);
                break;
            case Magic.ice:
                spellElement = "ice";
                ElementSymbols[2].SetActive(true);
                break;      
            case Magic.wind:
                spellElement = "wind";
                ElementSymbols[3].SetActive(true); break;
            case Magic.earth:
                spellElement = "earth";
                ElementSymbols[4].SetActive(true);
                break;
            case Magic.lightning:
                spellElement = "lightning";
                ElementSymbols[5].SetActive(true);
                break;
            case Magic.shadow:
                spellElement = "shadow";
                ElementSymbols[6].SetActive(true);
                break;
            case Magic.light:
                spellElement = "light";
                ElementSymbols[7].SetActive(true);
                break;
        }      
        if(previousMagicElement != opposingMagicElement)
        {
            
        }   
    }

    public void AssignSupportSpells()
    {
        foreach(Spell spell in (Enum.GetValues(typeof(SupportSpellTypes))))
        {
            //Show all available support magic player can use. 
            //Do not show spells they have not acquired.
            //Do not show spells they cannot use based on job. 
            //If spell is learned but cannot cast(silenced, no mp, etc.) It should be greyed out. 
        }
    }
    public void AssignSupportTech()
    {
        foreach (Spell spell in (Enum.GetValues(typeof(SupportSpellTypes))))//Need Tech Scripts
        {
            //Show all available support tech player can use. 
            //Do not show spells they have not acquired.
            //Do not show spells they cannot use based on job. 
            //If spell is learned but cannot cast(silenced, no mp/sp, etc.) It should be greyed out. 
        }
    }
    public void AssignOffensiveSpells()
    {
        foreach (Spell spell in (Enum.GetValues(typeof(OffensiveSpellTypes))))
        {
            //Show all available offensive magic player can use. 
            //Do not show spells they have not acquired.
            //Do not show spells they cannot use based on job. 
            //If spell is learned but cannot cast(silenced, no mp, etc.) It should be greyed out. 
        }
    }
    public void AssignOffensiveTech()
    {
        foreach (Spell spell in (Enum.GetValues(typeof(OffensiveSpellTypes))))//Need Tech Scripts
        {
            //Show all available offensive tech player can use. 
            //Do not show spells they have not acquired.
            //Do not show spells they cannot use based on job. 
            //If spell is learned but cannot cast(silenced, no mp/sp, etc.) It should be greyed out. 
        }
    }
}

