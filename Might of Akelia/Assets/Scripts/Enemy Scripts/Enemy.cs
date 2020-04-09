using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public enum EnemyState { Idle, Wandering, WanderingAction, NoticePlayer, ChasePlayer }

public class Enemy : CombatController, IIdentifiable, ITargetable
{
    public bool canBlock;
    public bool canDodge;
    public bool isTargeted;

    [SerializeField]
    string displayText;

    [SerializeField]
    string descriptionText;

    [SerializeField]
    string displayCommand;

    GameObject mainTargetCursor;

    GameObject subTargetCursor;

    protected EnemyController enemyController;
    public SwordAnimator swordAnimator;
    CombatState enemyCombatState;
    //Enemy State
    public EnemyState State
    {
        get { return state; }
        set
        {
            state = value;
            StateChange();
        }
    }
    private EnemyState state;
    #region Interfaces IIdentifiable/ITargetable

    public string DisplayName
    {
        get
        {
            return this.gameObject.name;
        }
    }
    public string DisplayInput
    {
        get
        {
            return "Enemy Spotted! Press X to hit!";
        }
    }

    public GameObject MainTarget
    {
        get
        {
            return mainTargetCursor;
        }
    }

    public GameObject SubTarget
    {
        get
        {
            return subTargetCursor;
        }
    }
    #endregion
    protected override void OnStart()
    {
        // UIConsoleText.SetConsoleText("Enemy Health: " + this.HealthPoints); //DEBUG HEALTH VISIBILIY
        OnUpdate();
        mainTargetCursor = GameObject.Find("Main Target Cursor");

         subTargetCursor = GameObject.Find("Sub Target Cursor");
        base.OnStart();
    }
    protected override void OnAwake()
    {
        // enemyController = this.GetComponentInParent<EnemyController>();
        swordAnimator = this.GetComponentInChildren<SwordAnimator>();
        base.OnAwake();
    }
    protected override void OnUpdate()
    {
        //Debug.Log("inFrontOfPlayer " + controller.InFrontOFPlayer);
        //if (enemyController.inRangeOfPlayer && enemyController.AtDestination)
        //{ this.SetTarget(GameObject.FindObjectOfType<PlayerInteraction>()); }
        //else { this.SetTarget(null); }

        CombatSequence();

        base.OnUpdate();
    }
    protected virtual void CombatSequence()
    {
        if (this.targetToAttack != null || isTargeted)
        {
           // mainTargetCursor.transform.position = Vector3.Lerp(mainTargetCursor.transform.position, transform.position, Time.deltaTime);
            if (this.CurrentCombatState != CombatState.AttackState)
            {
                this.CurrentCombatState = CombatState.AttackState;
            }
            else
            {
                EnemyBlockOrDodgeChance();
            }
        }
        else
            this.CurrentCombatState = CombatState.IdleCombatState;
    }
    private void EnemyBlockOrDodgeChance()
    {
        int rnd = UnityEngine.Random.Range(0, 3);
        switch (rnd)
        {
            case 0:
                if (canBlock)
                { this.CurrentCombatState = CombatState.DefensiveStanceState; }
                break;
            case 1:
                if (canDodge)
                { this.CurrentCombatState = CombatState.EvadeState; }
                break;
            case 2:
                //do nothing - keep attacking and take the hit
                break;
        }
    }

    protected void StateChange()
    {
        switch (State)
        {
            case EnemyState.Idle:
                enemyController.enabled = true;
                break;
            case EnemyState.Wandering:
                enemyController.enabled = true;
                break;
            case EnemyState.WanderingAction:
                enemyController.enabled = false;
                break;
            case EnemyState.NoticePlayer:
                enemyController.enabled = false;
                break;
            case EnemyState.ChasePlayer:
                enemyController.enabled = false;
                break;
        }
    }

    public void DoActivate() //Later Add IDamageable/IKillable
    {
        Debug.Log("Attacking Enemy" + this.name);
        isTargeted = true;
        if (mainTargetCursor != null)
        {
            mainTargetCursor.SetActive(true);
        }
    }
    //protected override void PlayAttackAnimation()
    //{

    //    if (animator != null)
    //     animator.PlaySwordAttack(); 
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