using UnityEngine;
using System.Collections;

public class GameInformation : MonoBehaviour {

	// Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
	public static string PlayerName { get; set; }
    public static int PlayerLevel { get; set; }
    public static BaseCharacterClass PlayerClass { get; set; }
    public static int Strength { get; set; }
    public static int Agility { get; set; }
    public static int Vitality { get; set; }
    public static int Focus { get; set; }
    public static int Luck { get; set; }
    public static int Dexterity { get; set; }
    public static int Speed { get; set; }
    public static int Stamina { get; set; }
    public static int Wisdom { get; set; }
    public static int Spirit { get; set; }
}
