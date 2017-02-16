using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Customers : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject textBox;

    [SerializeField]
    Text theText;

    [SerializeField]
    TextAsset textFile;

    [SerializeField]
    string[] textLines;

    public int currentLine;
    public int endAtLine;

    bool isTextBoxPresent;

    // public PlayerController player;

    void Start()
    {

        //player = FindObjectOfType<PlayerController>();  
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
            isTextBoxPresent = true;
        }
        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
    }

    // Update is called once per frame
    void Update()
    {

        TextToggle();

    }



    private void TextToggle()
    {
        theText.text = textLines[currentLine];
        if (Input.GetKeyDown(KeyCode.Return) && (isTextBoxPresent))
        {
            currentLine++;
        }
        if (currentLine > endAtLine)
        {
            textBox.SetActive(false);
            isTextBoxPresent = false;
        }
    }
}
