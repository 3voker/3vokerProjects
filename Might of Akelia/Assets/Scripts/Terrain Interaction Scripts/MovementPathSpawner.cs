using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a movement path node 
/// </summary>
public class MovementPathSpawner : MonoBehaviour
{
    ObjectPooler objectPooler;

    public MovementPathNode movementPathNode;
    MovementPath movementPath;


    [SerializeField]
    GameObject railSystem;

    //[SerializeField]
    //GameObject Node;

    int numberInQueue;

    private void Awake()
    {
        movementPath = GetComponentInParent<MovementPath>();
        railSystem = movementPath.gameObject;

        objectPooler = ObjectPooler.Instance;
    }
    private void Update()
    {
    //  if()
    }
    public List<Transform> SpawnPath(List<Transform> PathSequence)
    {      
        List<Transform> PathSpawned = new List<Transform>(10);

        PathSpawned.AddRange(new Transform[9]);

        //int nodesInList = PathSequence.Count;

        //Transform lastItem = PathSequence[nodesInList - 1];

        //if (movementPath.pointB != lastItem)
        //{
        //    PathSequence.Remove(movementPath.pointB);
        //    PathSequence.Add(movementPath.pointB);
        //}

        PathSequence = PathSpawned;
       // movementPath.pointB = lastItem;

        PathSequence = PathSequence.Distinct().ToList();
        return PathSequence;
    }

    /// <summary>
    /// Sets the node in the next position
    /// </summary>
    /// <param name="PathSequence"></param>
    /// <param name="obj"></param>
    /// <param name="objectPosition"></param>
    /// <returns></returns>
    public Vector3 SetSpawnPoint(List<Transform> PathSequence, Transform obj, Vector3 objectPosition)
    {
        for (int i = 0; i < PathSequence.Count; i++)
        {

            foreach (ObjectPooler.Pool item in objectPooler.pools)
            {
                if (item.Equals(typeof(Transform)))
                {
                    if (PathSequence[1].transform != null)
                    {

                    }
                }
            }
        }

        return objectPosition;
    }

    public void SpawnNode(string name, Vector3 position)
    {
        //"RailPointPrefab"
        ObjectPooler.Instance.SpawnFromPool(name, position, Quaternion.identity);        
    }
    public GameObject GetPooledObject(GameObject obj)
    {
        for (int i = 0; i < movementPath.PathSequence.Count; i++)
        {
            if (!movementPath.PathSequence[i].gameObject.activeInHierarchy)
            {
                return movementPath.PathSequence[i].gameObject;
            }
        }
        obj.SetActive(true);
        movementPath.PathSequence.Add(obj.transform);
        return obj;
    }
}
