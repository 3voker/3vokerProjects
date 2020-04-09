using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellRestore  {

    int Restore { get; }
    int MinRestore { get; }
    int MaxRestore { get; }
}
