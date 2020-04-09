using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellDamage  {

    int Damage { get; }
    int MinDamage { get; }
    int MaxDamage { get; }
}
