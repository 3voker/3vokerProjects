using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingHazard : MonoBehaviour {

    /// <summary>
    /// Alter players HP to either Take Damage or Recover health 
    /// </summary>
    /// 
    //Bool that determines if player will be damaged
    [SerializeField]
    private bool isDamaging;

    [SerializeField]
    private float damage = 10;

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            other.SendMessage((isDamaging) ? "TakeDamage" : "RecoverHealth", Time.deltaTime * damage);
        }
    }

}
