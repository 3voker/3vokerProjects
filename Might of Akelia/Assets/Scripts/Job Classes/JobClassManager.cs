using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class JobClassManager : JobClassFactory//, IJobChanging
{

    /// <summary>
    /// Purpose of this script is to organize JobClasses into set lists
    /// Player toggles to add and remove jobs from active use list
    /// Player will only be able to add a one job to each category
    /// Only if Player meets to requirements to use that job 
    /// </summary>
    [SerializeField]
    GameObject player;

    [SerializeField]
    


    Rook rook = new Rook();

    string mainjobNames;
    string subjobNames;
    string basejobNames;
    string jobName;

    //List of all jobs available to choose from
    public List<Rook> allJobs;

    //Array list of the 4 job categories the player can place a different job into
    public JobMeterTier[] JobClasses = new JobMeterTier[]{JobMeterTier.Primary, JobMeterTier.Secondary, JobMeterTier.Tertiary, JobMeterTier.Quaternary };

    #region IjobChanging Interface Implementation
    public string jobClassName
    {
        get
        {
            return jobName;
        }
    }

    public string BaseJobName
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    //public JobMeterTier jobMeterTier { get { return JobClasses[rook.job]; } }

    //public JobTypeCategory jobTypeCategory => throw new NotImplementedException();

    //public int JobSPCost { get { return rook.spjobCost; } }

    //public int RequiredLevel { get { return RequiredLevel; } }


    //public bool CanChange => throw new NotImplementedException();

    //public string Description => throw new NotImplementedException();

    //public int JobClassHP => throw new NotImplementedException();

    //public int JobClassMP => throw new NotImplementedException();

    //public int MinDamage => throw new NotImplementedException();

    //public int MaxDamage => throw new NotImplementedException();

    //public int Restore => throw new NotImplementedException();

    //public int MinRestore => throw new NotImplementedException();

    //public int MaxRestore => throw new NotImplementedException();

    //public float JobChangeTime => throw new NotImplementedException();

    //public float JobChangeCoolDown => throw new NotImplementedException();

    //public float JobChangeRecoveryTime => throw new NotImplementedException();

    #endregion

    float spCost;

    bool isEqual;

    enum BaseJobClass { rook, Melee, Rogue, Support, Mage };
    
    public JobClassManager(string jobClassName, string subJobClassName, string baseJobClassName)
    {
      
    }
    //Sort through job list and change to the job selected from the list
    public override Rook ChangeJob(Rook chosenJob)
    {
        foreach (Rook job in allJobs)
        {
            
        }
        
        return base.ChangeJob(chosenJob);
    }

    //Sort through job list and add the job selected from the list
    public void AddJobToList()
    {
        String jobToRemove;

        foreach (Rook job in allJobs)
        {
           
            //if(job.name == jobToRemove)
            //{
            //    allJobs.Remove(job);
            //}
        }
    }
    //Sort through job list and remove the job selected from the list
    public void RemoveJobFromList()
    {
        
    }
}

