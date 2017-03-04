using UnityEngine;
using System.Collections;

public class TestGUI : MonoBehaviour {

    // Use this for initialization
    private BaseCharacterClass class1 = new BaseWarriorClass();
    private BaseCharacterClass class2 = new BaseMageClass();

    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
        GUILayout.Label(class1.CharacterClassName);      
        GUILayout.Label(class1.CharacterClassDescription);
        GUILayout.Label("Strength is now: " + class1.Strength.ToString());
        GUILayout.Label("Agility is now: " + class1.Agility.ToString());
        GUILayout.Label("Vitality is now: " + class1.Vitality.ToString());
        GUILayout.Label("Focus is now: " + class1.Focus.ToString());
        GUILayout.Label("Luck is now: " + class1.Luck.ToString());
        GUILayout.Label("Dexterity is now: " + class1.Dexterity.ToString());
        GUILayout.Label("Speed is now: " + class1.Speed.ToString());
        GUILayout.Label("Stamina is now: " + class1.Stamina.ToString());
        GUILayout.Label("Wisdom is now: " + class1.Wisdom.ToString());
        GUILayout.Label("Spirit is now: " + class1.Spirit.ToString());

        GUILayout.Label(class2.CharacterClassName);
        GUILayout.Label(class2.CharacterClassDescription);

        //OnGUI.class1.CharacterName;
        //OnGUI.class1.CharacterClassDescription;
        //OnGUI.class2.CharacterName;
        //OnGUI.class2.CharacterClassDescription;
    }
}
