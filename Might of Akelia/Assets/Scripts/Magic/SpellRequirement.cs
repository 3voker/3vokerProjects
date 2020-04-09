using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SpellRequirement {
    //Require specific level of job or player to cast
    //Require a specified job to cast
    //Require specific amount of MP
    //Bool to determine if can Cast
    //This should be placed as an interface requirement of every single spell before casting
	int LevelNeededToCast { get; }
    int MPNeededToCast { get; }
    bool CanCast { get; }
    
}
