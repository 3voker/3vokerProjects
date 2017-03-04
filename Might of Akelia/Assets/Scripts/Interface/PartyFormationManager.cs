using UnityEngine;
using System.Collections;
using System;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class PartyFormationManager : MonoBehaviour
    {

        // Use this for initialization

        [SerializeField]
        GameObject partyFormationPanel;
        [SerializeField]
        Transform partyFormationPanelCenter;
        [SerializeField]
        GameObject[,] partyFormationSpots;
        [SerializeField]
        ThirdPersonCharacter thirdPersonCharacter;
        [SerializeField]
        ThirdPersonUserControl thirdPersonUserControl;
     
        float distance = 0f;
        bool IsPartyFormationPanelShowing
        {
            get { return partyFormationPanel.activeSelf; }
        }             
        void Start()
        {
            
            HidePartyFormationPanel();
        }
        void Update()
        {
            //InventoryObjectRepresented.DescriptionText
            HandleInput();
            //   UpdateThirdPersonController();
          //  UpdateDescriptionText(defaultDescriptionMessage);  //public const string defaultDescriptionMessage
        }       
        private void HidePartyFormationPanel()
        {
            partyFormationPanel.SetActive(false);
            thirdPersonCharacter.enabled = true;
            thirdPersonUserControl.enabled = true;
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
            partyFormationPanel.transform.position = thirdPersonCharacter.transform.position;
            thirdPersonCharacter.enabled = false;
            thirdPersonUserControl.enabled = false;            
        }     
    }
}
