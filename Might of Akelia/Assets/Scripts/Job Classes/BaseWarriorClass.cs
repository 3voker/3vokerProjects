using UnityEngine;
using System.Collections;

public class BaseWarriorClass : BaseCharacterClass
{

    // Use this for initialization

    int initialLevel = 5;
	public BaseWarriorClass()
    {
        CharacterClassName = "Warrior";
        CharacterClassDescription = "A strong and powerful weapon master!";

        Strength = 10;
        Agility = 6;
        Vitality = 8;
        Focus = 5;
        Luck = 5;
        Dexterity = 8;
        Speed = 6;
        Stamina = 9;
        Wisdom = 4;
        Spirit = 4;
       
}
}
