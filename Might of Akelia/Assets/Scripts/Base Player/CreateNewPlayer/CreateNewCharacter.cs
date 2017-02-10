using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnitySampleAssets.Characters.ThirdPerson;

public class CreateNewCharacter : MonoBehaviour
    {
        // Use this for initialization
        private BasePlayer newPlayer;        
        bool isWarriorClass;
        bool isRogueClass;
        bool isSupportClass;
        bool isMageClass;
        string playerName = "Enter Name";     
        // Update is called once per frame
        void Update()
        {
            newPlayer = new BasePlayer();
        }
        void OnGUI()
        {
            playerName = GUILayout.TextArea(playerName, 15);
            isWarriorClass = GUILayout.Toggle(isWarriorClass, "Warrior Class");
            isRogueClass = GUILayout.Toggle(isRogueClass, "Rogue Class");
            isSupportClass = GUILayout.Toggle(isSupportClass, "Support Class");
            isMageClass = GUILayout.Toggle(isMageClass, "Mage Class");

            if (GUILayout.Button("Create"))
            {
                if (isWarriorClass)
                {
                    newPlayer.PlayerClass = new BaseWarriorClass();
                }
                else if (isRogueClass)
                {
                    newPlayer.PlayerClass = new BaseRogueClass();
                }
                else if (isSupportClass)
                {
                    newPlayer.PlayerClass = new BaseSupportClass();
                }
                else if (isMageClass)
                {
                    newPlayer.PlayerClass = new BaseMageClass();
                }
                newPlayer.PlayerName = playerName;
                newPlayer.PlayerLevel = 1;
                newPlayer.Strength = newPlayer.PlayerClass.Strength;
                newPlayer.Agility = newPlayer.PlayerClass.Agility;
                newPlayer.Vitality = newPlayer.PlayerClass.Vitality;
                newPlayer.Speed = newPlayer.PlayerClass.Speed;
                newPlayer.Focus = newPlayer.PlayerClass.Focus;
                newPlayer.Luck = newPlayer.PlayerClass.Luck;
                newPlayer.Dexterity = newPlayer.PlayerClass.Dexterity;
                newPlayer.Wisdom = newPlayer.PlayerClass.Wisdom;
                newPlayer.Spirit = newPlayer.PlayerClass.Spirit;
                newPlayer.Stamina = newPlayer.PlayerClass.Stamina;
                //Save & Load...
                StoreNewPlayerInfo();
                SaveInformation.SaveAllInformation();
                //Print to Log the level, class, and stats. 
                Debug.Log("Player Name: " + newPlayer.PlayerName);
                Debug.Log("Player Class: " + newPlayer.PlayerClass.CharacterClassName);
                Debug.Log("Player Level: " + newPlayer.PlayerLevel);
                Debug.Log("Player Strength: " + newPlayer.Strength);
                Debug.Log("Player Agility: " + newPlayer.Agility);
                Debug.Log("Player Vitality: " + newPlayer.Vitality);
                Debug.Log("Player Speed: " + newPlayer.Speed);
                Debug.Log("Player Focus: " + newPlayer.Focus);
                Debug.Log("Player Luck: " + newPlayer.Luck);
                Debug.Log("Player Dexterity: " + newPlayer.Dexterity);
                Debug.Log("Player Wisdom: " + newPlayer.Wisdom);
                Debug.Log("Player Spirit: " + newPlayer.Spirit);
                Debug.Log("Player Stamina: " + newPlayer.Stamina);
            }
            if (GUILayout.Button("Load"))
            {
               // SceneManager.LoadScene("Test Scene");
            }

        }

        private void StoreNewPlayerInfo()
        {
            GameInformation.PlayerName = newPlayer.PlayerName;
            GameInformation.PlayerLevel = newPlayer.PlayerLevel;
            GameInformation.Strength = newPlayer.Strength;
            GameInformation.Agility = newPlayer.Agility;
            GameInformation.Vitality = newPlayer.Vitality;
            GameInformation.Speed = newPlayer.Speed;
            GameInformation.Focus = newPlayer.Focus;
            GameInformation.Luck = newPlayer.Luck;
            GameInformation.Dexterity = newPlayer.Dexterity;
            GameInformation.Wisdom = newPlayer.Wisdom;
            GameInformation.Spirit = newPlayer.Spirit;
            GameInformation.Stamina = newPlayer.Stamina;
        }
    }

