using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechniqueManager : MonoBehaviour {

    /// <summary>
    /// Enum that toggles between menu states that display weapon techniques, mana techniques, and job techniques 
    /// </summary>
    protected enum TechniqueScreenMenuState { ClosedMenu, OpenMenu, TechniqueMenu, ManaTechniqueMenu, JobTechniques }

    #region Technique Menu Variables 
    /// <summary>
    /// Technique Menu State 
    /// </summary>
    //Reference to TechniqueState 
    TechniqueScreenMenuState techniqueMenuState;

    [SerializeField]
    TechniqueScreenMenuState currentTechniqueMenuState;

    /// <summary>
    /// Previous Window of MenuManagerState 
    /// </summary>
    [SerializeField]
    TechniqueScreenMenuState previousTechniqueMenuState;

    /// <summary>
    /// Gets Current Main Menu State
    /// </summary>
    protected TechniqueScreenMenuState CurrentTechniqueMenuState
    {
        get { return currentTechniqueMenuState; }
        set
        {
            previousTechniqueMenuState = currentTechniqueMenuState;
            currentTechniqueMenuState = value;
            TechniqueMenuScreenChange(value);
        }
    }
    #endregion
    #region Technique Menu State Machine 
    /// <summary>
    /// Controls main menu screen change
    /// </summary>
    protected TechniqueScreenMenuState TechniqueMenuScreenChange(TechniqueScreenMenuState menu)
    {
        switch (CurrentTechniqueMenuState)
        {
            case TechniqueScreenMenuState.ClosedMenu:

        
                return previousTechniqueMenuState = menu;  
        }
        return previousTechniqueMenuState = menu;
    }

    #endregion
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
