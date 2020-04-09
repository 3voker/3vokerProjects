using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFollowable
{

    void Follow(GameObject agent, FollowPath followPath);

}
