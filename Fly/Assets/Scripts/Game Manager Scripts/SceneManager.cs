using UnityEngine;
using System.Collections;
using System;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class SceneManager : MonoBehaviour, IActivatable
    {

        // Use this for initialization
        
        InventoryManager inventoryManager;
        TextBoxManagerScript textBoxManagerScript;

        void start()
        {
            inventoryManager = GetComponent<InventoryManager>();
            textBoxManagerScript = GetComponent<TextBoxManagerScript>();
           
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

     
    
    }
}
