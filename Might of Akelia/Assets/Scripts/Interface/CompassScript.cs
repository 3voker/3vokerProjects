using UnityEngine;
using System.Collections;
using System;

public class CompassScript : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Transform player;
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
        float angDeg = player.eulerAngles.y - 90;
        float angRed = angDeg * Mathf.Deg2Rad;

        float blipX = 25 * Mathf.Cos(angRed);
        float blipY = 25 * Mathf.Sin(angRed);

        blipX += 55;
        blipY += 55;

        return new Rect(blipX, blipY, 10, 10);
    }
}
