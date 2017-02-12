using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class Hazard : MonoBehaviour
    {

        // Use this for initialization
        

        void OnCollision(GameObject other)
        {
            ThirdPersonUserControl player = GetComponent<ThirdPersonUserControl>();
            if (other.gameObject.tag == "Player")
            {
                // Player.TakeDamage();
            }
        }
    }
}
