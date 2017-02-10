using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class BaseScroll : BaseStatItem
    {

        // Use this for initialization

        private int spellEffectID;
        public int SpellEffectID
        {
            get { return spellEffectID; }
            set { spellEffectID = value; }
        }
    }
}
