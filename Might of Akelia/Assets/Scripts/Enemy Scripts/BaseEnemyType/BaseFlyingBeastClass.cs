using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlyingBeastClass : BaseRace {

    // Use this for initialization

    //Weakness to Piercing, Wind, Ice

    BaseRace flyingEnemyRace;

    public BaseRace FlyingEnemyRace
    {
        get { return flyingEnemyRace; }
        set { flyingEnemyRace = value; }
    }
    void Start () {
		
	}

    public virtual void WingSpin()
    {

    }

    public override void death()
    {
        base.death();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
