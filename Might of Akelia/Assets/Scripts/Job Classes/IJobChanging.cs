using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJobChanging
{

    // Use this for initialization 
    string BaseJobName { get; }
    JobMeterTier jobMeterTier {get; }
    JobTypeCategory jobTypeCategory { get; }

    int JobSPCost { get; }
    int RequiredLevel { get; }
    bool CanChange { get; }
    string Description { get; }
    int JobClassHP { get; }
    int JobClassMP { get; }

    int MinDamage { get; }
    int MaxDamage { get; }
    int Restore { get; }

    int MinRestore { get; }
    int MaxRestore { get; }
    //Cast times
    float JobChangeTime { get; }
    float JobChangeCoolDown { get; }
    float JobChangeRecoveryTime { get; }
}
