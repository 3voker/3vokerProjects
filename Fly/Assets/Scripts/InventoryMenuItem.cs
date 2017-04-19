using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace UnitySampleAssets.Characters.ThirdPerson {
    [RequireComponent(typeof(Toggle))]
    public class InventoryMenuItem : MonoBehaviour
    {
        [SerializeField]      
        private Toggle toggle;

        private InventoryManager inventoryManager;

        public InventoryObject InventoryObjectRepresented { get; set; }

        public void ClickedThisMenuItem()
        {
            AudioSource audio = inventoryManager.GetComponent<AudioSource>();
            audio.Play();
        }

        public void OnValueChanged()
        {

            if (toggle.isOn)
            {
                //update the description text based on the selected item
                inventoryManager.UpdateDescriptionText(InventoryObjectRepresented.DescriptionText);
            } //inventoryObjectRepresented 
            else
            {
                inventoryManager.UpdateDescriptionText(InventoryObjectRepresented.DescriptionText);
                //update description text to some default message. InventoryManager.defaultDescriptionMessage
            }

        }
        void Start()
        {
            toggle = GetComponent<Toggle>();

            try
            {
                inventoryManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
            }
            catch (System.Exception)
            {

                throw new System.Exception("Scene requires game object named"
                    + "Inventory Manager with an Inventory Manager");
            }
        }      
    }
}
