using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMagic : BaseMagic {

     public WindMagic()
    {
        this.CurrentElement = Magic.wind;
        this.StrongAgainstElement = Magic.earth;
        this.WeakAgainstElement = Magic.ice;
    }
}
