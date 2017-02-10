using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class CreateNewWeapon : MonoBehaviour
    {

        // Use this for initialization

        private BaseWeapon newWeapon;

          string[] equipmentItemCondition = new string[5] { "Broken", "Cracked", "Common", "Good", "Amazing"};
         string[] itemDescription = new string[2] { "Opened new chest! ", "Opened old chest" };
void Start()
        {
            CreateWeapon();
            Debug.Log(newWeapon.ItemName);
            Debug.Log(newWeapon.ItemDescription);
            Debug.Log("You've found a " + newWeapon.WeaponType.ToString() + ".");
            Debug.Log("Stamina " + newWeapon.Stamina.ToString());
            Debug.Log("Strength " + newWeapon.Strength.ToString());
        }
        public void CreateWeapon()
        {//Assign weapon name 
         //Create weapon description
         //weapon id
         //stats
         //type of weapon
         //spell effect id
            newWeapon = new BaseWeapon();
             newWeapon.ItemName = equipmentItemCondition[Random.Range(0, 5)];
            newWeapon.ItemName = "W" + Random.Range(1, 101);
            newWeapon.ItemDescription = "Weapon found!";
            newWeapon.ItemID = Random.Range(1, 101);
            newWeapon.Stamina = Random.Range(1, 11);
            newWeapon.Focus  = Random.Range(1, 11);
            newWeapon.Strength = Random.Range(1, 11);
            newWeapon.Speed = Random.Range(1, 11);
            newWeapon.SpellEffectID = Random.Range(1, 101);
            ChooseWeaponType();

        }

        // Update is called once per frame
        
        private void ChooseWeaponType()
        {
            int randomTemp = Random.Range(1, 14);
            if(randomTemp == 1)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.SWORD;
            }
            else if (randomTemp == 2)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.STAFF;
            }
            else if (randomTemp == 3)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.POLEARM;
            }
            else if (randomTemp == 4)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.AXE;
            }
            else if (randomTemp == 5)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.SHIELD;
            }
            else if (randomTemp == 6)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.DAGGER;
            }        
            else if (randomTemp == 6)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.GAUNTLETS;
            }
            else if (randomTemp == 6)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.BOW;
            }
            else if (randomTemp == 6)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.GUN;
            }
            else if (randomTemp == 10)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.DUALGUNS;
            }
            else if (randomTemp == 11)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.CANNON;
            }
            else if (randomTemp == 12)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.WAND;
            }
            else if (randomTemp == 13)
            {
                newWeapon.WeaponType = BaseWeapon.WeaponTypes.THROWING;
            }
        }
    }
}
