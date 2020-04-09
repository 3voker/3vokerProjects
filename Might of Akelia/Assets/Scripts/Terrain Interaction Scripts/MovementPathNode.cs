using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
/// <summary>
/// Hodes look for the path,
///
/// </summary>
public class MovementPathNode : MonoBehaviour, Ipoolable
{
    [SerializeField]
    public GameObject railSystem;

    MovementPath movementPath;

    public int lastItem;

    public int orderNumber;

    [SerializeField]
    bool isFirst;

    [SerializeField]
    bool isLast;

    [SerializeField]
    bool movementPathAlignment;

    [SerializeField]
    bool isPointofContact;

    [SerializeField]
    bool isGenerating;

    public bool isPartOfPath;

    Vector3 endPoint;

    Vector3 startPoint;

    void Awake()
    {

    }
    private void FixedUpdate()
    {
        //If movement Path is nonexistent, look for it. 
        if (movementPath == null)
        {
            isPartOfPath = false;
            railSystem = GetComponentInParent<MovementPath>().gameObject; //  railSystem = this.transform.root.gameObject;

            movementPath = railSystem.GetComponent<MovementPath>();
        }

        //If Movement path scripts grants permission to spawn, make node a part of the path
        if (movementPath.CanSpawn == false)
        {
            isPartOfPath = false;
        }

        else
            isPartOfPath = true;
        //isFirst = CheckIfFirst();

        //isLast = CheckIfLast();

        //isGenerating = movementPath.CanSpawn;

        //int nodesInList = movementPath.PathSequence.Count;

        //Transform lastItem = movementPath.PathSequence[nodesInList - 1];

        //movementPath = railSystem.GetComponent<MovementPath>();


        movementPathAlignment = CheckAlignment();
        }

    private bool CheckIfLast()
    {
        if (this.gameObject.tag == "Point B")
        {
            this.transform.position = endPoint;
            if (movementPath.PathSequence[lastItem] == this.gameObject)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    private bool CheckIfFirst()
    {
        if (this.gameObject.tag == "Point A")
        {
            this.transform.position = startPoint;
            if (movementPath.PathSequence[0] == this.gameObject)
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    private bool CheckAlignment()
    {
        if (isFirst && isLast)
        {
            return true;
        }
        else
            return false;
    }

    public void OnObjectSpawn()
    {
        this.gameObject.SetActive(true);
    }
    public void AssignParent(GameObject parent)
    {
        parent = railSystem;
        this.transform.parent = railSystem.transform;
    }
}
