using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : IState {

    private LayerMask searchLayer;

    private GameObject ownerGameObject;

    //Radius in which class searches for
    private float jumpRadius;

    //Tag on game object
    private string tagToLookFor;

    public bool idleCompleted;

    private NavMeshAgent navMeshAgent;

    private System.Action<IdleResults> idleResultsCallBack;

    //Searches layer, ownerGameObject, radius of search, tag to look for, and activates NavMeshAgent to interact(Move, attack, etc..)
    public Idle(LayerMask searchLayer, GameObject ownerGameObject, float jumpRadius, string tagToLookFor, NavMeshAgent navMeshAgent, System.Action<IdleResults> idleResultsCallBack)
    {
        this.searchLayer = searchLayer;
        this.ownerGameObject = ownerGameObject;
        this.jumpRadius = jumpRadius;
        this.tagToLookFor = tagToLookFor;
        this.navMeshAgent = navMeshAgent;
        this.idleResultsCallBack = idleResultsCallBack;
    }

    public void Enter()
    {
        //Entering the state of Search for should toggle searchCompleted to false to initiate the ExecuteMEthod
        idleCompleted = false;
    }

    public void Execute()
    {
        if (!idleCompleted)
        {
            var hitObjects = Physics.OverlapSphere(this.ownerGameObject.transform.position, this.jumpRadius);

            var allObjectsWithTheRequiredTag = new List<Collider>();

            for (int i = 0; i < hitObjects.Length; i++)
            {
                if (hitObjects[i].CompareTag(this.tagToLookFor))
                {
                    // this.navMeshAgent.SetDestination(hitObjects[i].transform.position);

                    allObjectsWithTheRequiredTag.Add(hitObjects[i]);
                }
                break;
            }

            var idleResults = new IdleResults(hitObjects, allObjectsWithTheRequiredTag);
            //This is where we should send the information back.
            this.idleResultsCallBack(idleResults);
            
            this.idleCompleted = true;
        }
    }

    public void Exit()
    {

    }
}

public class IdleResults
{
    public Collider[] allHitObjectsInSearchRadius;
    public List<Collider> AllHitObjectsWithRequiredTag;
    //Closest object

    //Farthest object

    public IdleResults(Collider[] allHitObjectsInSearchRadius, List<Collider> AllHitObjectsWithRequiredTag)
    {
        this.allHitObjectsInSearchRadius = allHitObjectsInSearchRadius;
        this.AllHitObjectsWithRequiredTag = AllHitObjectsWithRequiredTag;
    }
}
