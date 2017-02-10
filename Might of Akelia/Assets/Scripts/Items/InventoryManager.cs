using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class InventoryManager : MonoBehaviour
    {

        // Use this for initialization
        [SerializeField]
        GameObject inventoryMenuPanel;

        [SerializeField]
        ThirdPersonCharacter thirdPersonCharacter;

        [SerializeField]
        ThirdPersonUserControl thirdPersonUserControl;
        
        [SerializeField]
        GameObject inventoryItemTogglePrefab;

        [SerializeField]
        Transform inventoryItemsListPanel;

        public InventoryObject InventoryObjectRepresented { get; set; }

        [SerializeField]
        Text descriptionText;

        private bool isOn;

        public List<InventoryObject> InventoryObjects { get; set; }

        public List<GameObject> inventoryObjectToggles;

        public const string defaultDescriptionMessage = "Select an item.";

        bool IsInventoryMenuShowing
        {
            get { return inventoryMenuPanel.activeSelf; }
        }
        // private bool isInventoryMenuShowing;
        void Start()
        {

            InventoryObjects = new List<InventoryObject>();

            inventoryObjectToggles = new List<GameObject>();
            HideInventoryMenu();
        }
        void Update()
        {
            //InventoryObjectRepresented.DescriptionText
            HandleInput();
            UpdateCursor();
         //   UpdateThirdPersonController();
            UpdateDescriptionText(defaultDescriptionMessage);  //public const string defaultDescriptionMessage
        }



        public void UpdateDescriptionText(string newText)
        {
            descriptionText.text = newText;
        }

        void HandleInput()
        {
            if (Input.GetButton("leftJoystickButton"))
            {
                if (IsInventoryMenuShowing)
                {
                    HideInventoryMenu();
                }
                else
                {
                    ShowInventoryMenu();
                }
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
                thirdPersonCharacter.enabled = false;
                thirdPersonUserControl.enabled = false;
            }
            else
            {
                thirdPersonCharacter.enabled = true;
                thirdPersonUserControl.enabled = true;
            }
        }
        private void UpdateCursor()
        {
            if (IsInventoryMenuShowing)
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
