using UnityEngine;
using System.Collections;

public class CharacterLoader : MonoBehaviour
{
    #region Magic Number
    private int magicNumber = 32;
    public int MagicNumber { get { return magicNumber; } }
    #endregion
    public string characterNameToLoad;
    public string path = "Data/Dialogue/";
    public CharacterDialogue dialogueData;
    public int currentNode = 0;

    void Awake()
    {
        path = path + characterNameToLoad + "_" + DialogueManagement.Language;
        dialogueData = XMLFileHandler.Instance.LoadCharacterData(path);
    }

}
