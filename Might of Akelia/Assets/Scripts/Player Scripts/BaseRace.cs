using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRace  {

    // Use this for initialization
    enum Size { SmallSize, MediumSize, LargeSize };
    enum Race {Humanoid, Beast, FlyingBeast, Plant, Machine, Undead, Aquatic, Insectoid};
    enum ElementalResistance {water, fire, ice, wind, earth, lightning, shadow, light};
    enum ElementalWeakness {water, fire, ice, wind, earth, lightning, shadow, light };

    enum StatusEffectResistance {ooze, burn, froze, whirl, buried, shocked, darkened, brighted, stagger, slow, stop, charm, silence, blinded, KO};
    enum StatusEffectWeakness {ooze, burn, froze, whirl, buried, shocked, darkened, brighted, stagger, slow, stop, charm, silence, blinded, KO };

    public string EnemySurname { get; set; }

    public virtual void death()
    {
        DropItem();
        GiveExperiencePoints();
    }

    private void GiveExperiencePoints()
    {
        throw new NotImplementedException();
    }

    private void DropItem()
    {
       
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
