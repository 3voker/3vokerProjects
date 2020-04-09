using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

    public class ChatLogManager : MonoBehaviour {
    // Use this for initialization

        private CharacterLoader charLoader;
        private NPCDialogueGUI npcDialogueGui;
         [SerializeField]
        ThirdPersonPlayerCharacter thirdPersonPlayerCharacter;
        [SerializeField]
        PlayerController playerController;

        [SerializeField]
        GameObject chatLogPanel;

        [SerializeField]
        GameObject textBox;

        [SerializeField]
        GameObject picBox;
        
        [SerializeField]
        Text theText;

        [SerializeField]
        TextAsset textFile;

        [SerializeField]
        string[] textLines;

        [SerializeField]
        int currentLine;

        [SerializeField]
        Text[] ChatLog; 

        [SerializeField]
        int endAtLine;
        //Use playerscript to deactivate player when toggling through text...if you want to.

        [SerializeField]
        bool isActive;

        bool IsChatLogShowing
        {
            get { return chatLogPanel.activeSelf; }
        }
        void Start()
        {
      
        HidePanel();
        }
        // Update is called once per frame
        void Update()
        {
       
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }
        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
        if (IsChatLogShowing)
        {
            EnableTextBox();
        }
        else
        {
          //  DisableTextBox();
        }      
           ReadTextFile();
        HandleInput();
    }
        private void ReadTextFile()
        {
            theText.text = textLines[currentLine];
            if (!isActive)
            {
                return;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                currentLine += 1;
            }
            if (currentLine > endAtLine || (Input.GetButtonDown("bButton")))
            {
                DisableTextBox();
            }
           // SendTextFileToChatLog();
        }
        public void EnableTextBox()
        {
            textBox.SetActive(true);
            if (IsChatLogShowing)
            {
                thirdPersonPlayerCharacter.enabled = false;
                playerController.enabled = false;
            }
        }
    void HandleInput()
    {
        if (IsChatLogShowing)
        {
            if (Input.anyKeyDown)
            {
                switch (Input.inputString)
                {
                    case "bButton":
                        Debug.Log("bButton pressed");
                        //HidePanel();
                        DisableTextBox();
                        break;
                    case "Select":
                        HidePanel();
                        break;
                    default:
                        Debug.Log("This is not a valid key");
                        HidePanel();
                        break;
                }
            }
        }
        else if ((Input.GetButton("Select")) && !IsChatLogShowing)
        {
            ShowPanel();
        }      
    }
    private void ShowPanel()
    {
        chatLogPanel.SetActive(true);
       // chatLogPanel.transform.position = thirdPersonPlayerCharacter.transform.position;
        //thirdPersonPlayerCharacter.enabled = false;
        //playerController.enabled = false;
    }
    private void HidePanel()
    {
        chatLogPanel.SetActive(false);
        thirdPersonPlayerCharacter.enabled = true;
        playerController.enabled = true;
    }
    public void DisableTextBox()
        {
            textBox.SetActive(false);
        }
    }


