using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthMagic : BaseMagic {

    // Use this for initialization
   
    public EarthMagic()
    {
        this.CurrentElement = Magic.earth;
        this.StrongAgainstElement = Magic.lightning;
        this.WeakAgainstElement = Magic.wind;
    }
}
