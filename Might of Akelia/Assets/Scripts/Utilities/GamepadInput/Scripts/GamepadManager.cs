using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

//Enum of all avaliable buttons for convinience purposes
public enum GamepadButtons { A, B, X, Y, Dpad_Up, Dpad_Down, Dpad_Left, Dpad_Right, Guide, Back, Start, LStick, RStick, LBumper, RBumper }

public class GamepadManager : MonoBehaviour
{
    public int GamepadCount = 1; // Number of gamepads to support

    private List<Gamepad> gamepads;     // Holds gamepad instances
    private static GamepadManager singleton; // Singleton instance                                    
    public static GamepadManager Instance // Return instance
    {
        get
        {
            if (singleton == null)
            {
                Debug.LogError("[GamepadManager]: Instance does not exist!");
                return null;
            }

            return singleton;
        }
    }

    #region Input Map Constants 
    //Enum to String Converter
    public string GamepadButton(GamepadButtons button)
    {
        switch (button)
        {
            case GamepadButtons.A:
                return A_Button;
                break;
            case GamepadButtons.B:
                return B_Button;
                break;
            case GamepadButtons.X:
                return X_Button;
                break;
            case GamepadButtons.Y:
                return Y_Button;
                break;
            case GamepadButtons.Dpad_Up:
                return DPad_Up_Button;
                break;
            case GamepadButtons.Dpad_Down:
                return DPad_Down_Button;
                break;
            case GamepadButtons.Dpad_Left:
                return DPad_Left_Button;
                break;
            case GamepadButtons.Dpad_Right:
                return DPad_Right_Button;
                break;
            case GamepadButtons.Guide:
                return Guide_Button;
                break;
            case GamepadButtons.Back:
                return Back_Button;
                break;
            case GamepadButtons.Start:
                return Start_Button;
                break;
            case GamepadButtons.LStick:
                return LStick_Button;
                break;
            case GamepadButtons.RStick:
                return RStick_Button;
                break;
            case GamepadButtons.LBumper:
                return LBumper_Button;
                break;
            case GamepadButtons.RBumper:
                return RBumper_Button;
                break;
            default:
                return A_Button;
                break;
        }
    }

    public const string A_Button = "A";
    public const string B_Button = "B";
    public const string X_Button = "X";
    public const string Y_Button = "Y";

    public const string DPad_Up_Button = "DPad_Up";
    public const string DPad_Down_Button = "DPad_Down";
    public const string DPad_Left_Button = "DPad_Left";
    public const string DPad_Right_Button = "DPad_Right";

    public const string Guide_Button = "Guide";
    public const string Back_Button = "Back";
    public const string Start_Button = "Start";

    public const string LStick_Button = "L3";
    public const string RStick_Button = "R3";
    public const string LBumper_Button = "LB";
    public const string RBumper_Button = "RB";
    #endregion

    
    // Initialize on 'Awake'
    void Awake()
    {
        // Found a duplicate instance of this class, destroy it!
        if (singleton != null && singleton != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            // Create singleton instance
            singleton = this;
            DontDestroyOnLoad(this.gameObject);

            // Lock GamepadCount to supported range
            GamepadCount = Mathf.Clamp(GamepadCount, 1, 4);

            gamepads = new List<Gamepad>();

            // Create specified number of gamepad instances
            for (int i = 0; i < GamepadCount; ++i)
            {
                gamepads.Add(new Gamepad(i + 1));
            }
        }
    }

    // Normal Unity update
    void Update()
    {
        UpdateGamepads();
        //for (int i = 0; i < gamepads.Count; ++i)
        //{ print("gamepad # " + i + gamepads[i].IsConnected); }
    }

    void LateUpdate()
    {
        Refresh();
    }

    //Run the update on each gamepad instance
    private void UpdateGamepads()
    {
        for (int i = 0; i < gamepads.Count; ++i)
        { gamepads[i].Update(); }
    }

    // Refresh gamepad states for next update
    public void Refresh()
    {
        for (int i = 0; i < gamepads.Count; ++i)
        { gamepads[i].Refresh(); }
    }

    // Return specified gamepad
    // (Pass index of desired gamepad, eg. 1)
    public Gamepad GetGamepad(int index)
    {
        for (int i = 0; i < gamepads.Count;)
        {
            // Indexes match, return this gamepad
            if (gamepads[i].Index == (index - 1))
            { return gamepads[i]; }
            else
            { ++i; }
        }

        Debug.LogError("[GamepadManager]: " + index + " is not a valid gamepad index!");

        return null;
    }

    // Return number of connected gamepads
    public int ConnectedTotal()
    {
        int total = 0;

        for (int i = 0; i < gamepads.Count; ++i)
        {
            if (gamepads[i].IsConnected)
                total++;
        }

        return total;
    }

    // Check across all gamepads for button press.
    // Return true if the conditions are met by any gamepad
    public bool GetButtonAny(string button)
    {
        for (int i = 0; i < gamepads.Count; ++i)
        {
            // Gamepad meets both conditions
            if (gamepads[i].IsConnected && gamepads[i].GetButton(button))
                return true;
        }

        return false;
    }

    public bool GetButtonAny(GamepadButtons button)
    {
        return GetButtonAny(this.GamepadButton(button));
    }

    // Check across all gamepads for button press - CURRENT frame.
    // Return true if the conditions are met by any gamepad
    public bool GetButtonDownAny(string button)
    {
        for (int i = 0; i < gamepads.Count; ++i)
        {
            // Gamepad meets both conditions
            if (gamepads[i].IsConnected && gamepads[i].GetButtonDown(button))
                return true;
        }

        return false;
    }
}