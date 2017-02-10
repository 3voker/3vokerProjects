using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

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

   // public PlayerController player;

    void Start()
    {
        //player = FindObjectOfType<PlayerController>();  
        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }
        if(endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        theText.text = textLines[currentLine];
        if(Input.GetKeyDown(KeyCode.Return))
        {
            currentLine++;
        }
        if(currentLine > endAtLine)
        {
            textBox.SetActive(false);
        }
    }
}
