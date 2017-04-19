using UnityEngine;
using System.Collections;
using System;
using System.Linq;
namespace UnitySampleAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(Animator))]

    public class Door : MonoBehaviour, IActivatable// Icommand
    {

        InventoryManager inventoryManager;
        TextBoxManagerScript textBoxManagerScript;

        [SerializeField]
        bool isLocked;

        [SerializeField]
        InventoryObject key;

        Animator animator;
      //  AudioSource audio;

        bool isOpen = false;
        void Start()
        {
            //    audio = GetComponent<AudioSource>();

            animator = GetComponent<Animator>();
            //try
            //{
            //    inventoryManager = GameObject.Find("Inventory Manager").GetComponent<InventoryManager>();
            //}
            //catch (System.Exception)
            //{

            //    throw new System.Exception("Scene requires game object named"
            //        + "Inventory Manager with an Inventory Manager");
            //}

        }


        //    bool hasKey;

        //Display Text is a property variable
        public string DisplayText
        {
            get
            {
                if (isLocked && !HasKey)
                 return "Locked Door";
                else if (isOpen)
                    return "";
                else if (isLocked && HasKey)
                    return "Unlock door";
                else if (!isLocked && isOpen == false)
                    return "Closed Door";
                else
                    return "Opened Door";
            }

        }
        //HasKey is a property variable
        private bool HasKey
        {
            get
            {
                return inventoryManager.InventoryObjects.Contains(key);
            }
        }

        public string DisplayCommand
        {
            get
            {
                if (isLocked && !HasKey)
                    return "Cannot Open Door";
                else if (isOpen)
                    return "";
                else if (isLocked && HasKey)
                    return "Unlocking door";
                else if (!isLocked && isOpen == false)
                    return "Opening Door!"; 
                else
                    return "Closing Door";

            }
        }

        public void DoActivate()
        {

            //Check the players inventory for the key. 
            //if they have it, open the door.
            //Otherwise, leave it locked.


            if (isLocked && HasKey || !isLocked)
            {

                //hasKey = inventoryManager.InventoryObjects.Contains(key);
                if (HasKey && isOpen == false)
                {
               //     audio.Play("");
                    openDoor();
                    isOpen = true;
                }
                else if (HasKey && isOpen == true)
                {
                    openDoor();
                    isOpen = false;
                }

            }
           //     audio.Play("");
            // animator.SetBool("shouldOpen", true);
        }

        private void openDoor()
        {
            if (isOpen == false)
            {
                // openDoor();
                animator.SetBool("shouldOpen", true);
              //  audio.Play("");

            }
            else if (isOpen == true)
            {
                animator.SetBool("shouldOpen", false);
               // audio.Play("");
            }

            //  animator.SetBool("shouldOpen", true);
        }


        // Use this for initialization

        public void DoCommand()
        {
            DoActivate();
        }
        // Update is called once per frame
    }
}
