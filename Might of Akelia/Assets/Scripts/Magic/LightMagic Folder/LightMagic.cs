using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMagic : BaseMagic {

    LightMagic()
    {
        this.CurrentElement = Magic.light;
        this.WeakAgainstElement = Magic.shadow;
        this.StrongAgainstElement = Magic.shadow; //I think? Shadow and Light are inversely effected by one another.
    }
}
