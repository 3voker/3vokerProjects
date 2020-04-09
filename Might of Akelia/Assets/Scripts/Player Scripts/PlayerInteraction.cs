using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//UI adding PlayerState PauseMenu
public enum PlayerInteractionState { Idle, Moving, Interacting, Talking, NavigateMenu }

[RequireComponent(typeof(PlayerController))]
public class PlayerInteraction : Rook
{
    //UI adding pause menu and health/magic sprites in inspector
    [HideInInspector]
    public GameObject pauseMenuPanel;
    [HideInInspector]
    public GameObject chatLogPanel;
    [HideInInspector]
    public GameObject partyFormationPanel;
    [HideInInspector]
    public GameObject questGuidePanel;
    public PlayerController playerController { get; private set; }
    private WeaponAnimator playerAnimator;

    public PlayerInteractionState CurrentPlayerInteractionState
    {
        get { return playerInteractionState; }
        set
        {
            playerInteractionState = value;
            PlayerInteractionStateChange();
        }
    }
    public bool isTalking;
    PlayerHealth playerHealth;
    //CombatState combatState;
    [SerializeField]
    private PlayerInteractionState playerInteractionState;

    public void PlayerInteractionStateChange()
    {
        switch (CurrentPlayerInteractionState)
        {
            case PlayerInteractionState.Idle:
                playerController.enabled = true;
                break;
            case PlayerInteractionState.Moving:
                playerController.enabled = true;
                break;
            case PlayerInteractionState.Interacting:
                playerController.enabled = false;
                break;
            case PlayerInteractionState.Talking:
                playerController.enabled = false;
                break;
            case PlayerInteractionState.NavigateMenu:
                playerController.enabled = false;
                break;
        }
    }

    PlayerInteractionState StateFunctions(PlayerInteractionState pState)
    {

        return pState;
    }
    protected override void OnAwake()
    {
        
     pauseMenuPanel = GameObject.Find("Pause Menu Panel");
     chatLogPanel = GameObject.Find("Chat Log Panel");
     partyFormationPanel = GameObject.Find("Party Formation Panel");
     questGuidePanel =  GameObject.Find("Quest Guide Panel");
     
     

        isTalking = chatLogPanel.activeSelf; 
        playerController = this.GetComponent<PlayerController>();
        playerAnimator = this.GetComponentInChildren<WeaponAnimator>();
        playerInteractionState = PlayerInteractionState.Idle;
        base.OnAwake();
    }

    protected override void OnUpdate()
    {
       // UIConsoleText.SetConsoleText("Player Health: " + playerHealth.m_CurrentHealth); //DEBUG HEALTH VISIBILIY
        if (isTalking)//(!chatLogPanel)
        {
            playerInteractionState = PlayerInteractionState.Talking;
        }
        else if (!pauseMenuPanel)
        {
            playerInteractionState = PlayerInteractionState.NavigateMenu;
        }
        else playerInteractionState = PlayerInteractionState.Idle;
        base.OnUpdate();
    }
   
    //protected override void ()
    //{
    //    if (animator != null)
    //        animator.PlaySwordAttack();
    //}

    //protected override void PlayBlockAnimation()
    //{
    //    if (animator != null)
    //        animator.PlaySwordBlock();
    //}

    //protected override void PlayUnBlockAnimation()
    //{
    //    if (animator != null)
    //        animator.PlaySwordUnBlock();
    //}
    //protected override void PlayCombatNothingAnimation()
    //{
    //    if (animator != null)
    //        animator.PlayNothing();
    //}
}
