using UnityEngine;
using System.Collections;
using System;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class Rook : MonoBehaviour
    {
        // JobClass job = new JobClass();

        // Use this for initialization
        float timePressed; 


        void Start()
        {
            timePressed = 3; 
            PlayerInput();
        }

        private void PlayerInput()
        {
            if(Input.GetAxis("LeftTrigger") > 0 && (Input.GetButtonDown("Xbutton")))
                {
               // JobClassToggle.changeJob();
                  }
        }

        // Update is called once per frame
        void Update()
        {
           
        }
    }
}
