using UnityEngine;
using System.Collections;
using System;

public class NPC : MonoBehaviour, IIdentifiable, IInteractable
{
    //Autumn Script additions
    //private PlayerController controller;
    //private NPCDialogueGUI dialogueGUIFiller;
    //private CharacterLoader characterLoader;
    //[SerializeField]
    //GameObject dialoguePanel;
    //[SerializeField]
    //GameObject MiniMapPanel;
    //[SerializeField]
    //GameObject userInputPanel;
    //[SerializeField]
    //GameObject partyInfoPanel;
    //[SerializeField]
    //GameObject questLogPanel;
    //[SerializeField]
    //GameObject inventoryMenuPanel;
    //[SerializeField]
    //Transform npcCamera;
    //[SerializeField]
    //Camera mainCamera;
    //[SerializeField]
    //float speed;
    //Vector3 newPosition;
    //Vector3 startingPosition;

    GameObject player;
    [SerializeField]
    string displayText;
    [SerializeField]
    string descriptionText;
    [SerializeField]
    string displayCommand;

    public string DisplayName
    {
        get
        {
            return displayText;
        }
    }   
    public string DisplayInput
    {
        get
        {
            return displayCommand;
        }
    }   
    public void DoCommand()
    {
        //Animator anim = player.GetComponent<Animator>();
        //anim.Play("");
        Debug.Log("NPC script Do Command!");
        DoActivate();       
     }
    void Start()
    {
        //Autumn addition
        //npcCamera = this.gameObject.transform.GetChild(0);
        //controller = GameObject.FindObjectOfType<PlayerController>();
        //characterLoader = this.GetComponent<CharacterLoader>();
        //dialogueGUIFiller = GameObject.FindObjectOfType<NPCDialogueGUI>();   
        //startingPosition = npcCamera.transform.position;
        //npcCamera.gameObject.SetActive(false);       
        //dialogueCanvas.gameObject.SetActive(false);
    }
       
public void DoActivate()
    {
        Debug.Log("NPC script Do Activate!");
        //Autumn addition
        //dialogueGUIFiller.PopulateGUI(characterLoader);
        //controller.enabled = false; //stop player from moving in dialogue
        //MiniMapPanel.SetActive(false);
        //userInputPanel.SetActive(false);   
        //partyInfoPanel.SetActive(false);   
        //questLogPanel.SetActive(false);   
        //inventoryMenuPanel.SetActive(false);
        //mainCamera.gameObject.SetActive(false);
        //npcCamera.gameObject.SetActive(true);
        //dialoguePanel.SetActive(true);
        //Debug.Log("Player is talking to the NPC.");
        //ZoomIn();
        Interact(player);
    }
    public void ExitDialogue()
    {
        //MiniMapPanel.SetActive(true);
        //dialoguePanel.SetActive(false);
       // userInputPanel.SetActive(true);
        //partyInfoPanel.SetActive(true);
        //questLogPanel.SetActive(true);
        //inventoryMenuPanel.SetActive(false);
        //npcCamera.gameObject.SetActive(false);
        //mainCamera.gameObject.SetActive(true);
        //controller.enabled = true; //return player movement
    }
    public void ZoomOut()
    {
        //float step = speed * Time.deltaTime;
        //Move back to player after finishing dialogue
        //Vector3.zero; 
       // npcCamera.transform.position = startingPosition;//Vector3.MoveTowards(transform.position, startingPosition, step);
    }
    //private void ZoomIn()
    //{
    //    newPosition.x = gameObject.transform.position.x;
    //    newPosition.y = gameObject.transform.position.y + 1f;
    //    newPosition.z = gameObject.transform.position.z + 2f;

    //    float step = speed * Time.deltaTime;
    //    npcCamera.transform.position = newPosition;//Vector3.MoveTowards(transform.position, newPosition, step);
    //}

    public void Interact(GameObject interactable)
    {
        Debug.Log("Interacted With Player.");
        interactable = this.gameObject;
        
    }

    public bool RevealInteract(GameObject agent)
    {
        throw new NotImplementedException();
    }
}
