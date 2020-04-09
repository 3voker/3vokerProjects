using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class InventoryManager : MenuManager
{
    /// <summary>
    /// Enum that toggles between menu states that display weapon techniques, mana techniques, and job techniques 
    /// </summary>
    protected enum EquipScreenMenuState { ClosedMenu, OpenMenu, WeaponsMenu, ArmorMenu, ItemsMenu, selectingMenu }

    GameObject inventoryMenuPanel;

    //[SerializeField]
    //ThirdPersonPlayerCharacter thirdPersonPlayerCharacter;
    //[SerializeField]
    //PlayerController playerController;

    //[SerializeField]
    //CombatController combatController;

    [SerializeField]
    GameObject inventoryItemTogglePrefab;

    [SerializeField]
    Transform inventoryItemsListPanel;
    GameObject inventoryItemsListPanelGameObject;



    public InventoryObject InventoryObjectRepresented { get; set; }

    [SerializeField]
    Text descriptionText;

    public List<InventoryObject> InventoryObjects { get; set; }

    public List<GameObject> inventoryObjectToggles;

    public const string defaultDescriptionMessage = "Select an item.";

    //Checks if Inventory Menu Screen is showing
    bool IsInventoryMenuShowing
    {
        get { return inventoryMenuPanel.activeSelf;}
        set { MainMenuScreenChange(MainMenuManagerState.InventoryMenu);}
    }
    #region Inventory Menu State Machine

    //CurrentMenu of MenuManager
    [SerializeField]
    EquipScreenMenuState currentInventoryMenuState;

    //Previous Window of MenuManagerState 
    [SerializeField]
    EquipScreenMenuState previousInventoryMenuState;

    protected EquipScreenMenuState GetMenuScreen
    {
        get { return currentInventoryMenuState; }
        set { previousInventoryMenuState = currentInventoryMenuState;
            currentInventoryMenuState = value;
            EquipScreenMenuStateChange(value); }
    }

    protected EquipScreenMenuState EquipScreenMenuStateChange(EquipScreenMenuState menu)
    {
        switch (menu)
        {
            case EquipScreenMenuState.ClosedMenu:

                return currentInventoryMenuState = menu;
            case EquipScreenMenuState.OpenMenu:
               // this.menuManagerCharacterState = CharacterState.CheckingInventoryState;
                return currentInventoryMenuState = menu;
            case EquipScreenMenuState.ItemsMenu:
                if (Input.GetButton("bButton"))
                {
                 //   this.menuManagerCharacterState = CharacterState.IdleState;
                    return menu = EquipScreenMenuState.ClosedMenu;
                }
                if(Input.GetAxisRaw("leftTrigger") != 0)
                {
                  //  this.menuManagerCharacterState = CharacterState.CheckingInventoryState;
                    return menu = EquipScreenMenuState.WeaponsMenu;
                }
                return currentInventoryMenuState = menu;
            case EquipScreenMenuState.WeaponsMenu:

                if (Input.GetButton("bButton"))
                {
                    return menu = EquipScreenMenuState.ClosedMenu;
                }
                if (Input.GetAxisRaw("leftTrigger") != 0)
                {
                //    this.menuManagerCharacterState = CharacterState.CheckingInventoryState;
                    return menu = EquipScreenMenuState.ArmorMenu;
                }
                return currentInventoryMenuState = menu;
            case EquipScreenMenuState.ArmorMenu:
                if (Input.GetButton("bButton"))
                {
                    return menu = EquipScreenMenuState.ClosedMenu;
                }
                if (Input.GetAxisRaw("leftTrigger") != 0)
                {
               //     this.menuManagerCharacterState = CharacterState.CheckingInventoryState;
                    return menu = EquipScreenMenuState.ItemsMenu;
                }
                if (Input.GetButton("aButton"))
                {
                 //   this.menuManagerCharacterState = CharacterState.CheckingInventoryState;
                    return menu = EquipScreenMenuState.selectingMenu;
                }
                return currentInventoryMenuState = menu;

        }
        return currentInventoryMenuState = menu;
    }
    #endregion

    // private bool isInventoryMenuShowing;
    void Start()
    {
        //thirdPersonPlayerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<ThirdPersonPlayerCharacter>();
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //combatController = GameObject.FindGameObjectWithTag("Player").GetComponent<CombatController>();

        inventoryMenuPanel = GameObject.Find("Inventory Menu Panel");
        inventoryItemsListPanelGameObject = GameObject.Find("Inventory Items List Panel");
       
        InventoryObjects = new List<InventoryObject>();

        inventoryObjectToggles = new List<GameObject>();
        HideInventoryMenu();
    }
    void Update()
    {
        //InventoryObjectRepresented.DescriptionText
        //ShowHideInventoryMenu(IsInventoryMenuShowing);
        UpdateCursor();
        //   UpdateThirdPersonController();
        UpdateDescriptionText(defaultDescriptionMessage);//public const string defaultDescriptionMessage 
        inventoryItemsListPanelGameObject = inventoryItemsListPanel.gameObject;
    }
    public void UpdateDescriptionText(string newText)
    {
        descriptionText.text = newText;
    }
    public bool ShowHideInventoryMenu(bool input)
    {
        if (combatController.CurrentCombatState == CombatState.CheckingInventoryState)
        {        
           ShowInventoryMenu();
           //CurrentMenuScreen(MenuScreen.InventoryMenu);
            return input = IsInventoryMenuShowing;
        }
        //Item menu should remain open while player is in throw animation.(Item Cool down is in inventory menu) 
        //else if(rook.ThrowingItemState() == Rook.CombatState.ThrowingItemState){ ShowInventoryMenu();}
        else
        {
            HideInventoryMenu();
          //  MainMenuScreenChange(MainMenuManagerState.ClosedMenu);
            return !IsInventoryMenuShowing;
        }
    }
    private void ShowInventoryMenu()
    {
        DestroyInventoryItemToggles();
        GenerateInventoryItemToggles();
        inventoryMenuPanel.SetActive(true);
        // thirdPersonCharacter.enabled = false;
        //   thirdPersonUserControl.enabled = false;
    }
    private void DestroyInventoryItemToggles()
    {
        foreach (GameObject item in inventoryObjectToggles)
        {
            Destroy(item);
        }
    }
    private void GenerateInventoryItemToggles()
    {

        for (int i = 0; i < InventoryObjects.Count; i++)
        {
            GameObject inventoryObjectToggle = GameObject.Instantiate
                (inventoryItemTogglePrefab, inventoryItemsListPanel) as GameObject;

            inventoryObjectToggle.GetComponent<InventoryMenuItem>().InventoryObjectRepresented = InventoryObjects[i];

            inventoryObjectToggle.GetComponentInChildren<Text>().text =
                InventoryObjects[i].DescriptionText;

            inventoryObjectToggle.GetComponentInChildren<Text>().text =
                InventoryObjects[i].DisplayText;


            Toggle toggle = inventoryObjectToggle.GetComponent<Toggle>();
            toggle.group = inventoryItemsListPanel.GetComponent<ToggleGroup>();
            inventoryObjectToggles.Add(inventoryObjectToggle);
        }
    }
    private void HideInventoryMenu()
    {
        // IsInventoryMenuShowing = false;
        inventoryMenuPanel.SetActive(false);
        // thirdPersonCharacter.enabled = true;
        //   thirdPersonUserControl.enabled = true;

    }
    private void UpdateThirdPersonController()
    {
        if (IsInventoryMenuShowing)
        {
            //Change to limit mobility while menu is showing or require D-Pad to toggle through menu
            thirdPersonPlayerCharacter.enabled = false;
            playerController.enabled = false;
        }
        else
        {
            thirdPersonPlayerCharacter.enabled = true;
            playerController.enabled = true;
        }
    }
    private void UpdateCursor()
    {
        if (IsInventoryMenuShowing)
        {
            Cursor.visible = true;
            //  Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

