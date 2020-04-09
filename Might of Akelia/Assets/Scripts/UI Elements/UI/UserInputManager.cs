using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    GameObject userInputStance;

    [SerializeField]
    GameObject userInputIconSelect;

    CombatState userInputState;

    public string UserInputChoiceChange(CombatState state)
    {
        switch (userInputState)
        {
            case CombatState.AttackState:
                return CombatState.AttackState.ToString();
        }
        return "";
    }
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
