using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum that toggles between menu states that displays an overview Menu of all player menu's
/// </summary>
public enum MainMenuManagerState { ClosedMenu, MainMenu, InventoryMenu, MagicSpellMenu, TechniqueMenu, PartyFormationMenu, ChatLogMenu, TextBoxMenu, ChatBoxMenu, UserInputMenu, QuestMenu, PartyMenu }


public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Prefab player Menu that will instantiate gameObject in its place
    /// </summary>
    [SerializeField]
    public GameObject playerMenu;

    public GameObject inventoryMenu;

    public GameObject magicSpellMenu;

    public GameObject techniqueMenu, partyFormationMenu, chatLogMenu, textBoxMenu, chatBoxMenu, userInputMenu, questMenu, partyMenu;
/// <summary>
/// Reference to inventory manager script attached to this game object 
/// </summary>
InventoryManager inventoryManager;

    /// <summary>
    /// Reference to technique manager script attached to this game object 
    /// </summary>
    

    /// <summary>
    /// Reference to spell manager script attached to this game object 
    /// </summary>
    SpellManager spellManager;

    /// <summary>
    /// Reference to playerController script attached to the player
    /// </summary>
    protected PlayerController playerController;

    /// <summary>
    /// Reference to thirdPersonPlayerCharacter script attached to the player
    /// </summary>
    protected ThirdPersonPlayerCharacter thirdPersonPlayerCharacter;

    /// <summary>
    /// Reference to combatController script attached to the player
    /// </summary>
    protected CombatController combatController;

    /// <summary>
    /// Toggles whether main window is open
    /// </summary>
    public bool isMainMenuActive { get { return playerMenu.activeSelf; } }


    /// <summary>
    /// Event System that may not get used 
    /// </summary>
    MyEventSystem myEventSystem;

    //Reference to Characterstate in player controller
    //public CharacterState menuManagerCharacterState;
    /// <summary>
    ///Reference to CombatState in the combat controller
    /// </summary>
    public CombatState menuManagerCombatState;

    /// <summary>
    /// Toggles whether menu screen is open
    /// </summary>
    bool isAMenuScreenOpen;
    /// <summary>
    /// Toggles whether player can input again
    /// </summary>
    bool canInputAgain = true;

    #region MainMenu State Machine Variables
    /// <summary>
    /// CurrentMenu of MenuManager
    /// </summary>
    [SerializeField]
    MainMenuManagerState currentMainMenuState;

    /// <summary>
    /// Previous Window of MenuManagerState 
    /// </summary>
    [SerializeField]
    MainMenuManagerState previousMainMenuState;

    /// <summary>
    /// Gets Current Main Menu State
    /// </summary>
    public MainMenuManagerState CurrentMainMenuState
    {
        get { return currentMainMenuState; }
        set
        {
            previousMainMenuState = currentMainMenuState;
            currentMainMenuState = value;
            MainMenuScreenChange(value);
        }
    }
    #endregion
    #region MainMenu State Machine 


    /// <summary>
    /// Controls main menu screen change
    /// </summary>
    protected MainMenuManagerState MainMenuScreenChange(MainMenuManagerState menu)
    {
        switch (CurrentMainMenuState)
        {
            case MainMenuManagerState.InventoryMenu:


                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;
            case MainMenuManagerState.ClosedMenu:
                isAMenuScreenOpen = false;
                return previousMainMenuState = menu;
            case MainMenuManagerState.MainMenu:
                //if (Input.GetButton("bButton") || Input.GetButton("Start"))
                //{
                //    CurrentMainMenuState = MainMenuManagerState.ClosedMenu;
                //}

                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;
            case MainMenuManagerState.PartyFormationMenu:

                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;
            case MainMenuManagerState.ChatLogMenu:
                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;
            case MainMenuManagerState.TextBoxMenu:
                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;
            case MainMenuManagerState.ChatBoxMenu:
                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;
            case MainMenuManagerState.UserInputMenu:
                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;
            case MainMenuManagerState.QuestMenu:
                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;
            case MainMenuManagerState.PartyMenu:
                isAMenuScreenOpen = true;
                return previousMainMenuState = menu;

        }
        return previousMainMenuState = menu;
    }
    #endregion

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        inventoryManager = GetComponent<InventoryManager>();
        thirdPersonPlayerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayerCharacter>();
        combatController = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatController>();
    }
    private void FixedUpdate()
    {
        bool mainMenuToggle = Input.GetButton("Start");

        if (mainMenuToggle)
            CurrentMainMenuState = MainMenuManagerState.MainMenu;
        playerMenu.SetActive(false);
        if (isMainMenuActive)
        {
            //If Defensive stance while main player menu is active, will switch to magic spell menu 
            if (Input.GetButton("leftBumper"))
            {
                CurrentMainMenuState = MainMenuManagerState.MagicSpellMenu;
            }
            if (Input.GetButton("rightBumper"))
            {
                CurrentMainMenuState = MainMenuManagerState.MagicSpellMenu;
            }
            if (Input.GetAxisRaw("leftTrigger") != 0)
            {
                CurrentMainMenuState = MainMenuManagerState.InventoryMenu;
            }
            if (Input.GetAxisRaw("rightTrigger") != 0)
            {
                CurrentMainMenuState = MainMenuManagerState.TechniqueMenu;
            }
            if (Input.GetButton("leftJoystickButton"))
            {
                CurrentMainMenuState = MainMenuManagerState.InventoryMenu;
            }
        }
    }
    private void LateUpdate()
    {
        switch (CurrentMainMenuState)
        {
            case MainMenuManagerState.ClosedMenu:
                isAMenuScreenOpen = false;
                break;
            case MainMenuManagerState.MainMenu:
                //if (Input.GetButton("bButton") || Input.GetButton("Start"))
                //{
                //    CurrentMainMenuState = MainMenuManagerState.ClosedMenu;
                //}
                //MainMenu.ShowHideMainMenu();
                isAMenuScreenOpen = true;
                break;

            case MainMenuManagerState.InventoryMenu:

                //inventoryManager.ShowHideInventoryMenu();
                isAMenuScreenOpen = true;
                break;

            case MainMenuManagerState.PartyFormationMenu:

                //PartyFormationMenu.ShowHidePartyFormationMenu();
                isAMenuScreenOpen = true;
                break;
            case MainMenuManagerState.ChatLogMenu:

                //ChatLogMenu.ShowHideChatLogMenu();
                isAMenuScreenOpen = true;
                break;
            case MainMenuManagerState.TextBoxMenu:

                //TextBoxMenu.ShowHideTextBoxMenu();
                isAMenuScreenOpen = true;
                break;
            case MainMenuManagerState.ChatBoxMenu:

                //ChatBoxMenu.ShowHideChatBoxMenu();
                isAMenuScreenOpen = true;
                break;
            case MainMenuManagerState.UserInputMenu:

                //UserInputMenu.ShowHideUserInputMenu();
                isAMenuScreenOpen = true;
                break;
            case MainMenuManagerState.QuestMenu:

                //QuestMenu.ShowHideQuestMenu();
                isAMenuScreenOpen = true;
                break;
            case MainMenuManagerState.PartyMenu:

                //PartyMenu.ShowHidePartyMenu();
                isAMenuScreenOpen = true;
                break;
        }
    }
    protected void OpenMainMenu()
    {
        MainMenuScreenChange(MainMenuManagerState.MainMenu);
        MainMenuScreenChange(CurrentMainMenuState);
    }
    private IEnumerator mainMenuStanceState()
    {
        yield return new WaitForSeconds(2);
        MainMenuScreenChange(CurrentMainMenuState);
    }

    /// <summary>
    /// Method that opens menu based on current combat state and permission to open
    /// </summary>
    public bool OpenCorrespondingMenu(CombatState combatState, bool menuOpen)
    {     
        if (combatState == CombatState.MagicDefensiveStanceState)
        {
            CurrentMainMenuState = MainMenuManagerState.MagicSpellMenu;
            return menuOpen = inventoryManager.ShowHideInventoryMenu(menuOpen);
            //CHANGE TO magicManager
        }
        if (combatState == CombatState.CheckingInventoryState)
        {
            CurrentMainMenuState = MainMenuManagerState.InventoryMenu;
            return menuOpen = inventoryManager.ShowHideInventoryMenu(menuOpen);
        }
        else
            return false;
    }


}
