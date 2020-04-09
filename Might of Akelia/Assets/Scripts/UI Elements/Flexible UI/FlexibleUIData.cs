using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIData : ScriptableObject
{
    /// <summary>
    /// Flexible UI Data to determine the many sprite elements 
    /// to change depending upon certain conditions met. 
    /// </summary>

    //The button being chosen
    public Sprite buttonSprite;
    //The state of the button interacted with
    public SpriteState buttonSpriteState;

    //The default color 
    public Color defaultColor;
    //The default icon 
    public Sprite defaultIcon;

    //The confirm color 
    public Color confirmColor;
    //The confirm icon 
    public Sprite confirmIcon;

    //The decline color 
    public Color declineColor;
    //The decline icon 
    public Sprite declineIcon;

    //The warning color 
    public Color warningColor;
    //The warning icon 
    public Sprite warningIcon;

}
