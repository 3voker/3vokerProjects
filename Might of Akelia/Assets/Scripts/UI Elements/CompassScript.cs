using UnityEngine;
using System.Collections;
using System;

public class CompassScript : MonoBehaviour {

    // Use this for initialization
    GameObject player;
    Transform playerLocation;
    [SerializeField]
    Texture compBg;
    [SerializeField]
    Texture blipTex;

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, 120, 120),compBg);
        GUI.DrawTexture(CreateBlip(), blipTex);
    }

    private Rect CreateBlip()
    {
        player = GameObject.FindWithTag("Player");
        playerLocation = player.transform;
        float angDeg = playerLocation.eulerAngles.y - 90;
        float angRed = angDeg * Mathf.Deg2Rad;

        float blipX = 25 * Mathf.Cos(angRed);
        float blipY = 25 * Mathf.Sin(angRed);

        blipX += 55;
        blipY += 55;

        return new Rect(blipX, blipY, 10, 10);
    }
}
