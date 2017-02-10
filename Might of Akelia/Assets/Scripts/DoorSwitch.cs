using UnityEngine;
using System.Collections;
using System;
namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class DoorSwitch : MonoBehaviour, IActivatable
    {

        [SerializeField]
        string displayText;
        [SerializeField]
        GameObject associatedDoor;
        [SerializeField]
        bool isReusable = true;

        Door door;

        bool hasBeenUsed = false;



        public string DisplayText
        {
            get
            {
                if (!isReusable && hasBeenUsed)
                    return "";
                else

                    return displayText;
            }
        }

        public void DoActivate()
        {
            if (!isReusable && hasBeenUsed)
            {
                hasBeenUsed = true;
                return;
            }

            else if (isReusable && hasBeenUsed)
            {
                hasBeenUsed = false;
                associatedDoor.GetComponent<Door>().DoActivate();
                return;
            }
            associatedDoor.gameObject.SendMessage("openDoor");
        }       
    }
}
