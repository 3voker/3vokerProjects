using UnityEngine;
using System.Collections;

namespace UnitySampleAssets.Characters.ThirdPerson
{
    public class CreateNewScroll : MonoBehaviour {

    // Use this for initialization


    BaseScroll newScroll;
	void Start () {
            CreateScroll();
            Debug.Log(newScroll.ItemDescription);
            Debug.Log(newScroll.ItemID.ToString());
            // Debug.Log("You've found a " + newScroll.ScrollType + " potion.");
            //newScroll.ItemDescription = "This is a " + newScroll.ScrollType + " potion";
        }
	
	// Update is called once per frame
	private void CreateScroll()
        {
            newScroll = new BaseScroll();
            newScroll.ItemName = "Scroll";
            newScroll.ItemDescription = "This is a new powerful scroll!";
            newScroll.ItemID = Random.Range(1, 101);
            newScroll.SpellEffectID = Random.Range(1, 101);
        }
}
}
