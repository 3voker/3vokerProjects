using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Barks { Surprised, Agreement, Laughing, Insulted }
public class NPCDialogueGUI : MonoBehaviour
{
    private CharacterLoader charLoader;
    private NPC npc;

    public Text dialogueText;
    public Button[] optionsButtons = new Button[4];
    //Keeping track of randomized shit
    private int[] optionsButtonDestinations = new int[4];
    private Barks[] optionsButtonBarks = new Barks[4];

    //Random gen
    private List<int> numsGened = new List<int>();
    int random = 0;

    public void PopulateGUI(CharacterLoader loader)
    {
        charLoader = loader;
        npc = loader.gameObject.GetComponent<NPC>();
        UpdateGUI();
    }

    public void UpdateGUI()
    {
        numsGened.Clear();
        dialogueText.text = charLoader.dialogueData.Nodes[charLoader.currentNode].text;
        RandomizeOptions();
    }

    private void RandomizeOptions()
    {
        int optionsCount = charLoader.dialogueData.Nodes[charLoader.currentNode].Options.Count;
        ButtonVisibility(optionsCount);

        if(optionsCount > 1)
        {
            for (int x = 0; x < optionsCount; x++)
            {
                while (numsGened.Contains(random))
                { random = Random.Range(0, optionsCount); }
                numsGened.Add(random);
                SetOptionsButtonGUI(x, random);
            }
        }
        else
        {
            SetOptionsButtonGUI(0, 0);
        }
    }

    private void SetOptionsButtonGUI(int buttonIndex, int optionIndex)
    {
        optionsButtons[buttonIndex].GetComponentInChildren<Text>().text = charLoader.dialogueData.Nodes[charLoader.currentNode].Options[optionIndex].text;
        optionsButtonDestinations[buttonIndex] = charLoader.dialogueData.Nodes[charLoader.currentNode].Options[optionIndex].destinationNodeID;
        optionsButtonBarks[buttonIndex] = (Barks)charLoader.dialogueData.Nodes[charLoader.currentNode].Options[optionIndex].bark;
    }

    private void ButtonVisibility(int optionsCount)
    {
        switch (optionsCount)
        {
            case 1:
                optionsButtons[1].gameObject.SetActive(false);
                optionsButtons[2].gameObject.SetActive(false);
                optionsButtons[3].gameObject.SetActive(false);
                break;
            case 2:
                optionsButtons[1].gameObject.SetActive(true);
                optionsButtons[2].gameObject.SetActive(false);
                optionsButtons[3].gameObject.SetActive(false);
                break;
            case 3:
                optionsButtons[1].gameObject.SetActive(true);
                optionsButtons[2].gameObject.SetActive(true);
                optionsButtons[3].gameObject.SetActive(false);
                break;
            case 4:
                optionsButtons[1].gameObject.SetActive(true);
                optionsButtons[2].gameObject.SetActive(true);
                optionsButtons[3].gameObject.SetActive(true);
                break;
        }
    }

    public void OptionsButton(int buttonIndex)
    {
        if(optionsButtonDestinations[buttonIndex] != charLoader.MagicNumber)
        {
            charLoader.currentNode = optionsButtonDestinations[buttonIndex];
            UpdateGUI();
        }
        else
        {
            npc.ExitDialogue();
        }
        //Play optionButtonBarks[buttonIndex]
    }
    public void ExitDialogueOption()
    {
        npc.ExitDialogue();
    }

}
