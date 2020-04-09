using UnityEngine;
using System.Collections;
using System;


    public class PartyFormationManager : MenuManager
    {

    /// <summary>
    /// Enum that toggles between menu states that control party battle positions and controlling AI party members position on Map
    /// </summary>
    public enum PartyFormationMenuState { ClosedMenu, OpenMenu, BattleFormationMenu, RealTimeStrategyMenu }

   
        [SerializeField]
        GameObject partyFormationPanel;
        [SerializeField]
        Transform partyFormationPanelCenter;
        [SerializeField]
        GameObject[,] partyFormationSpots;
        [SerializeField]
        float speed = 1;
        
        bool IsPartyFormationPanelShowing
        {
            get { return partyFormationPanel.activeSelf; }
        }


    #region Party Formation Menu State Machine Variables
    /// <summary>
    /// CurrentMenu of Party Formation Menu State
    /// </summary>
    [SerializeField]
    PartyFormationMenuState currentPartyFormationMenuState;

    /// <summary>
    /// Previous Window of Party Formation Menu State
    /// </summary>
    [SerializeField]
    PartyFormationMenuState previousPartyFormationMenuState;

    /// <summary>
    /// Gets Current Party Formation Menu State
    /// </summary>
    public PartyFormationMenuState CurrentPartyFormationMenuState
    {
        get { return currentPartyFormationMenuState; }
        set
        {
            previousPartyFormationMenuState = currentPartyFormationMenuState;
            currentPartyFormationMenuState = value;
            PartyFormationScreenChange(value);
        }
    }
    #endregion
    #region Party Formation Menu State Machine 


    /// <summary>
    /// Controls Party Formation Menu State screen change
    /// </summary>
    protected PartyFormationMenuState PartyFormationScreenChange(PartyFormationMenuState menu)
    {
        switch (CurrentPartyFormationMenuState)
        {
            case PartyFormationMenuState.RealTimeStrategyMenu:


                
                return previousPartyFormationMenuState = menu;
           
        }
        return previousPartyFormationMenuState = menu;
    }
    #endregion
    void Start()
        {
        
            HidePartyFormationPanel();
        }
        void Update()
        {
            //InventoryObjectRepresented.DescriptionText
            HandleInput();
            HandleLocation();
            //   UpdateThirdPersonController();
          //  UpdateDescriptionText(defaultDescriptionMessage);  //public const string defaultDescriptionMessage
        }


        private void HidePartyFormationPanel()
        {
            partyFormationPanel.SetActive(false);
            thirdPersonPlayerCharacter.enabled = true;
            playerController.enabled = true;
        }
        void HandleInput()
        {
            if (IsPartyFormationPanelShowing)
            {
                if (Input.anyKeyDown)
                {
                    switch (Input.inputString)
                    {
                        case "bButton":
                            Debug.Log("bButton pressed");
                            HidePartyFormationPanel();
                            break;
                        case "aButton":
                            Debug.Log("aButton pressed");
                            break;      
                        default:
                            Debug.Log("This is not a valid key");
                            HidePartyFormationPanel();
                            break;
                    }
                }
            }
            else if((Input.GetButton("yButton")) && !IsPartyFormationPanelShowing)
            {
                ShowPartyFormationPanel();
            }
        }
        void ShowPartyFormationPanel()
        {         
            partyFormationPanel.SetActive(true);         
            partyFormationPanel.transform.position = thirdPersonPlayerCharacter.transform.position;
            thirdPersonPlayerCharacter.enabled = false;
            playerController.enabled = false;            
            
        }
        private void HandleLocation()
        {
            //float distance of player and panels center
            float distance = Vector3.Distance(thirdPersonPlayerCharacter.transform.position, transform.position);
            //speed in which panel center moves towards player
            float floatTowards = speed + distance * Time.deltaTime;
            //if panel center and player are not aligned move panel towards player
            if (partyFormationPanel.transform.position != thirdPersonPlayerCharacter.transform.position)
            {
       partyFormationPanel.transform.position = Vector3.MoveTowards(partyFormationPanel.transform.position, thirdPersonPlayerCharacter.transform.position, floatTowards);
                
            }
        }
    }
