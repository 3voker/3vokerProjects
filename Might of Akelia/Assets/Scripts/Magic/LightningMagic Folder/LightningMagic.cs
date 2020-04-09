using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningMagic : BaseMagic {


     LightningMagic()
    {
        this.CurrentElement = Magic.lightning;
        this.StrongAgainstElement = Magic.water;
        this.WeakAgainstElement = Magic.earth;
    }
}
