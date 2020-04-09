using UnityEngine;
using System.Collections;
using System;
//Decides SP Cost and Placement of Job Classes for players
public enum JobMeterTier { Primary, Secondary, Tertiary, Quaternary };
//Category of JobType
public enum JobTypeCategory { Rook, Melee, Rogue, Support, Mage };

public enum Melee { Zergmeister, Lancer, Guardian, Pugilist, Vanguard, Grenadier, WarDrummer, Inquisitor }

public enum Rogue { Shinobi, Raider, Drifter, Slinger, Ronin, Ranger, Jester, Bomber }

public enum Support { Specialist, Monk, Barkeeper, Spectre, Jammer, Tailor, SpellBringer, Medicus }

public enum Mage { Warlock, BattleMage, Magician, Shaman, Chanter, Engineer, Arbiter, Apostate }

public abstract class JobClassFactory : MonoBehaviour
{
    //Primary Job Class is type BaseCharacterClass
    public BaseCharacterClass PrimaryJobClass { get; set; }
    public BaseCharacterClass SecondaryJobClass { get; set; }
    public BaseCharacterClass TertiaryJobClass { get; set; }
    public BaseCharacterClass QuaternaryJobClass { get; set; }

    public JobClassFactory()
    {

    }
    public JobClassFactory(BaseCharacterClass v1, BaseCharacterClass v2, BaseCharacterClass v3, BaseCharacterClass v4)
    {
        PrimaryJobClass = v1;
        SecondaryJobClass = v2;
        TertiaryJobClass = v3;
        QuaternaryJobClass = v4;
    }
    public virtual Rook ChangeJob(Rook chosenJob) { return chosenJob; }

    public virtual Rook LostJob(Rook unchosenJOb) { return unchosenJOb; }   
}

