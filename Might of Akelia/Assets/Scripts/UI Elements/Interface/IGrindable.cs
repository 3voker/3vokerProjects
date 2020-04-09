using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Any object that an agent can grind on should implement this interface.
// The Player should Ray/Cone/Spherecast in order to find Grindable objects.
// Use "grind" button to grind on rail.

//More than likely this will work by simple proximity

public interface IGrindable
{
    /// <summary>
    /// This method defines the behavior of the object when an agent grinds on it.
    /// </summary>
    void Grind(GameObject agent, MovementPath movePath);
}
