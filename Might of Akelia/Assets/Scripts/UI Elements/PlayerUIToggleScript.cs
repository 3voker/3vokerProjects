using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIToggleScript : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject ChatLogPanel;
    [SerializeField]
    GameObject UserInputPanel;
    [SerializeField]
    GameObject MiniMapPanel;
    [SerializeField]
    GameObject PartyMenu;
    [SerializeField]
    GameObject QuestLogPanel;
    [SerializeField]
    GameObject InventoryMenuPanel;

    Selection currentSel; // Create a Selection object that will be used throughout script

    Image imgCenterDown, imgLeft, imgBottomLeft,  imgRight, imgTopRight, imgBottomRight; //These variables will be used for fading the buttons when selected
    Button buttonUp, buttonDown, buttonLeft, buttonRight; //Will be used to invoke Button functions
    enum Selection { None, ChatLogPanel, UserInputPanel, MiniMapPanel, QuestLogPanel, InventoryMenuPanel };
    public AudioSource audioSource;
    [SerializeField]
    float fadeRate = 4f;

    void Start () {

        currentSel = Selection.None;
        InventoryMenuPanel.SetActive(false);
        //imgUp = transform.FindChild("Chat Log Panel").GetComponent<Image>();
        imgCenterDown = transform.Find("Chat Log Panel").GetComponent<Image>();
        imgBottomRight = transform.Find("User Input Panel").GetComponent<Image>();
        imgTopRight = transform.Find("MapPanel").GetComponent<Image>();
        imgLeft = transform.Find("Quest Log Panel").GetComponent<Image>();
        imgRight = transform.Find("Inventory Menu Panel").GetComponent<Image>();
        //Grab the Button components of all our buttons

        //buttonUp = transform.FindChild("Chat Log Panel").GetComponent<Button>();
        buttonDown = transform.Find("Chat Log Panel").GetComponent<Button>();
        buttonLeft = transform.Find("User Input Panel").GetComponent<Button>();
        buttonRight = transform.Find("MapPanel").GetComponent<Button>();
        buttonLeft = transform.Find("Quest Log Panel").GetComponent<Button>();
        buttonRight = transform.Find("Inventory Menu Panel").GetComponent<Button>();
    }
    void Update()
    {
        //Standard input calls.
        //While Interface select button is pressed. Pressing any corresponding button will navigate the user through UI's. 
        //Arrows to toggle through the UI. 
        //Jump button to select Panel for player to activate. 
        //Another alternative is hot key buttons to immediately select a corresponding menu. 
        if (Input.GetButton("Select"))
        {
            if (Input.GetButtonDown("yButton"))
            {
                if (currentSel == Selection.ChatLogPanel)
                {
                    //Executes if we already have up selected and user presses up again
                    buttonDown.onClick.Invoke(); //Call up button's OnClick() function
                    currentSel = Selection.None; //set currentSel back to None
                }
                else
                {
                    currentSel = Selection.ChatLogPanel; // changes currentSel to Up.
                    StartCoroutine(FadeIcon(imgCenterDown, currentSel)); //Begins fading the icon
                }
            }
            //The same code pattern from above is repeated for the rest of the inputs
            else if (Input.GetButtonDown("Jump"))
            {
                if (currentSel == Selection.UserInputPanel)
                {
                    buttonDown.onClick.Invoke();
                    currentSel = Selection.None;
                }
                else
                {
                    currentSel = Selection.UserInputPanel;
                    StartCoroutine(FadeIcon(imgCenterDown, currentSel));
                }
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                if (currentSel == Selection.MiniMapPanel)
                {
                    buttonLeft.onClick.Invoke();
                    currentSel = Selection.None;
                }
                else
                {
                    currentSel = Selection.MiniMapPanel;
                    StartCoroutine(FadeIcon(imgTopRight, currentSel));
                }
            }
            else if (Input.GetButtonDown("bButton"))
            {
                if (currentSel == Selection.QuestLogPanel)
                {
                    buttonRight.onClick.Invoke();
                    currentSel = Selection.None;
                }
                else
                {
                    currentSel = Selection.QuestLogPanel;
                    StartCoroutine(FadeIcon(imgLeft, currentSel));
                }
            }
            else if (Input.GetButtonDown("bButton"))
            {
                if (currentSel == Selection.InventoryMenuPanel)
                {
                    buttonRight.onClick.Invoke();
                    currentSel = Selection.None;
                }
                else
                {
                    currentSel = Selection.InventoryMenuPanel;
                    StartCoroutine(FadeIcon(imgRight, currentSel));
                }
            }
        }
    }

    IEnumerator FadeIcon(Image img, Selection sel)
    {
        //basic Fade Coroutine. For more Information:
        //https://www.studica.com/blog/create-a-fading-splash-screen-using-coroutines-in-unity-3d
        float alpha = 1f;

        while (currentSel == sel)
        {
            while (img.color.a > 0)
            {
                alpha -= Time.deltaTime * fadeRate;
                img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
                yield return null;
            }
            while (img.color.a < 1)
            {
                alpha += Time.deltaTime * fadeRate;
                img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
                yield return null;
            }
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
    }
    public void clickSound()
    {
        audioSource.Play();
    }
}
// Update is called once per frame



