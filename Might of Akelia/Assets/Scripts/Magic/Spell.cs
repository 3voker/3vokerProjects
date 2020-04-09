using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellCategories { NoCategory, Support, Offensive }
public enum SpellRanges { NoSpellRange, SingleTarget, ConeRange, LinearRange, AreaOfEffect }
public enum SupportSpellTypes { NonSupport, Barrier, Curative, Regenerative, Temporal, Boost, Raise, Ward}
public enum OffensiveSpellTypes { NonOffensive, Trap, Elemental, Degenerative, Temporal, Diminish, Ruin, Blitz }

[System.Serializable]
public class Spell : ISpellInfo, ISpellRequiring, ISpellCasting, ISpellRestore, ISpellDamage
{
    public SpellCaster spellCaster { get; set; }

    public bool IsCasting { get; protected set; }



    public SpellCategories spellCategory;
    public SpellCategories SpellCategory { get { return spellCategory; } set { spellCategory = value; } }

    [SerializeField]
    public SpellRanges spellRange;
    public SpellRanges SpellRange { get { return spellRange; } set { spellRange = value; } }

    [SerializeField]
    public SupportSpellTypes supportSpellType;
    public SupportSpellTypes SupportSpellType { get { return supportSpellType; } set { supportSpellType = value; } }

    [SerializeField]
    public OffensiveSpellTypes offensiveSpellType;
    public OffensiveSpellTypes OffensiveSpellType { get { return offensiveSpellType; } set { offensiveSpellType = value; } }



    #region Interface ISpellInfo returning Variables
    [SerializeField]
    public int spellID;
    public int SpellID { get { return spellID; } set { spellID = value; } }

    [SerializeField]
    public Texture icon;
    public Texture Icon { get { return icon; } set { icon = value; } }

    [SerializeField]
    public string spellName;
    public string SpellName { get { return spellName; } set { spellName = value; } }

    [SerializeField]
    public int manaCost;
    public int ManaCost { get { return manaCost; } set { manaCost = value; } }

    [SerializeField]
    public string element;
    public string Element { get { return element; } set { element = value; } }

    [SerializeField]
    public string description;
    public string Description { get { return description; } set { description = value; } }


    #endregion

    #region Interface ISpellRequiring returning Variables
    [SerializeField]
    public int requiredLevel;
    public int RequiredLevel { get { return requiredLevel; } set { requiredLevel = value; } }
    [SerializeField]
    public bool canCast;
    public bool CanCast { get { return canCast; } set { canCast = value; } }//Go check caster's mp in the spell/spell caster script
    #endregion

    #region Interface ISpellDamage returning Variables
    [SerializeField]
    public int damage;
    public int Damage { get { return damage; } set { damage = value; } }
    [SerializeField]
    public int minDamage;//Add resistance, weakness, player stats, and player charge for variable
    public int MinDamage { get { return minDamage; } set { minDamage = value; } }
    [SerializeField]
    public int maxDamage;//check player stats and player charge
    public int MaxDamage { get { return maxDamage; } set { maxDamage = value; } }
    #endregion

    #region Interface ISpellRestore returning Variables
    [SerializeField]
    public int restore;
    public int Restore { get { return restore; } set { restore = value; } }
    [SerializeField]
    public int minRestore;//Check spellcaster charge
    public int MinRestore { get { return minRestore; } set { minRestore = value; } }
    [SerializeField]
    public int maxRestore;
    public int MaxRestore { get { return maxRestore; } set { maxRestore = value; } }
    #endregion

    #region ISpellCasting Variables
    [SerializeField]
    public float spellCastTime;
    public float SpellCastTime { get { return spellCastTime; } set { spellCastTime = value; } }
    [SerializeField]
    public float spellRecastTime;
    public float SpellRecastTime { get { return spellRecastTime; } set { spellRecastTime = value; } }
    [SerializeField]
    public float casterRecoveryTime;
    public float CasterRecoveryTime { get { return casterRecoveryTime; } set { casterRecoveryTime = value; } }
    #endregion
    public Spell()
    {

    }
    public Spell(Spell sInfo)
    {
        //Pass SpellInfo
        SpellID = sInfo.SpellID;
        Icon = sInfo.Icon;
        SpellName = sInfo.SpellName;
        ManaCost = sInfo.ManaCost;
        Element = sInfo.Element;
        Description = sInfo.Description;

        //Pass parameters
        SpellCategory = sInfo.SpellCategory;
        SpellRange = sInfo.SpellRange;
        SupportSpellType = sInfo.SupportSpellType;
        OffensiveSpellType = sInfo.OffensiveSpellType;
      
        //Pass Spell Requirements
        RequiredLevel = sInfo.RequiredLevel;
        CanCast = sInfo.CanCast;

        //Pass Spell Damage
        Damage = sInfo.Damage;
        MinDamage = sInfo.MinDamage;
        MaxDamage = sInfo.MaxDamage;

        //Pass Spell Restore
        Restore = sInfo.Restore;
        MinRestore = sInfo.MinRestore;
        MaxRestore = sInfo.MaxRestore;

        //Pass Spell Casting
        SpellCastTime = sInfo.SpellCastTime;
        SpellRecastTime = sInfo.SpellRecastTime;
        CasterRecoveryTime = sInfo.CasterRecoveryTime;
    }

    //public Spell(SpellCategories spellCategory, SpellRanges spellRange, SupportSpellTypes supportSpellType, OffensiveSpellTypes offensiveSpellType)
    //{
    //    spellCategory = SpellCategory;
    //    spellRange = SpellRange;
    //    supportSpellType = SupportSpellType;
    //    offensiveSpellType = OffensiveSpellType;
    //}
    //public virtual string About()
    //{
    //    return Description;
    //}
    //This constructor passes (int)spell level requirement and (bool)if caster can cast.
    //public Spell(int baseSpellLevelRequirement, bool canCast)
    //{
    //    //RequiredLevel = baseSpellLevelRequirement;
    //    //CanCast = canCast;
    //}
    public virtual void Cast(SpellCaster caster, ITargetable spellTarget)
    {
        //caster.SubtractMana(ManaCost);
        //ApplySpell(caster, spellTarget);
    }
    public virtual void StopCasting()
    {
        spellCaster.StopCastingSpell();
    }
    //protected abstract void ApplySpell(SpellCaster caster, ITargetable spellTarget);//abstract
}
// BaseMagic baseMagic = new BaseMagic();
