using UnityEngine;
using System.Collections;
using System;


[RequireComponent(typeof(AudioSource))]
public class InventoryObject : MonoBehaviour, IIdentifiable
{
    [SerializeField]
    string displayText;

    [SerializeField]
    string descriptionText;

    [SerializeField]
    string displayCommand;

    [SerializeField]
    InventoryManager inventoryManager;

    [SerializeField]
    GameObject player;

    private string itemName;
    private string itemDescription;
    private int itemID;
    public enum ItemTypes { EQUIPMENT, WEAPON, SCROLL, POTION, CHEST }

    private ItemTypes itemType;

    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }
    public string ItemDescription
    {
        get { return itemDescription; }
        set { itemDescription = value; }
    }
    public int ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }
    public ItemTypes ItemType
    {
        get { return itemType; }
        set { itemType = value; }
    }
    bool shouldDisableWhenDonePlayingSoundEffect = false;
    Animator anim;
    AudioSource audioSource;
    //Renderer changeColor;
    //Color colorStart;
    //Color colorEnd;
    float duration = 3;
    public string DescriptionText
    {
        get
        {
            return descriptionText;
        }
    }
    public string DisplayText
    {
        get
        {
            return displayText;
        }
    }
    public string DisplayInput
    {
        get
        {
            return displayCommand;
        }
    }

    public string DisplayName
    {
        get
        {
           return this.gameObject.name.ToString();
        }
    }

   

    public void DoCommand()
    {
        Animator anim = player.GetComponent<Animator>();
        anim.Play("");
        DoActivate();
    }
    public void DoActivate()
    {
        //What happens to inventory Object?
        //Goes away
        //Gets added to inventory list(to do)
        audioSource.Play();
        inventoryManager.InventoryObjects.Add(this);
        Debug.Log("Inventory Objects:" + inventoryManager.InventoryObjects[0]);
        shouldDisableWhenDonePlayingSoundEffect = true;
    }
    // Use this for initialization
    public void Start()
    {
        inventoryManager = GameObject.Find("Game Manager Canvas").GetComponent<InventoryManager>();
        anim = GetComponent<Animator>();
        //GetComponent<MeshRenderer>().enabled = true;
        //colorStart = GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        audioSource = GetComponent<AudioSource>();

        SetInventory(inventoryManager);

    }
    public void SetInventory(InventoryManager inv)
    {
        if (inventoryManager == null)
        {
            inventoryManager = inv;
        }
        try
        {
            inventoryManager = GameObject.Find("Game Manager Canvas").GetComponent<InventoryManager>();
        }
        catch (Exception)
        {
            if (inventoryManager == null)
            {

                throw new System.Exception("Scene must contain a gameObject named 'inventory Manager' " +
                        "With an InventoryManager Script Attached");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        Rotation();
        if (shouldDisableWhenDonePlayingSoundEffect && !audioSource.isPlaying)
        {
            Debug.Log("Should go away now..");
            gameObject.SetActive(false);
        }
    }
    void InventoryObjects()
    {

    }
    void Rotation()
    {
        transform.Rotate(0, 100 * Time.deltaTime, 0);
        if (shouldDisableWhenDonePlayingSoundEffect)
        {
            transform.Rotate(25, 300 * Time.deltaTime, 100);
            FadeOut();
        }
    }
    public void FadeOut()
    {
        if (shouldDisableWhenDonePlayingSoundEffect && !audioSource.isPlaying)
        {
            float t;
            float alpha = GetComponent<MeshRenderer>().material.color.a;
            for (t = 0.0f; t < 1.0f; t += Time.deltaTime / duration)
            {
                Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
                GetComponent<MeshRenderer>().material.color = newColor;
            }
            gameObject.SetActive(false);
        }
    }
}
