using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class TextBoxManagerScript : MonoBehaviour
    {

        // Use this for initialization
        [SerializeField]
        ThirdPersonCharacter thirdPersonCharacter;

        [SerializeField]
        ThirdPersonUserControl thirdPersonUserControl;

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
        int endAtLine;
        //Use playerscript to deactivate player when toggling through text...if you want to.
        

        [SerializeField]
        bool isActive;

        void Start()
        {
           
            if (textFile != null)
            {
                textLines = (textFile.text.Split('\n'));
            }
            if (endAtLine == 0)
            {
                endAtLine = textLines.Length - 1;
            }
            if (isActive)
            {
                EnableTextBox();
            }
            else
            {

                DisableTextBox();
            }
        }

        // Update is called once per frame
        void Update()
        {
            theText.text = textLines[currentLine];


            if (!isActive)
            {
                return;
            }
            if (Input.GetButtonDown("xButton"))
            {
                currentLine += 1;
            }
            if (currentLine > endAtLine)
            {
                DisableTextBox();
            }
        }
        public void EnableTextBox()
        {
            textBox.SetActive(true);
            if (isActive)
            {
               thirdPersonCharacter.enabled = false;
               thirdPersonUserControl.enabled = false;
            }
        }
        public void DisableTextBox()
        {
            textBox.SetActive(false);
        }
    }
}
