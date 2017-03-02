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
        GameObject partyFormationPanelCenter;
        [SerializeField]
        GameObject[,] partyFormationSpots;

        [SerializeField]
        ThirdPersonCharacter thirdPersonCharacter;
        [SerializeField]
        ThirdPersonUserControl thirdPersonUserControl;


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
            UpdateCursor();
            //   UpdateThirdPersonController();
          //  UpdateDescriptionText(defaultDescriptionMessage);  //public const string defaultDescriptionMessage
        }

        private void HidePartyFormationPanel()
        {
            partyFormationPanel.SetActive(false);
        }

        void HandleInput()
        {
            if ((Input.GetButton("yButton")))
            {
                Debug.Log("Enter was pressed");
                if (IsPartyFormationPanelShowing)
                {
                    
                    HidePartyFormationPanel();
                }
                else
                {
                    ShowPartyFormationPanel();
                }
            }
        }

        private void ShowPartyFormationPanel()
        {
            partyFormationPanel.SetActive(true);
            partyFormationPanel.transform.position = thirdPersonCharacter.transform.position;
        }
        private void UpdateCursor()
        {
            if (IsPartyFormationPanelShowing)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
