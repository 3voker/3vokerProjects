using UnityEngine;
using System.Collections;
using System;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class BaseStatItem : InventoryObject
    {

        // Use this for initialization
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
}
