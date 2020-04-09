using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Zergmeister is a melee archetype job. It inherits from BaseWarriorClass Stats
/// It utilizes IJobChanging Class to change and alter stats
/// </summary>

public class Zergmeister : BaseWarriorClass, IJobChanging
{
    Rook rook;

    public string BaseJobName
    {
        get
        {
            return "Zergmeister";
        }
    }

    public JobMeterTier jobMeterTier { get { return JobMeterTier.Secondary; } }

    public JobTypeCategory jobTypeCategory { get { return JobTypeCategory.Melee; } }

    public int JobSPCost { get { return 10; } }

    public int RequiredLevel { get { return 10; } }

    //Check Rook to see if can change. 
    public bool CanChange { get { return rook.CanChange; } }

    public string Description { get { return "Warrior that can do the most!"; } }

    public int JobClassHP { get { return 10; } }

    public int JobClassMP { get { return 2; } }

    public int MinDamage { get { return 5; } }

    public int MaxDamage { get { return 5; } }

    public int Restore { get { return 2; } }

    public int MinRestore { get { return 1; } }

    public int MaxRestore { get { return 3; } }

    public float JobChangeTime { get { return rook.JobChangeTime; } }

    public float JobChangeCoolDown { get { return rook.JobChangeCoolDown; } }

    public float JobChangeRecoveryTime { get { return rook.JobChangeRecoveryTime; } }

}
