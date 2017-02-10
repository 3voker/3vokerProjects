using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class CreateNewPotion : MonoBehaviour
    {

        // Use this for initialization

        BasePotion newPotion;

        void Start()
        {
            CreatePotion();
            Debug.Log(newPotion.ItemID.ToString());
            Debug.Log("You've found a " + newPotion.PotionType + " potion.");
            newPotion.ItemDescription = "This is a " + newPotion.PotionType + " potion";
        }

        // Update is called once per frame
      private void CreatePotion()
        {
           
            newPotion = new BasePotion();
            newPotion.ItemName = "Potion";
            ChoosePotionType();
            
            newPotion.ItemID = Random.Range(1, 101);
           
        }
      private void ChoosePotionType()
        {
            int randomTemp = Random.Range(0, 12);
            if(randomTemp == 0)
            {
                newPotion.PotionType = BasePotion.PotionTypes.HEALTH;
            }
            else if (randomTemp == 1)
            {
                newPotion.PotionType = BasePotion.PotionTypes.MAGIC;
            }
            else if (randomTemp == 2)
            {
                newPotion.PotionType = BasePotion.PotionTypes.ENERGY;
            }
            else if (randomTemp == 3)
            {
                newPotion.PotionType = BasePotion.PotionTypes.STRENGTH;
            }
            else if (randomTemp == 4)
            {
                newPotion.PotionType = BasePotion.PotionTypes.AGILITY;
            }
            else if (randomTemp == 5)
            {
                newPotion.PotionType = BasePotion.PotionTypes.VITALITY;
            }
            else if (randomTemp == 6)
            {
                newPotion.PotionType = BasePotion.PotionTypes.SPEED;
            }
            else if (randomTemp == 7)
            {
                newPotion.PotionType = BasePotion.PotionTypes.FOCUS;
            }
            else if (randomTemp == 8)
            {
                newPotion.PotionType = BasePotion.PotionTypes.LUCK;
            }
            else if (randomTemp == 9)
            {
                newPotion.PotionType = BasePotion.PotionTypes.DEXTERITY;
            }
            else if (randomTemp == 10)
            {
                newPotion.PotionType = BasePotion.PotionTypes.WISDOM;
            }
            else if (randomTemp == 11)
            {
                newPotion.PotionType = BasePotion.PotionTypes.SPIRIT;
            }
            else if (randomTemp == 12)
            {
                newPotion.PotionType = BasePotion.PotionTypes.STAMINA;
            }
        }
    }
}
