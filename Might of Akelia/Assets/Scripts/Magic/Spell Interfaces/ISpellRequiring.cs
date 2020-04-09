using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpellRequiring
{
    int RequiredLevel { get; }
    bool CanCast { get; }
}
