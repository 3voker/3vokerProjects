using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Magic { water, fire, ice, wind, earth, lightning, shadow, light };
public abstract class BaseMagic : MonoBehaviour
{

    // Use this for initialization
    public Magic CurrentElement { get; protected set; }
    public Magic WeakAgainstElement { get; protected set; }
    public Magic StrongAgainstElement { get; protected set; }

    public BaseMagic()
    {
        
    }
    //Reveals what elemental weakness of elemental spell cast by user is.
    Magic ElementalWeakness(Magic currentElement, Magic weakAgainstElement)
    {
        switch (currentElement)
        {
            case 0: // if (currentElement == Magic.water)
                weakAgainstElement = Magic.lightning;
                return weakAgainstElement;
            case (Magic)1:
                //if (currentElement == Magic.fire)
                weakAgainstElement = Magic.water;
                return weakAgainstElement;
            case (Magic)2: // if (currentElement == Magic.ice)
                weakAgainstElement = Magic.fire;
                return weakAgainstElement;
            case (Magic)3: //if (currentElement == Magic.wind)
                weakAgainstElement = Magic.ice;
                return weakAgainstElement;
            case (Magic)4: //if (currentElement == Magic.earth)
                weakAgainstElement = Magic.wind;
                return weakAgainstElement;
            case (Magic)5: //if (currentElement == Magic.lightning)
                weakAgainstElement = Magic.earth;
                return weakAgainstElement;
            case (Magic)6: //if (currentElement == Magic.shadow)
                weakAgainstElement = Magic.light;
                return weakAgainstElement;
            case (Magic)7: //if (currentElement == Magic.light)
                weakAgainstElement = Magic.shadow;
                return weakAgainstElement;
        }
        return weakAgainstElement;
    }
    //Declares what Elemental Resistance spell or user has. 
    Magic ElementalResistance(Magic currentElement, Magic strongAgainstElement)
    {
        switch (currentElement)
        {
            case 0: // if (currentElement == Magic.water)
                strongAgainstElement = Magic.fire;
                return strongAgainstElement;
            case (Magic)1:
                //if (currentElement == Magic.fire)
                strongAgainstElement = Magic.water;
                return strongAgainstElement;
            case (Magic)2: // if (currentElement == Magic.ice)
                strongAgainstElement = Magic.fire;
                return strongAgainstElement;
            case (Magic)3: //if (currentElement == Magic.wind)
                strongAgainstElement = Magic.ice;
                return strongAgainstElement;
            case (Magic)4: //if (currentElement == Magic.earth)
                strongAgainstElement = Magic.wind;
                return strongAgainstElement;
            case (Magic)5: //if (currentElement == Magic.lightning)
                strongAgainstElement = Magic.earth;
                return strongAgainstElement;
            case (Magic)6: //if (currentElement == Magic.shadow)
                strongAgainstElement = Magic.light;
                return strongAgainstElement;
            case (Magic)7: //if (currentElement == Magic.light)
                strongAgainstElement = Magic.shadow;
                return strongAgainstElement;
        }
        return strongAgainstElement;
    }
}
