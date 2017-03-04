using UnityEngine;
using System.Collections;
using System;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class PauseMenuManager : MonoBehaviour
    {

        // Use this for initialization
        [SerializeField]
        GameObject pauseMenuPanel;
        [SerializeField]
        ThirdPersonCharacter thirdPersonCharacter;

        [SerializeField]
        ThirdPersonUserControl thirdPersonUserControl;

        [SerializeField]
        Camera playerCamera;
        bool paused = false;
        bool myCheck = false;
      
        bool IsPauseMenuShowing
        {
            get { return pauseMenuPanel.activeSelf; }
        }
        void Start()
        {
            paused = false;
            HidePauseMenu();
        }

        private void HidePauseMenu()
        {
            pauseMenuPanel.SetActive(false);
            thirdPersonCharacter.enabled = true;
            thirdPersonUserControl.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {        
            HandleInput();
            UpdateCursor();
           // UpdateThirdPersonController();
        }
        void HandleInput()
        {
            //If bool is true and start button pressed. Pause game. Freeze time.
            //Else will unpause and hide the pause menu.
            if (Input.GetButton("startButton") && IsPauseMenuShowing)
            {              
                    Debug.Log("Closing Pause Menu");
                    HidePauseMenu();
                    Time.timeScale = 1;
                    paused = false;                                                               
            }
            else if (Input.GetButton("startButton") && !IsPauseMenuShowing)
            {
                ShowPauseMenu();
                Time.timeScale = 0;
                paused = true;
                Debug.Log("Hiding Pause Menu");              
            }
        }
        private void ShowPauseMenu()
        {          
            pauseMenuPanel.SetActive(true);
            thirdPersonCharacter.enabled = false;
            thirdPersonUserControl.enabled = false;
        }
        //private void UpdateThirdPersonController()
        //{
        //    if (IsPauseMenuShowing)
        //    {
        //        thirdPersonCharacter.enabled = false;
        //        thirdPersonUserControl.enabled = false;
        //    }
        //    else
        //    {
        //        thirdPersonCharacter.enabled = true;
        //        thirdPersonUserControl.enabled = true;
        //    }
        //}
        private void UpdateCursor()
        {
            if (IsPauseMenuShowing)
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
