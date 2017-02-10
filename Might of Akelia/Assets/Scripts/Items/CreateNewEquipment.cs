using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class CreateNewEquipment : MonoBehaviour {

        // Use this for initialization
        //Instantiate BaseEquipment for access to base stats and equipment types
        BaseEquipment newEquipment;
        //Array for equipments condition, new, old, broken, etc...
        string[] equipmentItemCondition = new string[5] { "broken", "cracked", "common", "good", "sturdy"};
        //Array for the description of equipment, would go well to describe durability
        string[] itemDescription = new string[2] { "Opened new chest! ", "Opened old chest" };
    void Start() {

            CreateEquipment();
         //   Debug.Log(newEquipment.ItemName);
            Debug.Log(newEquipment.ItemDescription);
            Debug.Log("You've found a " + newEquipment.ItemName + 
                " " + newEquipment.EquipmentType.ToString() + " Armor.");
            Debug.Log("Stamina " + newEquipment.Stamina.ToString());
            Debug.Log("Strength " + newEquipment.Strength.ToString());
        }
    private void CreateEquipment()
        {
            newEquipment = new BaseEquipment();
            newEquipment.ItemName = equipmentItemCondition[Random.Range(0, 5)];
            newEquipment.ItemID = Random.Range(1, 101);
            ChooseItemType();
            newEquipment.ItemDescription = itemDescription[Random.Range(0, itemDescription.Length)];
            newEquipment.ItemID = Random.Range(1, 101);
            newEquipment.Stamina = Random.Range(1, 11);
            newEquipment.Focus = Random.Range(1, 11);
            newEquipment.Strength = Random.Range(1, 11);
            newEquipment.Speed = Random.Range(1, 11);
            newEquipment.SpellEffectID = Random.Range(1, 101);
        }
    private void ChooseItemType()
        {
            int randomTemp = Random.Range(1, 9);
            if(randomTemp == 1)
            {
                newEquipment.EquipmentType = BaseEquipment.EquipmentTypes.HEAD;
            }
            else if (randomTemp == 2)
            {
                newEquipment.EquipmentType = BaseEquipment.EquipmentTypes.CHEST;
            }
            else if (randomTemp == 3)
            {
                newEquipment.EquipmentType = BaseEquipment.EquipmentTypes.HANDS;
            }
            else if (randomTemp == 4)
            {
                newEquipment.EquipmentType = BaseEquipment.EquipmentTypes.LEG;
            }
            else if (randomTemp == 5)
            {
                newEquipment.EquipmentType = BaseEquipment.EquipmentTypes.FEET;
            }
            else if (randomTemp == 6)
            {
                newEquipment.EquipmentType = BaseEquipment.EquipmentTypes.NECKLACE;
            }
            else if (randomTemp == 7)
            {
                newEquipment.EquipmentType = BaseEquipment.EquipmentTypes.RING1;
            }
            else if (randomTemp == 8)
            {
                newEquipment.EquipmentType = BaseEquipment.EquipmentTypes.RING2;
            }
        }

        // Update is called once per frame
     
    }
}
