using UnityEngine;
using System.Collections;

public class BaseSupportClass : BaseCharacterClass
{

    // Use this for initialization
    int initialLevel = 5;
    public BaseSupportClass()
    {
        CharacterClassName = "Support";
        CharacterClassDescription = "A wise and spiritual support class!";


        Strength = 3;
        Agility = 7;
        Vitality = 3;
        Focus = 9;
        Luck = 6;
        Dexterity = 7;
        Speed = 7;
        Stamina = 5;
        Wisdom = 9;
        Spirit = 9;

    }
}

