using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMagic : BaseMagic {


    public FireMagic()
    {
        this.CurrentElement = Magic.fire;
        this.StrongAgainstElement = Magic.ice;
        this.WeakAgainstElement = Magic.water;
    }
}
