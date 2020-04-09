using UnityEngine;
using System.Collections;

public class BaseRogueClass : BaseCharacterClass
{

    // Use this for initialization
    int initialLevel = 5;

    JobTypeCategory jobType;

    public BaseRogueClass()
    {
        CharacterClassName = "Rogue";
        jobType = JobTypeCategory.Rogue;
        CharacterClassDescription = "A fast and nimble rogue class!";

        Strength = 5;
        Agility = 9;
        Vitality = 4;
        Focus = 4;
        Luck = 8;
        Dexterity = 9;
        Speed = 8;
        Stamina = 9;
        Wisdom = 4;
        Spirit = 4;

    }
}
