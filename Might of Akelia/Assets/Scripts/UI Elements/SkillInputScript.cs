using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillInputScript : MonoBehaviour
{

    [SerializeField]
    float fadeRate = 4f; //Used to adjust image fade speed

    enum Selection { None, Up, Down, Left, Right }; //Will be used to keep track of what's selected
    Selection currentSel; // Create a Selection object that will be used throughout script

    Image imgUp, imgDown, imgLeft, imgRight; //These variables will be used for fading the buttons when selected
    Button buttonUp, buttonDown, buttonLeft, buttonRight; //Will be used to invoke Button functions

    void Start()
    {
        currentSel = Selection.None; //assign currentSel to None.

        //Grab the Image components of all our buttons
        imgUp = transform.Find("YButton").GetComponent<Image>();
        imgDown = transform.Find("AButton").GetComponent<Image>();
        imgLeft = transform.Find("XButton").GetComponent<Image>();
        imgRight = transform.Find("BButton").GetComponent<Image>();

        //Grab the Button components of all our buttons
        buttonUp = transform.Find("YButton").GetComponent<Button>();
        buttonDown = transform.Find("AButton").GetComponent<Button>();
        buttonLeft = transform.Find("XButton").GetComponent<Button>();
        buttonRight = transform.Find("BButton").GetComponent<Button>();
    }

    void Update()
    {
        //Standard input calls.
        if (Input.GetButtonDown("yButton"))
        {
            if (currentSel == Selection.Up)
            {
                //Executes if we already have up selected and user presses up again
                buttonUp.onClick.Invoke(); //Call up button's OnClick() function
                currentSel = Selection.None; //set currentSel back to None
            }
            else
            {
                currentSel = Selection.Up; // changes currentSel to Up.
                StartCoroutine(FadeIcon(imgUp, currentSel)); //Begins fading the icon
            }
        }
        //The same code pattern from above is repeated for the rest of the inputs
        else if (Input.GetButtonDown("Jump"))
        {
            if (currentSel == Selection.Down)
            {
                buttonDown.onClick.Invoke();
                currentSel = Selection.None;
            }
            else
            {
                currentSel = Selection.Down;
                StartCoroutine(FadeIcon(imgDown, currentSel));
            }
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            if (currentSel == Selection.Left)
            {
                buttonLeft.onClick.Invoke();
                currentSel = Selection.None;
            }
            else
            {
                currentSel = Selection.Left;
                StartCoroutine(FadeIcon(imgLeft, currentSel));
            }
        }
        else if (Input.GetButtonDown("bButton"))
        {
            if (currentSel == Selection.Right)
            {
                buttonRight.onClick.Invoke();
                currentSel = Selection.None;
            }
            else
            {
                currentSel = Selection.Right;
                StartCoroutine(FadeIcon(imgRight, currentSel));
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
}