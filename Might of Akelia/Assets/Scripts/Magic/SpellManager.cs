using UnityEngine;
using System.Collections;

public class SpellManager : MonoBehaviour {

    // Use this for initialization
    //Create an array of spells.
    //[SerializeField]
    //GameObject spellEmitter;
    [SerializeField]
    GameObject[] Spells;


    //[SerializeField]
    //GameObject[] spellPrefabs;

    // Animation animation;

    Animator animator;

    public int Spell;

    void start ()
    {
        //animation = GetComponent<Animation>();
        //animator = GetComponent<Animator>(); curSpell

    }
    public void CastSpell(int Spell)
    {
        //Uses the passed parameter in the method "loaded gun" 
        //GameObject temporarySpellHandler;
        //temporarySpellHandler = Instantiate(spellPrefabs[0], spellEmitter.transform.position, spellEmitter.transform.rotation) as GameObject;

        Spells[Spell].GetComponent<Animator>().SetTrigger("isActivated");
        //    Destroy(temporarySpellHandler, .25f);
        //
    }
}
