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

        bool IsPauseMenuShowing
        {
            get { return pauseMenuPanel.activeSelf; }
        }
        void Start()
        {
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
            UpdateThirdPersonController();
        }
        void HandleInput()
        {
            if (Input.GetButton("startButton"))
            {
                if (IsPauseMenuShowing)
                {
                    HidePauseMenu();
                }
                else
                {
                    ShowPauseMenu();
                }
            }
        }
        private void ShowPauseMenu()
        {
           
            pauseMenuPanel.SetActive(true);
            thirdPersonCharacter.enabled = false;
            thirdPersonUserControl.enabled = false;
        }
        private void UpdateThirdPersonController()
        {
            if (IsPauseMenuShowing)
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
