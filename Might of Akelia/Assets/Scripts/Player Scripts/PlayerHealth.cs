using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Rook
{  
    [HideInInspector]
    public Canvas worldCanvas;
  
    public Color fullHealthColor = Color.green;
    public Color lowHealthColor = Color.red;
    public float yOffset = 2;
    public float width = 10.44f;

    private Slider healthBarSlider;
    private Vector3 offset;
    float playerHealthPoints;

    public Image currentHealthBar;
    public Text ratioText;

    public float PlayerMaxHealth
    {
        get { return playerHealthPoints; }
        set { playerHealthPoints = this.MaxHealthPoints; }
    }
  
    void OnEnable()
    {
        playerHealthPoints = this.HealthPoints;       
        UpdateHealthBar();
        isDestroyed = false;      
    }
  
    void Update()
    {
        UpdateHealthBar();      
    }

    private void UpdateHealthBar()
    {
        float ratio = this.healthPoints / this.MaxHealthPoints;
        currentHealthBar.color = Color.Lerp(lowHealthColor, fullHealthColor, this.healthPoints / this.MaxHealthPoints);
        currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        ratioText.text = (ratio * 100).ToString() + "%";
        offset = new Vector3(0, yOffset, 0);
        //Rework later for secondary UI healthbar for distant partyMembers & enemies
        //if(camera.transform.position >= maxDistanceRange)
        //{
        //    Instantiate(currentHealthBar, playerRook.gameObject.transform.position + offset, Quaternion.identity);
        //}
        //else
        //{ Destroy(currentHealthBar.gameObject); }       
    }
}
