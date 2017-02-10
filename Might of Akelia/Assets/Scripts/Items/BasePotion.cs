using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class BasePotion : BaseStatItem
    {
        public enum PotionTypes
        {
            HEALTH,
            MAGIC,
            ENERGY,
            STRENGTH,
            AGILITY,
            VITALITY,
            SPEED,
            FOCUS,
            LUCK,
            DEXTERITY,
            WISDOM,
            SPIRIT,
            STAMINA
         }
        // Use this for initialization
        private PotionTypes potionType;
        private int spellEffectID;

        public PotionTypes PotionType
        {
            get { return potionType; }
            set { potionType = value; }
        }
        public int SpellEffectID
        {
            get { return spellEffectID; }
            set { spellEffectID = value; }
        }
    }
}
