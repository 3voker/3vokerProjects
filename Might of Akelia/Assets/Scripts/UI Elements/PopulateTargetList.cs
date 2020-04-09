using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PopulateTargetList : MonoBehaviour
{

    // Use this for initialization
    Enemy enemyScript;
    CombatState combatState;
    private Transform targetPopulatorTransform;
    private List<Collider> nearbyTargets;
    private List<Collider> tempTargetsList;
    private Transform[] enemyTargets;
    private Array[] enemyList = new Array[5];
  
    GameObject mainTargetCursor;
   
    GameObject subTargetCursor;
    SpriteRenderer spriteRenderer;
    GameObject goMainTarget;
    GameObject goSubTarget;
    Collider selectedTarget;
    Vector3 mainTargetPosition;
    Vector3 subTargetPosition;
    Vector3 lookAtMainTarget;
    Color color;
    GameObject gameObjectEnemy;
    Collider mainTargetedEnemy;
    Collider subTargetedEnemy;
    bool mainTargetInList;
    bool subTarget1InList;
    bool subtarget2InList;
    bool subTarget3InList;
    bool subtarget4InList;
    bool lockedOn;
    [Header("Targeting Variables")]
    float ActiveTargetCountDown;
    [SerializeField]
    float enemyDistance;
    [SerializeField]
    float maxEnemyBattleDistance = 30f;
    void Start()
    {
        mainTargetCursor = GameObject.FindGameObjectWithTag("MainTargetCursor");

        subTargetCursor = GameObject.FindGameObjectWithTag("SubTargetCursor");
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainTargetedEnemy = GetComponent<BoxCollider>();
        subTargetedEnemy = GetComponent<BoxCollider>();
        nearbyTargets = new List<Collider>();
        //Collider[] nearbyTargets = FindObjectsOfType(typeof(GameObject)) as Collider[];
        tempTargetsList = new List<Collider>(); //Temporary targetList
        selectedTarget = null;
        targetPopulatorTransform = transform;
        //gameObjectEnemy = collider.gameObject.FindWithTag("Enemy");        
        CountAllColliders();
    }
    public void CountAllColliders()
    {
        Collider[] goArray = (Collider[])FindObjectsOfType(typeof(Collider));   //FindGameObjectsWithTag("Enemy");

    }
    public void AddTarget(Collider enemy)
    { //targets = nearbyTargets
        nearbyTargets.Add(enemy);

    }
    //if we do not have an enemy targeted yet, then find the clostest one and target him
    //if we do have an enemy targeted, then get the next target
    //if we have the last target in the list, then get then first target in the list
    private void SelectTarget()
    {
        //selectedTarget.renderer.material.color = Color.red;
        if (selectedTarget != null) { Debug.Log("Select Target: " + selectedTarget + "."); }

        //PlayerAttack pa = (PlayerAttack)GetComponent("PlayerAttack");
        //pa.target = selectedTarget.gameObject;
    }
    private void DeselectTarget()
    {
        Debug.Log("Deselect Target");
        //selectedTarget.renderer.material.color = Color.white;
        selectedTarget = null;
    }
    void Update()
    {

        //foreach (Collider obj in nearbyTargets) { Debug.Log(obj.name); }

        while (nearbyTargets.Remove(null)) ;
        nearbyTargets.Clear();
        //AddAllEnemies(); If you ever replace OnCollisionEnter() with a better function.     
    }
    //private void FixedUpdate()
    //{
    //    if (Input.GetAxisRaw("rightTrigger") != 0)
    //    {
    //        MainTarget(true);
    //        While there
    //}
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy: " + other);
            TargetEnemy(ref other);
        }
    }
    void OnTriggerStay(Collider other)
    {
        //if (other == gameObjectEnemy)
        //{
        //    TargetEnemy(other);
        //}
        //else if (other.transform.tag == "Hazard")
        //{
        //    //HazardTarget();
        //   // AddToTargetArray(other.transform);
        //    Debug.Log("Hazard Target: " + other.transform.name);
        //    //Yellow. RGBA is (1, 0.92, 0.016, 1)
        //}
        //else if (other.transform.tag == "Clue")
        //{
        //    //NPCTarget();
        //    //AddToTargetArray(other.transform);
        //    Debug.Log("Clue Target: " + other.transform.name);
        //    //Magenta. RGBA is (1, 0, 1, 1). Though I think purple would be better...
        //}
        //else if (other.transform.tag == "NPC")
        //{
        //    NPCTarget();
        //    //AddToTargetArray(other.transform);
        //    Debug.Log("NPC Target: " + other.transform.name);
        //    //Solid blue. RGBA is (0, 0, 1, 1).
        //}
        //else if (other.transform.tag == "Items")
        //{
        //    ConfirmTarget();
        //    //AddToTargetArray(other.transform);
        //    Debug.Log("Confirm Target: " + other.transform.name);
        //    //Solid green. RGBA is (0, 1, 0, 1).
        //}
        //Create a subtarget condition so that when arraylist is full or player 
        //is doing an action that only effects one target activate subTarget cursors.
        //Cursors over subtargets will be gray and slightly opaque.   
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Left Populate collider");
        if (!lockedOn || enemyDistance > maxEnemyBattleDistance)// || enemy.CurrentCombatState == CombatState.DieState
        {
            foreach (Collider farthestTarget in nearbyTargets)
            {
                RemoveFromTargetArray(farthestTarget, nearbyTargets);
            }
        }
    }
    private void TargetEnemy(ref Collider enemy)
    {
        int targetIndex = UnityEngine.Random.Range(0, nearbyTargets.Count); //Add enemy to index.        
        bool alreadyAddedEnemy = nearbyTargets.Contains(enemy); //check if enemy is already added
        mainTargetInList = nearbyTargets.IndexOf(enemy) != -1;
        subTarget1InList = false;
        subtarget2InList = false;
        subTarget3InList = false;
        subtarget4InList = false;//selectedTarget = collider in current index which SHOULD be [0]
        if (enemy != null)
        {
            nearbyTargets.Add(enemy);
            Debug.Log(enemy.name + " spotted!!!");
            //selectedTarget = nearbyTargets[0];
            nearbyTargets[targetIndex] = enemy;//other = enemy collider in current target index          
            foreach (Collider nearbyTarget in nearbyTargets)
            {
                for (bool alreadyAdded = false; alreadyAdded;)
                {
                    for (int i = 0; i < nearbyTargets.Count; i++)
                    {
                        if (nearbyTarget == nearbyTargets[i])
                        {
                            //Search if other is already added to nearbyTargets list.                            
                            //Debug.Log("foreach, for, for, if debug: " + selectedTarget.name);
                            alreadyAdded = true;
                            nearbyTargets.Remove(enemy);
                            i--;
                            break;
                        }
                        //Red RGBA is (1, 0, 0, 1)  
                        else if (!alreadyAdded)
                        {
                            //nearbyTargets.Add(enemy);                            
                            enemyScript.isTargeted = true;
                            AddToTargetArray(ref enemy, ref nearbyTargets);
                            alreadyAdded = true;
                            continue;
                        }
                        enemy = nearbyTargets[targetIndex];
                    }
                    //if (!alreadyAddedEnemy) { AddToTargetArray(ref enemy, ref nearbyTargets); continue; }                    
                }               
            }  
        }
       // if (!alreadyAddedEnemy) { AddToTargetArray(ref enemy, ref nearbyTargets);}
        if (enemy == null)
        {
            Debug.Log("No enemies in sight...");
        }
        mainTargetPosition = nearbyTargets[0].transform.position;
        lookAtMainTarget = nearbyTargets[0].transform.position;
        VectorTargets(enemy.transform.position, enemy.transform.position, lookAtMainTarget);

        Debug.Log("Target is selected Target" + nearbyTargets[0] + "at location: " + mainTargetPosition);
        Debug.Log("Main enemy location is: " + enemy.transform.position);
        //Debug.Log("Sub enemy location is: " + enemy.transform.position);
        Debug.Log("Player look at this target: " + lookAtMainTarget);
    }
    public void VectorTargets(Vector3 mainTargetPosition, Vector3 subTargetPosition, Vector3 lookAtMainTarget)
    {
        Debug.Log("VectorTargets Function");
        SortTargetsByDistance(); //SortTargetsByDistance(ref selectedTarget , nearbyTargets); 
        if (mainTargetInList)
        {  //tempTargetsList[0] = nearbyTargets[0];

            //  Debug.Log("No, Active targets!");         
            //mainTargetPosition = Vector3.zero;
            //lookAtMainTarget = Vector3.zero;
            //subTargetPosition = Vector3.zero;
            Debug.Log("VectorTargets Function: nearbytargets[0] is null");
        }
        //else if (nearbyTargets[1] == null)
        //{
        //    tempTargetsList[1] = nearbyTargets[1];
        //    subTargetPosition = Vector3.zero;
        //    Destroy(subTargetCursor);
        //    //   Debug.Log("No, Active targets!");
        //    Debug.Log("VectorTargets Function: nearbytargets[1] is null");
        //}
        else if (!mainTargetInList)// (nearbyTargets[0] != null)
        {
            
            nearbyTargets.Add(selectedTarget); //nearbyTargets.Add(selectedTarget);
            selectedTarget = nearbyTargets[0];
            mainTargetPosition = Vector3.zero;
            lookAtMainTarget = mainTargetPosition;
            MainTarget(); //MainTarget(lockedOn);
            Debug.Log("Main target is selected Target" + nearbyTargets[0] + "at location: " + mainTargetPosition);

        }
        else if (nearbyTargets[1] != null)
        {
            //SubTarget();
            SortTargetsByDistance();
            nearbyTargets[1].transform.position = subTargetPosition;
            subTargetPosition = subTargetCursor.transform.position;

            Debug.Log("SubTarget is selected Target" + nearbyTargets[1] + "at location: " + subTargetPosition);
        }
        else
        {
            int index = nearbyTargets.IndexOf(selectedTarget);
            if (index < nearbyTargets.Count - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            DeselectTarget();
            selectedTarget = nearbyTargets[index];
        }
        SelectTarget();
    }
    public void TransformTargets(GameObject mainTargetCursor, GameObject subTargetCursor)
    {
        bool checkMainTargetCursor = nearbyTargets.Contains(mainTargetedEnemy);
        bool checkSubTargetCursor = nearbyTargets.Contains(subTargetedEnemy);
        if (selectedTarget == null)
        {
            mainTargetCursor = null;
            // Destroy(mainTargetCursor);           
        }
        else if (nearbyTargets[1] == null)
        {
            subTargetCursor.SetActive(false);
        }
        else if (selectedTarget != null)
        {
            selectedTarget = nearbyTargets[0];
            nearbyTargets[0].transform.position = mainTargetCursor.transform.position;
        }
        else if (checkMainTargetCursor)// (nearbyTargets[1] != null)
        {
            //SubTarget();
            nearbyTargets[1].transform.position = subTargetCursor.transform.position;
        }
    }
    public void MainTarget()
    {
        lockedOn = true;
        //mainTargetPosition = lookAtMainTarget;
        TransformTargets(mainTargetCursor, subTargetCursor);
    }
    // private void EnemyTarget()
    //{
    //    throw new NotImplementedException();
    //}
    //private void ConfirmTarget()
    //{
    //    throw new NotImplementedException();
    //}
    //private void SubTarget()
    //{

    //}
    //private void HazardTarget()
    //{
    //    throw new NotImplementedException();
    //}
    //private void NPCTarget()
    //{
    //    throw new NotImplementedException();
    //}
    void AddToTargetArray(ref Collider addedTarget, ref List<Collider> targetList)
    {
        int targets = 1;
        goMainTarget = nearbyTargets[0].gameObject;
        goSubTarget = nearbyTargets[1].gameObject;

        //targetList.Add(selectedTarget); //(addedTarget)
                                        //targetList = Array.copy(nearbyTargets, targetList, nearbyTargets.Length);
        goMainTarget = Instantiate(mainTargetCursor, new Vector3(goMainTarget.transform.position.x, goMainTarget.transform.position.y, goMainTarget.transform.position.z), goMainTarget.transform.rotation) as GameObject;
        //foreach (Collider newTarget in targetList)
        //{
        //    if (newTarget.CompareTag("Enemy"))
        //    {
        //        targets++;
        //        for (int i = 0; i < targets; i++)
        //        {
        //            goSubTarget = Instantiate(subTargetCursor, new Vector3(goSubTarget.transform.position.x, goSubTarget.transform.position.y, goSubTarget.transform.position.z), goSubTarget.transform.rotation) as GameObject;
        //        }
        //    }
        //}
    }
    private void SortTargetsByDistance()
    {
        nearbyTargets.Sort(delegate (Collider t1, Collider t2)
        {
            t1 = nearbyTargets[0];
            t2 = nearbyTargets[1];
            return Vector3.Distance(t1.transform.position, targetPopulatorTransform.position).CompareTo(Vector3.Distance(t2.transform.position, targetPopulatorTransform.position));
        });
    }
    private void RemoveFromTargetArray(Collider farthestTarget, List<Collider> targetList)
    {
        ActiveTargetCountDown = 5;
        if (farthestTarget != null)
        {
            ActiveTargetCountDown--;
            if (ActiveTargetCountDown <= 0)
            {
                targetList.Remove(farthestTarget);
            }
        }
    }
    public void DoActivate()
    {
        throw new NotImplementedException();
    }
    public void DoCommand()
    {
        throw new NotImplementedException();
    }
}
