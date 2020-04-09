using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMagic : BaseMagic {

     public IceMagic()
    {
        this.CurrentElement = Magic.ice;
        this.StrongAgainstElement = Magic.wind;
        this.WeakAgainstElement = Magic.fire;
    }
}
