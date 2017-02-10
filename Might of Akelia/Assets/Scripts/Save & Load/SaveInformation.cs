using UnityEngine;
using System.Collections;


namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class SaveInformation : MonoBehaviour
    {

        // Use this for initialization
        public static void SaveAllInformation()
        {
            PlayerPrefs.SetInt("PLAYERLEVEL", GameInformation.PlayerLevel);
            PlayerPrefs.SetString("PLAYERNAME", GameInformation.PlayerName);
            PlayerPrefs.SetInt("Strength", GameInformation.Strength);
            PlayerPrefs.SetInt("Agility", GameInformation.Agility);
            PlayerPrefs.SetInt("Vitality", GameInformation.Vitality);
            PlayerPrefs.SetInt("Speed", GameInformation.Speed);
            PlayerPrefs.SetInt("Focus", GameInformation.Focus);
            PlayerPrefs.SetInt("Luck", GameInformation.Luck);
            PlayerPrefs.SetInt("Dexterity", GameInformation.Dexterity);
            PlayerPrefs.SetInt("Wisdom", GameInformation.Wisdom);
            PlayerPrefs.SetInt("Spirit", GameInformation.Spirit);
            PlayerPrefs.SetInt("Stamina", GameInformation.Stamina);

            Debug.Log("Saved All Information!!");

        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
