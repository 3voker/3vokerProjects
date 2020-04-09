using UnityEngine;
using System.Collections;
public class BaseMageClass : BaseCharacterClass
{

    // Use this for initialization

    int initialLevel = 5;


    public BaseMageClass()
    {
        CharacterClassName = "Mage";
        CharacterClassDescription = "A wise and spiritual magic user!";
        Strength = 5; 
        Agility = 6; 
        Vitality = 4; 
        Focus = 10; 
        Luck = 7; 
        Dexterity = 8; 
        Speed = 7; 
        Stamina = 4; 
        Wisdom = 10;  
        Spirit = 9; 

    }
}