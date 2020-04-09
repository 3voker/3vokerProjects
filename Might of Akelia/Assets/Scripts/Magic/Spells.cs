using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public delegate void Cast(SpellCaster caster, Rook spellTarget);

public class Spells
{
    Dictionary<string, Spell> spell = new Dictionary<string, Spell>();
    SpellManager spellManager;

    SpellCategories supportCategory;
    SpellRanges spellRange;
    SupportSpellTypes supportSpellType;
    OffensiveSpellTypes offensiveSpellType;
   
    //public Spells(int spellRequiredLevel, bool canCastSpell) : base(spellRequiredLevel, canCastSpell)
    //{
        //this.RequiredLevel = spellRequiredLevel;
        //this.CanCast = canCastSpell;
    //}

     
    void Start() //Lot of ghost code here...
    {
        //Support Magic List
        //Spell ElementalStrike = new Spells("ElementalStrike", 1);
        //Offensive Magic List
       // Spells Cure1 = new Spells("Cure 1", 2); //It knows about the class
        //Cure1.supportSpellType = SupportSpellTypes.Curative;
        //Cure2.supportMagicType = SupportspellType.Curative; //Method
        //Cure3.supportMagicType = SupportspellType.Curative; //Method
    }
    public void Cast(SpellCaster spellCaster, ITargetable spellTarget)
    {

    }
    #region Spells
    public static void ElementalStrike(SpellCaster caster)
    {     
        //if (CurrentElement == currentMagicElement)
        //    this.minDamage = 1;
        //this.maxDamage = RequiredLevel;
    }
    //Restores Health points to target
    //[Spells("Cure1", 1)]
    //public static void Cure1(SpellCaster caster, Rook spellTarget)
    //{
    //    description = "Restores target's health points.";
    //    spellTarget.RecoverHealth(4);
    //    //this.minRestore += 30;
    //    //this.maxRestore += 30;//
    //}
    public static void ElementalResistanceUP(SpellCaster caster, Rook spellTarget)
    {
        //if (previousMagicElement == this.currentMagicElement)
        //{
        //    this.minDamage -= minDamage;
        //}
    }
    public static void Tighten(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Jewel(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ManaParry(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ElementalWall(SpellCaster caster, Rook spellTarget)
    {

    }
    //Restores Health points to target
    public static void Cure2(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void StatusResistanceUP(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Fluent(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Sacrifice(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ManaJump(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ElementalShift(SpellCaster caster, Rook spellTarget)
    {

    }
    //Restores Health points to target
    public static void Cure3(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void KnockBackResistanceUP(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Stride(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Omni(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ManaBreak(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ForbiddenArtWard(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void StatusEffect(SpellCaster caster, Rook spellTarget)
    {

    }
    // [Spell("ElementalMagic1", 5)]
    public static void ElementalMagic1(SpellCaster caster, Rook spellTarget)
    {
        //this.Description = "Deals " + SpellManager.spellElement + " damage to target.";
        spellTarget.TakeDamage(7);
    }
    public static void Confuse(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Loosen(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Fracture(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ManaLock(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ResistanceDown(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ElementalMagic2(SpellCaster caster, Rook spellTarget)
    {
        spellTarget.TakeDamage(7);
    }
    public static void Half(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Stutter(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Vortex(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ManaDash(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void SkillDown(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ElementalMagic3(SpellCaster caster, Rook spellTarget)
    {
        spellTarget.TakeDamage(7);
    }
    public static void Trance(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Finish(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void Curse(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ManaStomp(SpellCaster caster, Rook spellTarget)
    {

    }
    public static void ForbiddenArtBlitz(SpellCaster caster, Rook spellTarget)
    {

    }
#endregion
    //protected override void ApplySpell(SpellCaster caster, ITargetable spellTarget)
    //{
    //    throw new NotImplementedException();
    //}
}
public class Cure1 : Spell, ISpellInfo, ISpellRequiring, ISpellCasting, ISpellDamage
{
    public Cure1()
    {

    }

    public Cure1(Spell sp)
    {
        #region MagicTypeEnum
        this.SpellCategory = sp.SpellCategory;
        this.SpellRange = SpellRanges.SingleTarget;
        this.SupportSpellType = SupportSpellTypes.Curative;
        this.OffensiveSpellType = OffensiveSpellTypes.NonOffensive;
        #endregion

        #region SpellInfo
        this.SpellID = 0;
        this.SpellName = "Cure 1";
        this.ManaCost = 5;
        this.Element = (string)(Magic.water).ToString(); //GetElement()
        #endregion

        #region  SpellRequirements
        this.RequiredLevel = 5;
        this.CanCast = true; //Bool CheckRequirement(int Required Level, int MPCost, string StatusEffect)   
        #endregion

        #region SpellCasting
        this.SpellCastTime = 3f;
        this.SpellRecastTime = 3f;
        this.CasterRecoveryTime = 3f;
        #endregion

        #region SpellRestoring
        this.Restore = 5;
        this.MinRestore = 5;
        this.MaxRestore = 5;
        #endregion

        #region SpellDamage
        this.Damage = 5; //Int MagicDamage(int Damage, int playerStat, int enemyStat, element, resistance)
        this.MinDamage = 5; //Calculate Minimum(Base Damage)
        this.MaxDamage = 5; //Calculate Maximum(Critical Hit, Weakness)
        #endregion
    }

    public override void Cast(SpellCaster caster, ITargetable spellTarget)
    {
        this.SpellCastTime -= Time.deltaTime;
        while (SpellCastTime >= 0)
        {
            caster.SubtractMana(ManaCost);
        }

        base.Cast(caster, spellTarget);
    }
    //protected override void ApplySpell(SpellCaster caster, ITargetable spellTarget)
    //{
    //    throw new NotImplementedException();
    //}
}

