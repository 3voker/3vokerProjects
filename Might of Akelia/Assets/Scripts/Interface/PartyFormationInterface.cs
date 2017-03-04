using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PartyFormationInterface : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Button button;
    [SerializeField]
    Text buttonText;
    [SerializeField]
    string playerSide;


    public void SetSpace()
    {
        buttonText.text = playerSide;
        button.interactable = false;
    }
    
}
