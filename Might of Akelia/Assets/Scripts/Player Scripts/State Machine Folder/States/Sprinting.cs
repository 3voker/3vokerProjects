using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinting : IState
{
    private LayerMask sprintLayer;

    private GameObject ownerGameObject; 

    private ThirdPersonPlayerCharacter thirdPersonPlayerCharacter;

    private float dashRadius;

    private string tagToLookFor;

    public bool sprintCompleted;


    private System.Action<SprintResults> sprintResultsCallBack;

    public Sprinting(LayerMask sprintLayer, GameObject ownerGameObject, ThirdPersonPlayerCharacter thirdPersonPlayerCharacter, float dashRadius, string tagToLookFor, System.Action<SprintResults> sprintResultsCallBack)
    {
        this.sprintLayer = sprintLayer;
        this.ownerGameObject = ownerGameObject;
        this.thirdPersonPlayerCharacter = thirdPersonPlayerCharacter;
        this.dashRadius = dashRadius;
        this.tagToLookFor = tagToLookFor;
        this.sprintResultsCallBack = sprintResultsCallBack;
    }
    public void Enter()
    {
        sprintCompleted = false;
    }

    public void Execute()
    {
        if (!sprintCompleted)
        {
            var hitObjects = Physics.OverlapSphere(this.ownerGameObject.transform.position, this.dashRadius);

            var allObjectsWithTheRequiredTag = new List<Collider>();


            //Any wall layer mask within range will allow player to move along this wall.  
            for (int i = 0; i < hitObjects.Length; i++)
            {
                if (hitObjects[i].CompareTag(this.tagToLookFor))
                {
                    // this.navMeshAgent.SetDestination(hitObjects[i].transform.position);

                    allObjectsWithTheRequiredTag.Add(hitObjects[i]);
                }
                break;
            }

            var sprintResults = new SprintResults(hitObjects, allObjectsWithTheRequiredTag);
            //This is where we should send the information back.
            this.sprintResultsCallBack(sprintResults);

            this.sprintCompleted = true;
        }
    }

    public void Exit()
    {
      
    }

}

public class SprintResults
{
    public Collider[] allHitObjectsInSearchRadius;
    public List<Collider> AllHitObjectsWithRequiredTag;
    //Closest object

    //Farthest object

    public SprintResults(Collider[] allHitObjectsInSearchRadius, List<Collider> AllHitObjectsWithRequiredTag)
    {
        this.allHitObjectsInSearchRadius = allHitObjectsInSearchRadius;
        this.AllHitObjectsWithRequiredTag = AllHitObjectsWithRequiredTag;
    }

}
