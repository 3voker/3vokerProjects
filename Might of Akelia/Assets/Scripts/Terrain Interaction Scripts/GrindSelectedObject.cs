using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindSelectedObject : MonoBehaviour
{

    /// <summary>
    /// Script needed to detect and interact with the object selected by raycast.
    /// Attached to the MainCamera, child of the RigidbodyFPS.
    /// </summary>
    private IGrindable objectToGrindOn; // reference pointing to the object best suitable for interaction

    private IFollowable objectPathToFollow;

    private void OnEnable()
    {
        DetectGrindableObject.ObjectToGrindOnChanged += OnObjectToGrindOnChanged;
    }

    private void OnDisable()
    {
        DetectGrindableObject.ObjectToGrindOnChanged -= OnObjectToGrindOnChanged;
    }

    private void Update()
    {
        CheckForGrindInput();
    }

    /// <summary>
    /// Called when Object To Interact With changes in the DetectInteractableObject
    /// script through an event.
    /// </summary>  
    public void OnObjectToGrindOnChanged(IGrindable returnedGrindable)
    {
        objectToGrindOn = returnedGrindable;
    }

    public void OnObjectToFollowChanged(IFollowable returnFollowableblePath)
    {
        objectPathToFollow = returnFollowableblePath;
    }

    /// <summary>
    /// checks if objectToInteractWith is not null then if interact input is pressed.
    /// activate interact of objectToInteractWith if both return true.
    /// </summary>
    private void CheckForGrindInput()
    {
        if (objectToGrindOn != null)
        {
            // Passes root gameObject (Player) as interacting agent.
            objectToGrindOn.Grind(transform.root.gameObject, objectToGrindOn as MovementPath);
        }
        if(objectPathToFollow != null)
        {
            objectPathToFollow.Follow(transform.root.gameObject, objectPathToFollow as FollowPath);
        }
    }
}
