using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellCasting  {
    
    //Cast times
    float SpellCastTime { get; }
    float SpellRecastTime { get; }
    float CasterRecoveryTime { get; }
}
