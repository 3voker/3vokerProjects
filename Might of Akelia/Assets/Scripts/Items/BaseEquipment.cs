using UnityEngine;
using System.Collections;


namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class BaseEquipment : BaseStatItem
    {
        public enum EquipmentTypes
        {
            HEAD, 
            CHEST,
            HANDS,
            LEG,
            FEET,
            NECKLACE,
            RING1,
            RING2,
        }

        private EquipmentTypes equipmentType;
        private int spellEffectID;

        public EquipmentTypes EquipmentType
        {
            get { return equipmentType; }
            set { equipmentType = value; }
        }
        public int SpellEffectID
        {
            get { return spellEffectID; }
            set { spellEffectID = value; }
        }

    }
}
