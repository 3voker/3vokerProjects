using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class BaseWeapon : BaseStatItem
    {
        //BaseWeapon <- BaseStatItem < InventoryObject
        // Use this for initialization
      public enum WeaponTypes
        {
            SWORD,          
            STAFF,
            POLEARM,
            AXE,          
            SHIELD,           
            DAGGER,
            GAUNTLETS,
            BOW,
            GUN, 
            DUALGUNS,
            CANNON,
            WAND, 
            THROWING
        }
        private WeaponTypes weaponType;
        private int spellEffectID;

        public WeaponTypes WeaponType
        {
            get { return weaponType; }
            set { weaponType = value; }
        }
        public int SpellEffectID
        {
            get { return spellEffectID; }
            set { spellEffectID = value; }
        }
    }
}
