using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMagic : BaseMagic {

     public WaterMagic()
    {
        this.CurrentElement = Magic.water;
        this.StrongAgainstElement = Magic.fire;
        this.WeakAgainstElement = Magic.lightning;
    }
}
