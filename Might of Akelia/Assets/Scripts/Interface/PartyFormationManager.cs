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
        [SerializeField]
        float speed = 1;
        
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
            HandleLocation();
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
        private void HandleLocation()
        {
            //float distance of player and panels center
            float distance = Vector3.Distance(thirdPersonCharacter.transform.position, transform.position);
            //speed in which panel center moves towards player
            float floatTowards = speed + distance * Time.deltaTime;
            //if panel center and player are not aligned move panel towards player
            if (partyFormationPanel.transform.position != thirdPersonCharacter.transform.position)
            {
       partyFormationPanel.transform.position = Vector3.MoveTowards(partyFormationPanel.transform.position, thirdPersonCharacter.transform.position, floatTowards);
                
            }
        }
    }
}
