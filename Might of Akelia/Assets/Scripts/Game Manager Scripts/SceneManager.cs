using UnityEngine;
using System.Collections;
using System;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class SceneManager : MonoBehaviour, IActivatable, Icommand 
    {

        // Use this for initialization
        
        InventoryManager inventoryManager;
        TextBoxManagerScript textBoxManagerScript;

        void start()
        {
            inventoryManager = GetComponent<InventoryManager>();
            textBoxManagerScript = GetComponent<TextBoxManagerScript>();
           
        }

        public string DisplayCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string DisplayText
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void DoActivate()
        {
            throw new NotImplementedException();
        }

        public void DoCommand()
        {
            throw new NotImplementedException();
        }
    
    }
}
