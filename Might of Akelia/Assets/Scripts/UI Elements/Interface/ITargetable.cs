using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable
{
    GameObject MainTarget { get; }
    GameObject SubTarget { get; }
}
