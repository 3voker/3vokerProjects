using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellInfo {

    int SpellID { get; }
    string SpellName { get; }
    int ManaCost { get; }
    string Element { get; }
    string Description { get; }
}
