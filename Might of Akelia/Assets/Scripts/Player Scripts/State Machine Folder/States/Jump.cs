using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jump : IState
{
    private LayerMask searchLayer;

    private GameObject ownerGameObject;

    //Radius in which class searches for
    private float jumpRadius;

    //Tag on game object
    private string tagToLookFor;

    public bool jumpCompleted;

    private NavMeshAgent navMeshAgent;

    private System.Action<JumpResults> jumpResultsCallBack;

    //Searches layer, ownerGameObject, radius of search, tag to look for, and activates NavMeshAgent to interact(Move, attack, etc..)
    public Jump(LayerMask searchLayer, GameObject ownerGameObject, float jumpRadius, string tagToLookFor, NavMeshAgent navMeshAgent, System.Action<JumpResults> jumpResultsCallBack)
    {
        this.searchLayer = searchLayer;
        this.ownerGameObject = ownerGameObject;
        this.jumpRadius = jumpRadius;
        this.tagToLookFor = tagToLookFor;
        this.navMeshAgent = navMeshAgent;
        this.jumpResultsCallBack = jumpResultsCallBack;
    }

    public void Enter()
    {
        //Entering the state of Search for should toggle searchCompleted to false to initiate the ExecuteMEthod
        jumpCompleted = false;
    }

    public void Execute()
    {
        if (!jumpCompleted)
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

            var jumpResults = new JumpResults(hitObjects, allObjectsWithTheRequiredTag);
            //This is where we should send the information back.
            this.jumpResultsCallBack(jumpResults);

            this.jumpCompleted = true;
        }
    }

    public void Exit()
    {

    }
}

public class JumpResults
{
    public Collider[] allHitObjectsInSearchRadius;
    public List<Collider> AllHitObjectsWithRequiredTag;
    //Closest object

    //Farthest object

    public JumpResults(Collider[] allHitObjectsInSearchRadius, List<Collider> AllHitObjectsWithRequiredTag)
    {
        this.allHitObjectsInSearchRadius = allHitObjectsInSearchRadius;
        this.AllHitObjectsWithRequiredTag = AllHitObjectsWithRequiredTag;
    }
}
