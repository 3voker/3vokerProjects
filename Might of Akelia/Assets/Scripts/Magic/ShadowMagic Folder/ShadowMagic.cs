using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMagic : BaseMagic {

     public ShadowMagic()
    {
        this.CurrentElement = Magic.shadow;
        this.StrongAgainstElement = Magic.light;
        this.WeakAgainstElement = Magic.light;
    }
}
