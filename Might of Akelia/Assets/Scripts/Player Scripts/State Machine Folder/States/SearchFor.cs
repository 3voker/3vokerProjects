using UnityEngine;
using UnityEngine.AI;

public class SearchFor : IState
{
    private LayerMask searchLayer;

    private GameObject ownerGameObject;

    //Radius in which class searches for
    private float searchRadius;

    //Tag on game object
    private string tagToLookFor;


    //Searches layer, ownerGameObject, radius of search, tag to look for, and activates NavMeshAgent to interact(Move, attack, etc..)
    public SearchFor(LayerMask searchLayer, GameObject ownerGameObject, float searchRadius, string tagToLookFor)
    {
        this.searchLayer = searchLayer;
        this.ownerGameObject = ownerGameObject;
        this.searchRadius = searchRadius;
        this.tagToLookFor = tagToLookFor;    
    }

    public void Enter()
    {

    }

    public void Execute()
    {
        var hitObjects = Physics.OverlapSphere(this.ownerGameObject.transform.position, this.searchRadius);

        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (hitObjects[i].CompareTag(this.tagToLookFor))
            {
               // this.navMeshAgent.SetDestination(hitObjects[i].transform.position);
            }
            break;
        }
    }

    public void Exit()
    {

    }
}
