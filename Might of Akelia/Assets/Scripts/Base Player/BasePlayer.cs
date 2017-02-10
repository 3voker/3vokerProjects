using UnityEngine;
using System.Collections;
namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class BasePlayer 
    {

        // Use this for initialization
        string playerName;
        int playerLevel;
        BaseCharacterClass playerClass;

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

        //public string PlayerName
        //{
        //    get { return playerName; }
        //    set { playerName = value;}
        //}
        public string PlayerName { get; set; }
        public int PlayerLevel
        {
            get { return playerLevel; }
            set { playerLevel = value; }
        }
        public BaseCharacterClass PlayerClass
        {
            get { return playerClass; }
            set { playerClass = value; }
        }
        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }
        public int Agility
        {
            get { return agility; }
            set { agility = value; }
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
