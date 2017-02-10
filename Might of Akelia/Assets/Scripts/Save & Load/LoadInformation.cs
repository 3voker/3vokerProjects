using UnityEngine;
using System.Collections;


namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class LoadInformation : MonoBehaviour
    {

        // Use this for initialization
        public static void LoadAllinformation()
        {
            GameInformation.PlayerName = PlayerPrefs.GetString("PLAYERNAME");
            GameInformation.PlayerLevel = PlayerPrefs.GetInt("Level");
            GameInformation.Strength = PlayerPrefs.GetInt("Strength");
            GameInformation.Agility = PlayerPrefs.GetInt("Agility");
            GameInformation.Vitality = PlayerPrefs.GetInt("Vitality");
            GameInformation.Speed = PlayerPrefs.GetInt("Speed");
            GameInformation.Focus = PlayerPrefs.GetInt("Focus");
            GameInformation.Luck = PlayerPrefs.GetInt("Luck");
            GameInformation.Dexterity = PlayerPrefs.GetInt("Dexterity");
            GameInformation.Wisdom = PlayerPrefs.GetInt("Wisdom");
            GameInformation.Spirit = PlayerPrefs.GetInt("Spirit");
            GameInformation.Stamina = PlayerPrefs.GetInt("Stamina");
        }
    }
}
