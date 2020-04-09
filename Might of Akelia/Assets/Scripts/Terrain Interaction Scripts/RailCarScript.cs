using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCarScript : MonoBehaviour {

    [SerializeField]
    GameObject railCar;
  
    [SerializeField]
    FollowPath followRailBalancerPath;

    /// <summary>
    /// Toggle to invert the Y axis when grinding rails(Tilts on the X axis) 
    /// </summary>
    public bool invertY;

    /// <summary>
    /// Toggle to invert the X axis when grinding rails(Tilts on the Z axis)
    /// </summary>
    public bool invertX;
  
    /// <summary>
    /// Amplifies input of players input to add rotation value
    /// </summary>
    public float rotationalPlayerInputBuffer = 30;

    void Start ()
    {  
        railCar = this.gameObject;
        //railBalancer = GameObject.FindGameObjectWithTag("Rail Balancer");
        //railCar.transform.forward = railBalancer.transform.forward;
    }

    public Quaternion SetRailCarPath(float playerYInput, float railBalancerYAxis, float playerXInput)
    {
        //Assign player y input to the railCar X rotational axis 
        playerYInput = Input.GetAxis("leftJoystickVertical");
        if (invertY) { playerYInput = -Input.GetAxis("leftJoystickVertical"); }

        //Assign player x input to the railCar Z rotational axis 
        playerXInput = Input.GetAxis("leftJoystickHorizontal");
        if (invertX) { playerXInput = -Input.GetAxis("leftJoystickHorizontal"); }

        Quaternion rotationalValue = Quaternion.Euler(playerYInput * rotationalPlayerInputBuffer, railBalancerYAxis, playerXInput * rotationalPlayerInputBuffer);

        return rotationalValue;
    }
}
