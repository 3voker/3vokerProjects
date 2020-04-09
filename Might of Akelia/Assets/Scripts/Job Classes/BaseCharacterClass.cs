using UnityEngine;
using System.Collections;



public class BaseCharacterClass : MonoBehaviour
{
    //FrameWork Notes: Main job, Subjob, Race, and Level determine players overall stats. 
    //Each variation of the 4 will add or subtract the stats for a specific value. 
    //Example: Minotaur, Zergmeister, Guardian, Level 20. HP: 680, STR: 33, VIT: 35, AGI: 29, INT: 18. etc.
    // Use this for initialization
    private string characterClassName;
    private string characterClassDescription;

    //8 stats for 8 elements // can start with 5 basic jobs.
    // Strength - Determines physical / magical attack power vs opponents physical / magical defense
    //Physical Attack Power - physical attacks, technique potency, physical knockback potency.
    //Magical Attack Power - magical attacks, mana technique potency, magical knockback potency. 

    //Agility - Determines physical / Magical Evasion vs opponents physical / Magical Accuracy
    //Physical Evasion - increased chance of naturally evading physical attacks, increased aerial speed, and projectile deflection. 
    //Magical Evasion - increased chance of naturally evading magical attacks, increased wall attack and wall running duration. 

    //Vitality - Determines physical / magical defense vs opponents physical / magical attack power
    //Physical Defense - Physical Defense(Block potency), Grapple Potency, Physical knockback resistance
    //Magical Defense - Magical Defense(Mana Parry potency), Mana Lock Potency, magical knockback resistance

    //Speed - Determines physical / magical speed vs opponents physical / magical evasion
    //*Physical Speed - Combo Speed, Movement Speed, Counter Attack Speed
    //*Magical Speed - Magic Cast Speed, Recast Time, Mana Technique Speed

    //Focus- Determines physical / magical resistance vs opponents physical / magical potency 
    //*Physical Resistance - Physical KnockBack resistance, Ability to break grapples, critical hit resistance.
    //*Magical Resistance - Magical KnockBack resistance, Ability to resist status effects, magical critical hit resistance.

    //Luck - Determines physical / Magical Luck vs opponents physical / magical Accuracy & Speed.
    //Physical Luck - Lower item/weapon/armor fail rate, increased disarming and counter attack potency. 
    //Magical Luck - Lower spell interruption rate, lower magic scroll fail rate, increased spell reflection potency.

    //Dexterity - Determines physical / magical acccuracy vs opponents physical / magical evasion 
    //Physical Accuracy - physical critical Hit, Accuracy, Parrying strength
    //Magical Accuracy - Magic Spell Movement Speed, Magic Spell Homing Potency, Status Effect Potency.

    //Wisdom - Determines physical / magical technique resistance, range, and duration vs opponents Strength
    //Physical Wisdom - increase physical technique resistance, range, and duration. 
    //Magical Wisdom - increase magical technique resistance, range, and duration.

    //Spirit - Determines physical / magical health and recovery
    //Physical Spirit - HitStun resistance, HP recover speed, Increased HP 
    //Magical Spirit - Status Recover speed, MP recover Speed, Increased MP 

    //Stamina - Determines SP gain/loss, as well as overall stats based on party SP % vs opponent SP %.
    //Armor Breach - SP % determines party attack, defense, potency, and resistance. 
    //Lower the % the lower party overall stats become. (Can drop to 1/3rd of original parameters)
    //higher the % the higher party overall stats become. (Can rise to 2x of original parameters)

    private int strength;
    private int agility;
    private int vitality;
    private int speed;
    private int focus;
    private int luck;
    private int dexterity;
    private int wisdom;
    private int spirit;
    private int stamina;

    public string CharacterClassName
    {
        get { return characterClassName; }
        set { characterClassName = value; }
    }
    public string CharacterClassDescription
    {
        get { return characterClassDescription; }
        set { characterClassDescription = value; }
    }
    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }
    public int Agility
    {
        get { return strength; }
        set { strength = value; }
    }
    public int Vitality
    {
        get { return vitality; }
        set { vitality = value; }
    }
    public int Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public int Focus
    {
        get { return focus; }
        set { focus = value; }
    }
    public int Luck
    {
        get { return luck; }
        set { luck = value; }
    }
    public int Dexterity
    {
        get { return dexterity; }
        set { dexterity = value; }
    }
    public int Wisdom
    {
        get { return wisdom; }
        set { wisdom = value; }
    }
    public int Spirit
    {
        get { return spirit; }
        set { spirit = value; }
    }
    public int Stamina
    {
        get { return stamina; }
        set { stamina = value; }
    }
}



