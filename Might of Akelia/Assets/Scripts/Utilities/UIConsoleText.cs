using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIConsoleText : MonoBehaviour
{
    private static Text consoleText;

    void Awake() { consoleText = this.GetComponent<Text>(); }

    public static void SetConsoleText(string newText)
    {
        consoleText.text = newText;
    }

    public static void AddNewConsoleTextLine(string newText)
    {
        consoleText.text += newText;
    }

}
