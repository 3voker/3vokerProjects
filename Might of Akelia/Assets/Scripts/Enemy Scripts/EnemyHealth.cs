using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    EnemyController enemyController;
  
    [Tooltip("Weather the turret is Destroyed or not")]
    public bool isDestroyed = false;
    private bool dying;
    [Tooltip("Turret Current Health")]
    //HUD script variable heartSprites converts this float to integer for array index 
    [HideInInspector]
    public Canvas worldCanvas;
    public GameObject healthBarPrefab;
    public float yOffset = 2;
    public float width = 10.44f;

    private Slider healthBar;
    private Enemy enemy;
   // private Enemy enemy;

    private Vector3 offset;

    public float MaxHealth { get; private set; }
   
    void Start()
    {
        enemyController = this.GetComponent<EnemyController>();
        enemy = this.GetComponent<Enemy>();
    }
    void OnStart()
    {
        UIConsoleText.SetConsoleText("Enemy Health: " + enemy.HealthPoints); //DEBUG HEALTH VISIBILIY
    }
    //Apply damage to Turret
    void Destroy()
    {
        enemyController.MeshCollider_Status(true);
        enemyController.isKinematicRigidbodies(false);
        enemyController._Raycast.TurretLaser_Status(false);
        Destroy(this.gameObject, 3);
    }
    void Awake()
    {
       // enemy = this.GetComponent<Enemy>();
        worldCanvas = GameObject.Find("World View Canvas").GetComponent<Canvas>();
        if (worldCanvas == null) { Debug.LogError("World canvas does not exist"); }
    }
    void OnEnable()
    {
        InitializeBar();
    }
    private void InitializeBar()
    {
        GameObject bar = Instantiate(healthBarPrefab, worldCanvas.transform) as GameObject;
        healthBar = bar.GetComponent<Slider>();
      //  healthBar.maxValue = enemy.MaxHealth;
        healthBar.GetComponent<RectTransform>().localScale = Vector3.one;
        healthBar.GetComponent<RectTransform>().sizeDelta = new Vector3(width, healthBar.GetComponent<RectTransform>().sizeDelta.y);
        offset = new Vector3(0, yOffset, 0);
    }
    void Update()
    {
        if (enemy.HealthPoints > 0 && healthBar != null)
        {
            healthBar.value = enemy.HealthPoints;
            healthBar.gameObject.transform.position = enemy.gameObject.transform.position + offset;
        }
        else
        {
            if (healthBar != null)
            { Destroy(healthBar.gameObject); }
            isDestroyed = true;
        }
    }
    void OnDisable()
    {
        if (healthBar != null)
        { Destroy(healthBar.gameObject); }
    }
}
